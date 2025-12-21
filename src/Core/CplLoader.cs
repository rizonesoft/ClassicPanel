using System.Runtime.InteropServices;
using System.IO;
using ClassicPanel.Core;

namespace ClassicPanel.Core;

/// <summary>
/// Represents a single Control Panel applet item.
/// </summary>
public class CplItem
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public nint Handle { get; set; } // Handle to the loaded DLL
    public Icon? Icon { get; set; }
    public int AppletIndex { get; set; }
}

/// <summary>
/// Loads and manages Control Panel applets from the system folder.
/// </summary>
public class CplLoader
{
    private readonly List<CplItem> _items = new();

    /// <summary>
    /// Gets all loaded applet items.
    /// </summary>
    public IReadOnlyList<CplItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Loads all .cpl files from the system folder next to the executable.
    /// </summary>
    public void LoadSystemFolder()
    {
        _items.Clear();

        // 1. Locate the 'system' folder next to the .exe
        string exePath = AppDomain.CurrentDomain.BaseDirectory;
        string systemPath = Path.Combine(exePath, "system");

        if (!Directory.Exists(systemPath))
        {
            try
            {
                Directory.CreateDirectory(systemPath);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("Failed to create system folder", ex, "CplLoader.LoadSystemFolder");
            }
            return;
        }

        // 2. Scan for .cpl files with retry logic for transient errors
        try
        {
            string[] files = ErrorRecovery.Retry(
                () => Directory.GetFiles(systemPath, "*.cpl"),
                maxRetries: 2,
                initialDelayMs: 50,
                isTransientError: ErrorRecovery.IsTransientFileError,
                context: "CplLoader.LoadSystemFolder.GetFiles"
            );

            foreach (var file in files)
            {
                try
                {
                    // Retry loading individual CPL files for transient errors
                    ErrorRecovery.Retry(
                        () => LoadCplFile(file),
                        maxRetries: 1,
                        initialDelayMs: 100,
                        isTransientError: ex => ex is IOException && ErrorRecovery.IsTransientFileError(ex),
                        context: $"CplLoader.LoadCplFile({Path.GetFileName(file)})"
                    );
                }
                catch (Exception ex)
                {
                    // Log error but continue with other files
                    // Non-transient errors (e.g., BadImageFormatException, DllNotFoundException) are expected
                    // for invalid or incompatible CPL files, so we just log and continue
                    var errorInfo = ErrorInfo.FromCplLoadError(file, ex);
                    errorInfo.ShowToUser = false; // Don't show to user for individual file failures
                    ErrorLogger.LogError(errorInfo);
                }
            }
        }
        catch (Exception ex)
        {
            // Log error if directory access fails after retries
            ErrorLogger.LogError("Failed to scan system folder for CPL files", ex, "CplLoader.LoadSystemFolder");
        }
    }

    private void LoadCplFile(string filePath)
    {
        nint hModule = nint.Zero;
        
        try
        {
            // 3. Load the Library
            hModule = NativeLibrary.Load(filePath);
            if (hModule == nint.Zero)
            {
                throw new InvalidOperationException($"Failed to load native library: {filePath}");
            }

            // 4. Find the entry point
            if (!NativeLibrary.TryGetExport(hModule, "CPlApplet", out nint procAddress))
            {
                // Not a valid CPL file - unload and return
                NativeLibrary.Free(hModule);
                return;
            }

            // 5. Get delegate for CPlApplet function
            var applet = Marshal.GetDelegateForFunctionPointer<CplInterop.CPlAppletDelegate>(procAddress);
            if (applet == null)
            {
                throw new InvalidOperationException($"Failed to get delegate for CPlApplet in: {filePath}");
            }

            // 6. Initialize
            int initResult = applet(nint.Zero, CplInterop.CPL_INIT, nint.Zero, nint.Zero);
            if (initResult != 0)
            {
                // Initialization failed - unload and return
                NativeLibrary.Free(hModule);
                return;
            }

            // 7. Get Count of applets inside this file
            int count = applet(nint.Zero, CplInterop.CPL_GETCOUNT, nint.Zero, nint.Zero);
            if (count <= 0)
            {
                // No applets in this file - unload and return
                NativeLibrary.Free(hModule);
                return;
            }

            // 8. Create items for each applet
            for (int i = 0; i < count; i++)
            {
                // Get Info (TODO: Implement proper marshaling for CPL_INQUIRE/CPL_NEWINQUIRE)
                var item = new CplItem
                {
                    Path = filePath,
                    AppletIndex = i,
                    Name = Path.GetFileName(filePath),
                    Handle = hModule
                };

                _items.Add(item);
            }
        }
        catch (DllNotFoundException ex)
        {
            // Library not found - log and rethrow
            throw new DllNotFoundException($"Control Panel library not found: {filePath}", ex);
        }
        catch (BadImageFormatException ex)
        {
            // Invalid DLL format (e.g., wrong architecture) - log and rethrow
            throw new BadImageFormatException($"Invalid Control Panel library format: {filePath}", ex);
        }
        catch (Exception)
        {
            // Clean up on error
            if (hModule != nint.Zero)
            {
                try
                {
                    NativeLibrary.Free(hModule);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
            
            // Re-throw to be handled by caller
            throw;
        }
    }
}


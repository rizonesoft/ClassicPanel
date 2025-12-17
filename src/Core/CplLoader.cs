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
            Directory.CreateDirectory(systemPath);
            return;
        }

        // 2. Scan for .cpl files
        foreach (var file in Directory.GetFiles(systemPath, "*.cpl"))
        {
            try
            {
                LoadCplFile(file);
            }
            catch (Exception ex)
            {
                // Log error but continue with other files
                System.Diagnostics.Debug.WriteLine($"Failed to load CPL file '{file}': {ex.Message}");
            }
        }
    }

    private void LoadCplFile(string filePath)
    {
        // 3. Load the Library
        nint hModule = NativeLibrary.Load(filePath);

        // 4. Find the entry point
        if (NativeLibrary.TryGetExport(hModule, "CPlApplet", out nint procAddress))
        {
            var applet = Marshal.GetDelegateForFunctionPointer<CplInterop.CPlAppletDelegate>(procAddress);

            // 5. Initialize
            if (applet(nint.Zero, CplInterop.CPL_INIT, nint.Zero, nint.Zero) == 0)
            {
                // 6. Get Count of applets inside this file
                int count = applet(nint.Zero, CplInterop.CPL_GETCOUNT, nint.Zero, nint.Zero);

                for (int i = 0; i < count; i++)
                {
                    // 7. Get Info (TODO: Implement proper marshaling for CPL_INQUIRE/CPL_NEWINQUIRE)
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
        }
    }
}


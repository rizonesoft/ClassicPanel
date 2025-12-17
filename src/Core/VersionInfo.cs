using System.Reflection;

namespace ClassicPanel.Core;

/// <summary>
/// Provides version information for ClassicPanel.
/// Version information is automatically extracted from the assembly.
/// </summary>
public static class VersionInfo
{
    /// <summary>
    /// Gets the full version string (e.g., "0.1.0").
    /// </summary>
    public static string Version { get; }

    /// <summary>
    /// Gets the version as a System.Version object.
    /// </summary>
    public static Version VersionObject { get; }

    /// <summary>
    /// Gets the assembly version.
    /// </summary>
    public static string AssemblyVersion { get; }

    /// <summary>
    /// Gets the file version.
    /// </summary>
    public static string FileVersion { get; }

    /// <summary>
    /// Gets the product name.
    /// </summary>
    public static string ProductName { get; }

    /// <summary>
    /// Gets the company name.
    /// </summary>
    public static string Company { get; }

    /// <summary>
    /// Gets the copyright information.
    /// </summary>
    public static string Copyright { get; }

    /// <summary>
    /// Gets the product description.
    /// </summary>
    public static string Description { get; }

    /// <summary>
    /// Gets the build date (compilation time).
    /// </summary>
    public static DateTime BuildDate { get; }

    static VersionInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();

        // Get version from assembly
        VersionObject = assemblyName.Version ?? new Version(0, 1, 0);
        Version = VersionObject.ToString(3); // Major.Minor.Build (no revision)
        AssemblyVersion = VersionObject.ToString();

        // Get file version attribute
        var fileVersionAttr = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        FileVersion = fileVersionAttr?.Version ?? Version;

        // Get informational version (can include pre-release info)
        var infoVersionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        var informationalVersion = infoVersionAttr?.InformationalVersion ?? Version;

        // Get product information
        var productAttr = assembly.GetCustomAttribute<AssemblyProductAttribute>();
        ProductName = productAttr?.Product ?? "ClassicPanel";

        var companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        Company = companyAttr?.Company ?? "Rizonesoft";

        var copyrightAttr = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
        Copyright = copyrightAttr?.Copyright ?? "Copyright © 2025 Rizonetech (Pty) Ltd";

        var descriptionAttr = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
        Description = descriptionAttr?.Description ?? "A portable classic Windows Control Panel interface using .NET 10";

        // Get build date from assembly (if available via build date attribute)
        // For now, use assembly last write time as fallback
        var buildDateAttr = assembly.GetCustomAttribute<AssemblyMetadataAttribute>();
        if (buildDateAttr != null && buildDateAttr.Key == "BuildDate" && DateTime.TryParse(buildDateAttr.Value, out var parsedDate))
        {
            BuildDate = parsedDate;
        }
        else
        {
            // Fallback to file write time
            // For single-file apps, Assembly.Location is empty, so use AppContext.BaseDirectory
            string? assemblyPath = null;
            try
            {
                var location = assembly.Location;
                if (!string.IsNullOrEmpty(location) && File.Exists(location))
                {
                    assemblyPath = location;
                }
            }
            catch
            {
                // Assembly.Location may throw in some contexts
            }

            if (assemblyPath == null)
            {
                // Try to find the assembly in AppContext.BaseDirectory
                var name = assembly.GetName().Name;
                if (!string.IsNullOrEmpty(name))
                {
                    var possiblePath = Path.Combine(AppContext.BaseDirectory, name + ".dll");
                    if (File.Exists(possiblePath))
                    {
                        assemblyPath = possiblePath;
                    }
                }
            }

            if (assemblyPath != null && File.Exists(assemblyPath))
            {
                var fileInfo = new FileInfo(assemblyPath);
                BuildDate = fileInfo.LastWriteTime;
            }
            else
            {
                // Final fallback to current time
                BuildDate = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Gets a formatted version string for display.
    /// </summary>
    /// <returns>A formatted version string.</returns>
    public static string GetDisplayVersion()
    {
        return $"{ProductName} {Version}";
    }

    /// <summary>
    /// Gets detailed version information for about dialogs.
    /// </summary>
    /// <returns>A formatted string with detailed version information.</returns>
    public static string GetDetailedVersionInfo()
    {
        return $"{ProductName} {Version}\n" +
               $"Assembly Version: {AssemblyVersion}\n" +
               $"File Version: {FileVersion}\n" +
               $"Build Date: {BuildDate:yyyy-MM-dd HH:mm:ss}\n" +
               $"{Copyright}\n" +
               $"{Company}";
    }
}


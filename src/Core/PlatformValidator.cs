using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClassicPanel.Core;

/// <summary>
/// Validates that the platform meets ClassicPanel's requirements:
/// - Windows 10 or Windows 11 only
/// - 64-bit (x64) architecture only
/// </summary>
public static class PlatformValidator
{
    // Windows version constants
    private const int WINDOWS_10_MIN_BUILD = 10240; // Windows 10 RTM
    private const int WINDOWS_11_MIN_BUILD = 22000; // Windows 11 RTM

    /// <summary>
    /// Validates the current platform meets ClassicPanel requirements.
    /// </summary>
    /// <returns>True if platform is valid, false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown when platform doesn't meet requirements.</exception>
    public static bool ValidatePlatform()
    {
        // Check if running on Windows
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ShowErrorAndExit(
                "Unsupported Operating System",
                "ClassicPanel only supports Windows 10 and Windows 11.\n\n" +
                "Your operating system is not supported."
            );
            return false;
        }

        // Check architecture (must be x64)
        if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
        {
            ShowErrorAndExit(
                "Unsupported Architecture",
                "ClassicPanel requires a 64-bit (x64) system.\n\n" +
                $"Detected architecture: {RuntimeInformation.ProcessArchitecture}\n" +
                "Please run ClassicPanel on a 64-bit Windows system."
            );
            return false;
        }

        // Check OS version (Windows 10 or 11 only)
        var osVersion = GetWindowsVersion();
        if (osVersion == null)
        {
            ShowErrorAndExit(
                "Unable to Determine Windows Version",
                "ClassicPanel could not determine your Windows version.\n\n" +
                "ClassicPanel requires Windows 10 (build 10240+) or Windows 11."
            );
            return false;
        }

        if (!IsSupportedWindowsVersion(osVersion.Value))
        {
            var versionName = GetWindowsVersionName(osVersion.Value);
            var isOldWindows = osVersion.Value.Major == 6; // Windows 7, 8, or 8.1

            string message;
            if (isOldWindows)
            {
                message = "ClassicPanel does not support Windows 7, Windows 8, or Windows 8.1.\n\n" +
                         $"Detected version: {versionName}\n\n" +
                         "ClassicPanel requires Windows 10 (build 10240 or later) or Windows 11.\n\n" +
                         "Please upgrade to a supported version of Windows to use ClassicPanel.";
            }
            else
            {
                message = "ClassicPanel only supports Windows 10 and Windows 11.\n\n" +
                         $"Detected version: {versionName}\n\n" +
                         "Please upgrade to Windows 10 (build 10240 or later) or Windows 11 to use ClassicPanel.";
            }

            ShowErrorAndExit("Unsupported Windows Version", message);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the current Windows version information.
    /// </summary>
    /// <returns>Windows version info or null if unable to determine.</returns>
    private static WindowsVersion? GetWindowsVersion()
    {
        try
        {
            // Try to get version using RtlGetVersion (most reliable method)
            var version = GetVersionUsingRtlGetVersion();
            if (version != null)
            {
                return version;
            }

            // Fallback to Environment.OSVersion (deprecated but may still work)
            var osVersion = Environment.OSVersion;
            if (osVersion.Platform == PlatformID.Win32NT)
            {
                return new WindowsVersion
                {
                    Major = osVersion.Version.Major,
                    Minor = osVersion.Version.Minor,
                    Build = osVersion.Version.Build
                };
            }
        }
        catch
        {
            // If all methods fail, return null
        }

        return null;
    }

    /// <summary>
    /// Gets Windows version using RtlGetVersion (more reliable than Environment.OSVersion).
    /// </summary>
    private static WindowsVersion? GetVersionUsingRtlGetVersion()
    {
        try
        {
            OSVERSIONINFOEX osvi = new OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = Marshal.SizeOf<OSVERSIONINFOEX>()
            };

            if (RtlGetVersion(ref osvi) == 0)
            {
                return new WindowsVersion
                {
                    Major = (int)osvi.dwMajorVersion,
                    Minor = (int)osvi.dwMinorVersion,
                    Build = (int)osvi.dwBuildNumber
                };
            }
        }
        catch
        {
            // P/Invoke failed, return null to try fallback
        }

        return null;
    }

    /// <summary>
    /// Checks if the Windows version is supported (Windows 10 or 11).
    /// Explicitly rejects Windows 7, 8, and 8.1.
    /// </summary>
    private static bool IsSupportedWindowsVersion(WindowsVersion version)
    {
        // Explicitly reject Windows 7 (6.1), Windows 8 (6.2), and Windows 8.1 (6.3)
        if (version.Major == 6)
        {
            return false; // Windows 7, 8, or 8.1 - not supported
        }

        // Windows 10: Major = 10, Minor = 0, Build >= 10240
        if (version.Major == 10 && version.Minor == 0)
        {
            // Check if it's Windows 11 (build >= 22000) or Windows 10 (build >= 10240)
            if (version.Build >= WINDOWS_11_MIN_BUILD)
            {
                return true; // Windows 11
            }
            return version.Build >= WINDOWS_10_MIN_BUILD; // Windows 10
        }

        // Any other version is not supported
        return false;
    }

    /// <summary>
    /// Gets a friendly name for the Windows version.
    /// </summary>
    private static string GetWindowsVersionName(WindowsVersion version)
    {
        // Windows 11: Major = 10, Build >= 22000
        if (version.Major == 10 && version.Build >= WINDOWS_11_MIN_BUILD)
        {
            return $"Windows 11 (Build {version.Build})";
        }

        // Windows 10: Major = 10, Minor = 0, Build >= 10240
        if (version.Major == 10 && version.Minor == 0 && version.Build >= WINDOWS_10_MIN_BUILD)
        {
            return $"Windows 10 (Build {version.Build})";
        }

        // Windows 8.1: Major = 6, Minor = 3
        if (version.Major == 6 && version.Minor == 3)
        {
            return $"Windows 8.1 (Build {version.Build})";
        }

        // Windows 8: Major = 6, Minor = 2
        if (version.Major == 6 && version.Minor == 2)
        {
            return $"Windows 8 (Build {version.Build})";
        }

        // Windows 7: Major = 6, Minor = 1
        if (version.Major == 6 && version.Minor == 1)
        {
            return $"Windows 7 (Build {version.Build})";
        }

        // Unknown or unsupported version
        return $"Windows {version.Major}.{version.Minor} (Build {version.Build})";
    }

    /// <summary>
    /// Shows an error message dialog and exits the application.
    /// </summary>
    private static void ShowErrorAndExit(string title, string message)
    {
        MessageBox.Show(
            message,
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );

        Environment.Exit(1);
    }

    /// <summary>
    /// Represents Windows version information.
    /// </summary>
    private struct WindowsVersion
    {
        public int Major;
        public int Minor;
        public int Build;
    }

    #region P/Invoke Definitions

    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct OSVERSIONINFOEX
    {
        public int dwOSVersionInfoSize;
        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public uint dwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szCSDVersion;
        public ushort wServicePackMajor;
        public ushort wServicePackMinor;
        public ushort wSuiteMask;
        public byte wProductType;
        public byte wReserved;
    }

    #endregion
}


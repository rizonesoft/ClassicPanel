using System.Runtime.InteropServices;

namespace ClassicPanel.Core.Theme;

/// <summary>
/// P/Invoke definitions for Windows theme and accent color detection.
/// </summary>
internal static class WindowsThemeInterop
{
    #region Windows Registry Keys

    /// <summary>
    /// Registry path for Windows theme settings.
    /// </summary>
    private const string ThemeRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    /// <summary>
    /// Registry path for Windows accent color settings.
    /// </summary>
    private const string AccentColorRegistryPath = @"Software\Microsoft\Windows\DWM";

    /// <summary>
    /// Registry value name for apps theme (light/dark).
    /// </summary>
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";

    /// <summary>
    /// Registry value name for system accent color.
    /// </summary>
    private const string AccentColorValue = "AccentColor";

    #endregion

    #region Windows API

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int RegOpenKeyEx(
        nint hKey,
        string subKey,
        uint options,
        int samDesired,
        out nint phkResult);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegQueryValueEx(
        nint hKey,
        string lpValueName,
        nint lpReserved,
        out uint lpType,
        byte[] lpData,
        ref int lpcbData);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegCloseKey(nint hKey);

    private const int HKEY_CURRENT_USER = unchecked((int)0x80000001);
    private const int KEY_READ = 0x20019;
    private const int REG_DWORD = 4;

    #endregion

    /// <summary>
    /// Gets the Windows apps theme preference (light or dark).
    /// </summary>
    /// <returns>True if light theme is preferred, false if dark theme is preferred.</returns>
    public static bool GetWindowsAppsTheme()
    {
        try
        {
            if (RegOpenKeyEx((nint)HKEY_CURRENT_USER, ThemeRegistryPath, 0, KEY_READ, out nint hKey) == 0)
            {
                try
                {
                    uint type;
                    byte[] data = new byte[4];
                    int dataSize = 4;

                    if (RegQueryValueEx(hKey, AppsUseLightThemeValue, 0, out type, data, ref dataSize) == 0 && type == REG_DWORD)
                    {
                        int value = BitConverter.ToInt32(data, 0);
                        return value != 0; // 1 = light, 0 = dark
                    }
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
        }
        catch
        {
            // Fall through to default
        }

        // Default to light theme if unable to read
        return true;
    }

    /// <summary>
    /// Gets the Windows accent color.
    /// </summary>
    /// <returns>The accent color as a Color, or null if unable to read.</returns>
    public static System.Drawing.Color? GetWindowsAccentColor()
    {
        try
        {
            if (RegOpenKeyEx((nint)HKEY_CURRENT_USER, AccentColorRegistryPath, 0, KEY_READ, out nint hKey) == 0)
            {
                try
                {
                    uint type;
                    byte[] data = new byte[4];
                    int dataSize = 4;

                    if (RegQueryValueEx(hKey, AccentColorValue, 0, out type, data, ref dataSize) == 0 && type == REG_DWORD)
                    {
                        // Accent color is stored as ARGB (but in BGR order in registry)
                        uint colorValue = BitConverter.ToUInt32(data, 0);
                        
                        // Extract ARGB components (Windows stores as BGR)
                        byte a = (byte)((colorValue >> 24) & 0xFF);
                        byte r = (byte)((colorValue >> 16) & 0xFF);
                        byte g = (byte)((colorValue >> 8) & 0xFF);
                        byte b = (byte)(colorValue & 0xFF);

                        return System.Drawing.Color.FromArgb(a, r, g, b);
                    }
                }
                finally
                {
                    RegCloseKey(hKey);
                }
            }
        }
        catch
        {
            // Fall through to null
        }

        return null;
    }

    /// <summary>
    /// Gets the effective theme mode based on system preference.
    /// </summary>
    /// <param name="requestedMode">The requested theme mode (Light, Dark, or System).</param>
    /// <returns>The effective theme mode (Light or Dark).</returns>
    public static string GetEffectiveTheme(string requestedMode)
    {
        if (string.Equals(requestedMode, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase))
        {
            return GetWindowsAppsTheme() ? AppConstants.LightTheme : AppConstants.DarkTheme;
        }

        return requestedMode;
    }
}


using System.Runtime.InteropServices;
using ClassicPanel.Core;

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

    #region DWM (Desktop Window Manager) API

    [DllImport("dwmapi.dll", SetLastError = true, PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(
        nint hwnd,
        int dwAttribute,
        ref int pvAttribute,
        int cbAttribute);

    // DWMWA_USE_IMMERSIVE_DARK_MODE = 20 (Windows 11)
    // DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19 (Windows 10)
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;

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
    /// Windows stores the accent color in the registry as ABGR (Alpha, Blue, Green, Red) format.
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
                        // Windows stores accent color as ABGR (Alpha, Blue, Green, Red) in a DWORD
                        // Bits 31-24: Alpha
                        // Bits 23-16: Blue
                        // Bits 15-8:  Green
                        // Bits 7-0:   Red
                        uint colorValue = BitConverter.ToUInt32(data, 0);
                        
                        // Extract ABGR components
                        byte a = (byte)((colorValue >> 24) & 0xFF);  // Alpha from bits 31-24
                        byte b = (byte)((colorValue >> 16) & 0xFF);  // Blue from bits 23-16
                        byte g = (byte)((colorValue >> 8) & 0xFF);   // Green from bits 15-8
                        byte r = (byte)(colorValue & 0xFF);          // Red from bits 7-0

                        // Create Color with ARGB order (System.Drawing.Color expects ARGB)
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

    /// <summary>
    /// Sets the window title bar theme (light or dark) using DWM.
    /// </summary>
    /// <param name="hwnd">Window handle.</param>
    /// <param name="useDarkMode">True to use dark mode, false for light mode.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    internal static bool SetWindowTitleBarTheme(nint hwnd, bool useDarkMode)
    {
        if (hwnd == nint.Zero)
            return false;

        try
        {
            int value = useDarkMode ? 1 : 0;
            int cbAttribute = sizeof(int);

            // Try Windows 11 API first (DWMWA_USE_IMMERSIVE_DARK_MODE = 20)
            int result = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, cbAttribute);
            
            // If that fails, try Windows 10 API (DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19)
            if (result != 0)
            {
                value = useDarkMode ? 1 : 0; // Reset value
                result = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref value, cbAttribute);
            }

            // If still failing, it might be that DWM composition is disabled or not supported
            // Return true if either call succeeded
            return result == 0; // 0 = success (S_OK)
        }
        catch (Exception ex)
        {
            // Log error but don't show to user (theme setting failure is non-critical)
            ErrorLogger.LogWarning($"Failed to set window title bar theme: {ex.Message}", "WindowsThemeInterop.SetWindowTitleBarTheme");
            return false;
        }
    }
}


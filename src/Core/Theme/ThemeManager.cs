namespace ClassicPanel.Core.Theme;

/// <summary>
/// Manages application themes including light, dark, and system mode.
/// Supports Windows accent colors and automatic theme detection.
/// </summary>
public static class ThemeManager
{
    private static string _currentTheme = AppConstants.DefaultTheme;
    private static ThemeData? _currentThemeData;
    private static System.Drawing.Color _accentColor = System.Drawing.Color.FromArgb(0, 120, 215); // Default Windows blue

    /// <summary>
    /// Gets or sets the current theme mode (Light, Dark, or System).
    /// </summary>
    public static string CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                value = AppConstants.DefaultTheme;

            if (!IsValidTheme(value))
                value = AppConstants.DefaultTheme;

            if (_currentTheme != value)
            {
                _currentTheme = value;
                RefreshTheme();
                OnThemeChanged?.Invoke(GetEffectiveTheme());
            }
        }
    }

    /// <summary>
    /// Gets the effective theme (Light or Dark) based on current mode and system preference.
    /// </summary>
    public static string GetEffectiveTheme()
    {
        return WindowsThemeInterop.GetEffectiveTheme(_currentTheme);
    }

    /// <summary>
    /// Gets the current theme data.
    /// </summary>
    public static ThemeData CurrentThemeData
    {
        get
        {
            if (_currentThemeData == null)
            {
                RefreshTheme();
            }
            return _currentThemeData!;
        }
    }

    /// <summary>
    /// Gets or sets the accent color.
    /// </summary>
    public static System.Drawing.Color AccentColor
    {
        get => _accentColor;
        set
        {
            if (_accentColor != value)
            {
                _accentColor = value;
                RefreshTheme();
                OnThemeChanged?.Invoke(GetEffectiveTheme());
            }
        }
    }

    /// <summary>
    /// Event raised when the theme changes.
    /// </summary>
    public static event Action<string>? OnThemeChanged;

    /// <summary>
    /// Initializes the theme manager.
    /// </summary>
    public static void Initialize()
    {
        // Load Windows accent color
        var windowsAccent = WindowsThemeInterop.GetWindowsAccentColor();
        if (windowsAccent.HasValue)
        {
            _accentColor = windowsAccent.Value;
        }

        // Set default theme
        CurrentTheme = AppConstants.DefaultTheme;
    }

    /// <summary>
    /// Refreshes the current theme data based on the current theme mode and accent color.
    /// </summary>
    private static void RefreshTheme()
    {
        var effectiveTheme = GetEffectiveTheme();

        if (string.Equals(effectiveTheme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
        {
            _currentThemeData = ThemeData.CreateLightTheme(_accentColor);
        }
        else
        {
            _currentThemeData = ThemeData.CreateDarkTheme(_accentColor);
        }
    }

    /// <summary>
    /// Checks if a theme name is valid.
    /// </summary>
    /// <param name="themeName">The theme name to check.</param>
    /// <returns>True if the theme is valid; otherwise, false.</returns>
    public static bool IsValidTheme(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
            return false;

        return string.Equals(themeName, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase) ||
               string.Equals(themeName, AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase) ||
               string.Equals(themeName, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Refreshes the accent color from Windows settings.
    /// </summary>
    public static void RefreshAccentColor()
    {
        var windowsAccent = WindowsThemeInterop.GetWindowsAccentColor();
        if (windowsAccent.HasValue)
        {
            AccentColor = windowsAccent.Value;
        }
    }

    /// <summary>
    /// Gets all available theme modes.
    /// </summary>
    /// <returns>An array of available theme names.</returns>
    public static string[] GetAvailableThemes()
    {
        return new[]
        {
            AppConstants.LightTheme,
            AppConstants.DarkTheme,
            AppConstants.SystemTheme
        };
    }
}


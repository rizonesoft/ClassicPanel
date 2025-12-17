namespace ClassicPanel.Core.Theme;

/// <summary>
/// Represents theme data including colors and styling information.
/// </summary>
public class ThemeData
{
    /// <summary>
    /// Gets or sets the theme name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    public System.Drawing.Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the foreground (text) color.
    /// </summary>
    public System.Drawing.Color ForegroundColor { get; set; }

    /// <summary>
    /// Gets or sets the accent color.
    /// </summary>
    public System.Drawing.Color AccentColor { get; set; }

    /// <summary>
    /// Gets or sets the border color.
    /// </summary>
    public System.Drawing.Color BorderColor { get; set; }

    /// <summary>
    /// Gets or sets the hover background color.
    /// </summary>
    public System.Drawing.Color HoverBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the selected background color.
    /// </summary>
    public System.Drawing.Color SelectedBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the disabled foreground color.
    /// </summary>
    public System.Drawing.Color DisabledForegroundColor { get; set; }

    /// <summary>
    /// Gets or sets the control background color.
    /// </summary>
    public System.Drawing.Color ControlBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the control foreground color.
    /// </summary>
    public System.Drawing.Color ControlForegroundColor { get; set; }

    /// <summary>
    /// Creates a light theme data instance.
    /// </summary>
    /// <param name="accentColor">The accent color to use.</param>
    /// <returns>A light theme data instance.</returns>
    public static ThemeData CreateLightTheme(System.Drawing.Color accentColor)
    {
        return new ThemeData
        {
            Name = AppConstants.LightTheme,
            BackgroundColor = System.Drawing.Color.White,
            ForegroundColor = System.Drawing.Color.Black,
            AccentColor = accentColor,
            BorderColor = System.Drawing.Color.FromArgb(200, 200, 200),
            HoverBackgroundColor = System.Drawing.Color.FromArgb(240, 240, 240),
            SelectedBackgroundColor = System.Drawing.Color.FromArgb(230, 230, 230),
            DisabledForegroundColor = System.Drawing.Color.FromArgb(128, 128, 128),
            ControlBackgroundColor = System.Drawing.Color.White,
            ControlForegroundColor = System.Drawing.Color.Black
        };
    }

    /// <summary>
    /// Creates a dark theme data instance.
    /// </summary>
    /// <param name="accentColor">The accent color to use.</param>
    /// <returns>A dark theme data instance.</returns>
    public static ThemeData CreateDarkTheme(System.Drawing.Color accentColor)
    {
        return new ThemeData
        {
            Name = AppConstants.DarkTheme,
            BackgroundColor = System.Drawing.Color.FromArgb(32, 32, 32),
            ForegroundColor = System.Drawing.Color.White,
            AccentColor = accentColor,
            BorderColor = System.Drawing.Color.FromArgb(64, 64, 64),
            HoverBackgroundColor = System.Drawing.Color.FromArgb(48, 48, 48),
            SelectedBackgroundColor = System.Drawing.Color.FromArgb(56, 56, 56),
            DisabledForegroundColor = System.Drawing.Color.FromArgb(128, 128, 128),
            ControlBackgroundColor = System.Drawing.Color.FromArgb(40, 40, 40),
            ControlForegroundColor = System.Drawing.Color.White
        };
    }
}


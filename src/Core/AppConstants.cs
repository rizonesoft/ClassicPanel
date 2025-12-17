namespace ClassicPanel.Core;

/// <summary>
/// Application-wide constants and configuration values.
/// </summary>
public static class AppConstants
{
    #region Application Information

    /// <summary>
    /// Application name.
    /// </summary>
    public const string ApplicationName = "ClassicPanel";

    /// <summary>
    /// Application display name.
    /// </summary>
    public const string ApplicationDisplayName = "ClassicPanel";

    /// <summary>
    /// Company name.
    /// </summary>
    public const string CompanyName = "Rizonesoft";

    /// <summary>
    /// Website URL.
    /// </summary>
    public const string WebsiteUrl = "https://rizonesoft.com";

    #endregion

    #region File System Paths

    /// <summary>
    /// Name of the system folder where extensions are stored.
    /// </summary>
    public const string SystemFolderName = "system";

    /// <summary>
    /// Name of the settings file (for portable mode).
    /// </summary>
    public const string SettingsFileName = "ClassicPanel.json";

    /// <summary>
    /// Registry key path for installed mode settings.
    /// </summary>
    public const string RegistrySettingsPath = @"Software\Rizonesoft\ClassicPanel";

    #endregion

    #region Control Panel File Extensions

    /// <summary>
    /// Control Panel file extension.
    /// </summary>
    public const string CplFileExtension = ".cpl";

    /// <summary>
    /// Control Panel file filter for file dialogs.
    /// </summary>
    public const string CplFileFilter = "Control Panel Files (*.cpl)|*.cpl|All Files (*.*)|*.*";

    #endregion

    #region UI Constants

    /// <summary>
    /// Default window width in pixels.
    /// </summary>
    public const int DefaultWindowWidth = 900;

    /// <summary>
    /// Default window height in pixels.
    /// </summary>
    public const int DefaultWindowHeight = 600;

    /// <summary>
    /// Minimum window width in pixels.
    /// </summary>
    public const int MinimumWindowWidth = 640;

    /// <summary>
    /// Minimum window height in pixels.
    /// </summary>
    public const int MinimumWindowHeight = 480;

    /// <summary>
    /// Default icon size for large icons view (pixels).
    /// </summary>
    public const int DefaultLargeIconSize = 48;

    /// <summary>
    /// Default icon size for small icons view (pixels).
    /// </summary>
    public const int DefaultSmallIconSize = 16;

    /// <summary>
    /// Default sidebar width in pixels.
    /// </summary>
    public const int DefaultSidebarWidth = 200;

    /// <summary>
    /// Minimum sidebar width in pixels.
    /// </summary>
    public const int MinimumSidebarWidth = 150;

    #endregion

    #region Performance Targets

    /// <summary>
    /// Target startup time in milliseconds (1 second).
    /// </summary>
    public const int TargetStartupTimeMs = 1000;

    /// <summary>
    /// Target frame rate for animations (FPS).
    /// </summary>
    public const int TargetFrameRate = 60;

    #endregion

    #region Search & Filter

    /// <summary>
    /// Minimum search query length before performing search.
    /// </summary>
    public const int MinimumSearchLength = 1;

    /// <summary>
    /// Maximum search results to display.
    /// </summary>
    public const int MaximumSearchResults = 100;

    /// <summary>
    /// Search debounce delay in milliseconds.
    /// </summary>
    public const int SearchDebounceDelayMs = 300;

    #endregion

    #region Keyboard Shortcuts

    /// <summary>
    /// Keyboard shortcut for command palette (Ctrl+K).
    /// </summary>
    public const string CommandPaletteShortcut = "Ctrl+K";

    /// <summary>
    /// Keyboard shortcut for search (Ctrl+F).
    /// </summary>
    public const string SearchShortcut = "Ctrl+F";

    /// <summary>
    /// Keyboard shortcut for favorites (Ctrl+D).
    /// </summary>
    public const string FavoritesShortcut = "Ctrl+D";

    #endregion

    #region Localization

    /// <summary>
    /// Default culture code (en-US).
    /// </summary>
    public const string DefaultCulture = "en-US";

    /// <summary>
    /// Resources folder name for localization files.
    /// </summary>
    public const string ResourcesFolderName = "Resources";

    /// <summary>
    /// Localization file extension.
    /// </summary>
    public const string LocalizationFileExtension = ".resx";

    #endregion

    #region Themes

    /// <summary>
    /// Default theme name.
    /// </summary>
    public const string DefaultTheme = "Light";

    /// <summary>
    /// Light theme name.
    /// </summary>
    public const string LightTheme = "Light";

    /// <summary>
    /// Dark theme name.
    /// </summary>
    public const string DarkTheme = "Dark";

    /// <summary>
    /// System theme mode (follows Windows theme preference).
    /// </summary>
    public const string SystemTheme = "System";

    #endregion

    #region Error Messages

    /// <summary>
    /// Generic error message for unexpected errors.
    /// </summary>
    public const string GenericErrorMessage = "An unexpected error occurred. Please try again or contact support if the problem persists.";

    /// <summary>
    /// Error message when a CPL file cannot be loaded.
    /// </summary>
    public const string CplLoadErrorMessage = "Unable to load the Control Panel file. It may be corrupted or incompatible.";

    /// <summary>
    /// Error message when platform requirements are not met.
    /// </summary>
    public const string PlatformErrorMessage = "ClassicPanel requires Windows 10 or Windows 11 (64-bit).";

    #endregion
}


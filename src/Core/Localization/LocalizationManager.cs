using System.Globalization;
using System.Resources;

namespace ClassicPanel.Core.Localization;

/// <summary>
/// Manages localization and internationalization for ClassicPanel.
/// Provides access to localized strings and culture management.
/// </summary>
public static class LocalizationManager
{
    private static CultureInfo _currentCulture = CultureInfo.GetCultureInfo(AppConstants.DefaultCulture);
    private static ResourceManager? _resourceManager; // Will be initialized when resource files are added

    /// <summary>
    /// Gets or sets the current culture.
    /// </summary>
    public static CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _currentCulture = value;
            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;
            OnCultureChanged?.Invoke(value);
        }
    }

    /// <summary>
    /// Gets the default culture.
    /// </summary>
    public static CultureInfo DefaultCulture => CultureInfo.GetCultureInfo(AppConstants.DefaultCulture);

    /// <summary>
    /// Event raised when the culture changes.
    /// </summary>
    public static event Action<CultureInfo>? OnCultureChanged;

    /// <summary>
    /// Initializes the localization manager.
    /// </summary>
    public static void Initialize()
    {
        // Set default culture
        CurrentCulture = DefaultCulture;

        // Initialize resource manager (will be implemented when resource files are created)
        // _resourceManager = new ResourceManager("ClassicPanel.Resources.Strings", typeof(LocalizationManager).Assembly);
    }

    /// <summary>
    /// Gets a localized string by key.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <param name="defaultValue">The default value if the key is not found.</param>
    /// <returns>The localized string, or the default value if not found.</returns>
    public static string GetString(string key, string? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            return defaultValue ?? key;

        // Try to get from resource manager
        if (_resourceManager != null)
        {
            try
            {
                var value = _resourceManager.GetString(key, CurrentCulture);
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
            catch
            {
                // Fall through to default value
            }
        }

        // Return default value or key if no default provided
        return defaultValue ?? key;
    }

    /// <summary>
    /// Gets a formatted localized string with arguments.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <param name="args">Format arguments.</param>
    /// <returns>The formatted localized string.</returns>
    public static string GetString(string key, params object[] args)
    {
        var format = GetString(key);
        try
        {
            return string.Format(CurrentCulture, format, args);
        }
        catch
        {
            return format;
        }
    }

    /// <summary>
    /// Gets all available cultures supported by the application.
    /// </summary>
    /// <returns>An array of supported cultures.</returns>
    public static CultureInfo[] GetAvailableCultures()
    {
        // For now, return default culture
        // This will be expanded when resource files are added
        return new[] { DefaultCulture };
    }

    /// <summary>
    /// Checks if a culture is supported.
    /// </summary>
    /// <param name="culture">The culture to check.</param>
    /// <returns>True if the culture is supported; otherwise, false.</returns>
    public static bool IsCultureSupported(CultureInfo culture)
    {
        if (culture == null)
            return false;

        var available = GetAvailableCultures();
        return available.Any(c => c.Name.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Attempts to set the culture from a culture name.
    /// </summary>
    /// <param name="cultureName">The culture name (e.g., "en-US", "fr-FR").</param>
    /// <returns>True if the culture was set successfully; otherwise, false.</returns>
    public static bool TrySetCulture(string cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
            return false;

        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            if (IsCultureSupported(culture))
            {
                CurrentCulture = culture;
                return true;
            }
        }
        catch
        {
            // Culture not found or invalid
        }

        return false;
    }
}


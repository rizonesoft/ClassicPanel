namespace ClassicPanel.Core;

/// <summary>
/// Represents a category for organizing Control Panel items.
/// Categories are used for filtering and navigation in the UI.
/// </summary>
public enum Category
{
    /// <summary>
    /// All items (no filter).
    /// </summary>
    All = 0,

    /// <summary>
    /// System and maintenance tools.
    /// </summary>
    System = 1,

    /// <summary>
    /// Network and internet settings.
    /// </summary>
    Network = 2,

    /// <summary>
    /// Security and user account settings.
    /// </summary>
    Security = 3,

    /// <summary>
    /// Hardware and device settings.
    /// </summary>
    Hardware = 4,

    /// <summary>
    /// Programs and features.
    /// </summary>
    Programs = 5,

    /// <summary>
    /// Appearance and personalization.
    /// </summary>
    Appearance = 6,

    /// <summary>
    /// Administrative tools.
    /// </summary>
    AdministrativeTools = 7,

    /// <summary>
    /// Mobile and sync settings.
    /// </summary>
    Mobile = 8,

    /// <summary>
    /// Ease of access and accessibility.
    /// </summary>
    EaseOfAccess = 9,

    /// <summary>
    /// Clock, language, and region settings.
    /// </summary>
    ClockAndRegion = 10,

    /// <summary>
    /// Uncategorized items (no specific category assigned).
    /// </summary>
    Uncategorized = 99
}

/// <summary>
/// Provides utilities for working with categories.
/// </summary>
public static class CategoryHelper
{
    /// <summary>
    /// Gets the display name for a category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>The display name for the category.</returns>
    public static string GetDisplayName(Category category)
    {
        return category switch
        {
            Category.All => "All Items",
            Category.System => "System",
            Category.Network => "Network and Internet",
            Category.Security => "Security",
            Category.Hardware => "Hardware and Devices",
            Category.Programs => "Programs",
            Category.Appearance => "Appearance and Personalization",
            Category.AdministrativeTools => "Administrative Tools",
            Category.Mobile => "Mobile and Sync",
            Category.EaseOfAccess => "Ease of Access",
            Category.ClockAndRegion => "Clock and Region",
            Category.Uncategorized => "Uncategorized",
            _ => category.ToString()
        };
    }

    /// <summary>
    /// Gets the description for a category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>The description for the category.</returns>
    public static string GetDescription(Category category)
    {
        return category switch
        {
            Category.All => "All Control Panel items",
            Category.System => "System settings, maintenance, and properties",
            Category.Network => "Network connections, sharing, and internet settings",
            Category.Security => "User accounts, security, and parental controls",
            Category.Hardware => "Hardware devices, printers, and device management",
            Category.Programs => "Programs, features, and default programs",
            Category.Appearance => "Display, themes, and personalization",
            Category.AdministrativeTools => "Advanced system administration tools",
            Category.Mobile => "Mobile device sync and offline files",
            Category.EaseOfAccess => "Accessibility and ease of access settings",
            Category.ClockAndRegion => "Date, time, language, and regional settings",
            Category.Uncategorized => "Items without a specific category",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Gets all categories except "All" and "Uncategorized".
    /// </summary>
    /// <returns>An array of standard categories.</returns>
    public static Category[] GetStandardCategories()
    {
        return new[]
        {
            Category.System,
            Category.Network,
            Category.Security,
            Category.Hardware,
            Category.Programs,
            Category.Appearance,
            Category.AdministrativeTools,
            Category.Mobile,
            Category.EaseOfAccess,
            Category.ClockAndRegion
        };
    }

    /// <summary>
    /// Gets all available categories.
    /// </summary>
    /// <returns>An array of all categories.</returns>
    public static Category[] GetAllCategories()
    {
        return Enum.GetValues<Category>();
    }

    /// <summary>
    /// Attempts to parse a category from a string.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="category">When this method returns, contains the parsed category if successful; otherwise, Category.Uncategorized.</param>
    /// <returns>True if parsing was successful; otherwise, false.</returns>
    public static bool TryParse(string value, out Category category)
    {
        if (Enum.TryParse<Category>(value, true, out category))
        {
            return true;
        }

        // Try matching by display name
        foreach (var cat in GetAllCategories())
        {
            if (string.Equals(GetDisplayName(cat), value, StringComparison.OrdinalIgnoreCase))
            {
                category = cat;
                return true;
            }
        }

        category = Category.Uncategorized;
        return false;
    }
}


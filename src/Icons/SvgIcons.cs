namespace ClassicPanel.Icons;

/// <summary>
/// Provides predefined SVG icon data for common UI elements.
/// All icons use base64 SVG data URIs from Lucide icons for accurate rendering.
/// Icons are designed for 24x24px viewport (viewBox="0 0 24 24") and scale automatically.
/// 
/// To add a new icon, simply assign a data URI:
///   SvgIcons.IconName = SvgXmlParser.ParseSvgXml("data:image/svg+xml;base64,...");
/// </summary>
public static class SvgIcons
{
    // Theme icons
    /// <summary>Light mode icon (sun). Replace with data URI.</summary>
    public static SvgIconData LightMode { get; set; } = SvgIconData.FromPath("M12 2v2 M12 20v2 M4.93 4.93l1.41 1.41 M17.66 17.66l1.41 1.41 M2 12h2 M20 12h2 M6.34 17.66l-1.41 1.41 M19.07 4.93l-1.41 1.41 M12 6a6 6 0 1 0 0 12 6 6 0 0 0 0-12Z", 2.0f);

    /// <summary>Dark mode icon (moon). Replace with data URI.</summary>
    public static SvgIconData DarkMode { get; set; } = SvgIconData.FromPath("M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9Z", 2.0f);

    // File operations
    /// <summary>Refresh icon (refresh-ccw). Uses data URI.</summary>
    public static SvgIconData Refresh { get; set; } = SvgXmlParser.ParseSvgXml(
        "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0IiBmaWxsPSJub25lIiBzdHJva2U9ImN1cnJlbnRDb2xvciIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiIGNsYXNzPSJsdWNpZGUgbHVjaWRlLXJlZnJlc2gtY2N3LWljb24gbHVjaWRlLXJlZnJlc2gtY2N3Ij48cGF0aCBkPSJNMjEgMTJhOSA5IDAgMCAwLTktOSA5Ljc1IDkuNzUgMCAwIDAtNi43NCAyLjc0TDMgOCIvPjxwYXRoIGQ9Ik0zIDN2NWg1Ii8+PHBhdGggZD0iTTMgMTJhOSA5IDAgMCAwIDkgOSA5Ljc1IDkuNzUgMCAwIDAgNi43NC0yLjc0TDIxIDE2Ii8+PHBhdGggZD0iTTE2IDE2aDV2NSIvPjwvc3ZnPg=="
    );

    /// <summary>Settings icon. Replace with data URI.</summary>
    public static SvgIconData Settings { get; set; } = SvgIconData.FromPath("M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6z", 2.0f);

    // View modes
    /// <summary>Large icons view (grid-3x3). Replace with data URI.</summary>
    public static SvgIconData LargeIcons { get; set; } = SvgIconData.FromPath("M3 3h8v8H3z M13 3h8v8h-8z M3 13h8v8H3z M13 13h8v8h-8z", 2.0f);

    /// <summary>Small icons view (grid). Replace with data URI.</summary>
    public static SvgIconData SmallIcons { get; set; } = SvgIconData.FromPath("M3 3h4v4H3z M10 3h4v4h-4z M17 3h4v4h-4z M3 10h4v4H3z M10 10h4v4h-4z M17 10h4v4h-4z M3 17h4v4H3z M10 17h4v4h-4z M17 17h4v4h-4z", 2.0f);

    /// <summary>List view. Replace with data URI.</summary>
    public static SvgIconData List { get; set; } = SvgIconData.FromPath("M8 6h13 M8 12h13 M8 18h13 M3 6h.01 M3 12h.01 M3 18h.01", 2.0f);

    /// <summary>Details view (list-ordered). Replace with data URI.</summary>
    public static SvgIconData Details { get; set; } = SvgIconData.FromPath("M10 6h11 M10 12h11 M10 18h11 M4 6h1v4h-1z M4 10h2 M6 18H4c0-1 2-2 2-3s-1-1.5-2-1", 2.0f);

    // Common actions
    /// <summary>Search icon. Replace with data URI.</summary>
    public static SvgIconData Search { get; set; } = SvgIconData.FromPath("M21 21l-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z", 2.0f);

    /// <summary>Close icon (X). Replace with data URI.</summary>
    public static SvgIconData Close { get; set; } = SvgIconData.FromPath("M18 6L6 18 M6 6l12 12", 2.0f);

    /// <summary>Menu icon (hamburger). Replace with data URI.</summary>
    public static SvgIconData Menu { get; set; } = SvgIconData.FromPath("M4 12h16 M4 6h16 M4 18h16", 2.0f);
}


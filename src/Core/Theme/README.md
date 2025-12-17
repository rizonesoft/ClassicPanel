# Theme System Architecture

This folder contains the theme system infrastructure for ClassicPanel.

## Overview

The theme system supports three modes:
- **Light**: Light theme with light backgrounds
- **Dark**: Dark theme with dark backgrounds
- **System**: Follows Windows theme preference (automatically switches between light and dark)

The system automatically reads Windows accent colors and applies them to the application.

## Components

### ThemeManager

Main theme management class that:
- Manages current theme mode (Light/Dark/System)
- Loads Windows accent colors automatically
- Provides theme data for UI components
- Raises events when theme changes

**Usage:**
```csharp
// Initialize theme system
ThemeManager.Initialize();

// Set theme mode
ThemeManager.CurrentTheme = AppConstants.LightTheme;
ThemeManager.CurrentTheme = AppConstants.DarkTheme;
ThemeManager.CurrentTheme = AppConstants.SystemTheme; // Follows Windows

// Get current theme data
var theme = ThemeManager.CurrentThemeData;
var backgroundColor = theme.BackgroundColor;
var accentColor = theme.AccentColor;

// Refresh accent color from Windows
ThemeManager.RefreshAccentColor();

// Listen for theme changes
ThemeManager.OnThemeChanged += (effectiveTheme) => {
    // Update UI when theme changes
};
```

### ThemeData

Represents theme color data including:
- Background and foreground colors
- Accent color (from Windows)
- Border, hover, and selected colors
- Control colors

**Usage:**
```csharp
// Create theme instances
var lightTheme = ThemeData.CreateLightTheme(accentColor);
var darkTheme = ThemeData.CreateDarkTheme(accentColor);
```

### WindowsThemeInterop

Low-level Windows API interop for:
- Reading Windows apps theme preference (light/dark)
- Reading Windows accent color from registry
- Converting theme mode to effective theme

**Note**: This class is internal and used by ThemeManager.

## Windows Integration

The theme system reads from Windows registry:
- **Theme Preference**: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme`
  - `1` = Light theme preferred
  - `0` = Dark theme preferred
- **Accent Color**: `HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM\AccentColor`
  - Stored as ARGB DWORD value

## Future Enhancements

- Theme persistence (save user preference)
- Smooth theme transitions
- Custom theme support (if needed)
- High contrast mode support (accessibility)


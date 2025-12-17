# API Reference

## Core Classes

### VersionInfo

Provides version information for ClassicPanel.

**Properties:**
- `Version` (string) - Full version string (e.g., "0.1.0")
- `VersionObject` (Version) - Version as System.Version object
- `AssemblyVersion` (string) - Assembly version
- `FileVersion` (string) - File version
- `ProductName` (string) - Product name
- `Company` (string) - Company name
- `Copyright` (string) - Copyright information
- `Description` (string) - Product description
- `BuildDate` (DateTime) - Build date

**Methods:**
- `GetDisplayVersion()` - Gets formatted version string for display
- `GetDetailedVersionInfo()` - Gets detailed version information for about dialogs

### AppConstants

Application-wide constants and configuration values.

**Categories:**
- Application Information (name, company, website)
- File System Paths (system folder, settings file, registry paths)
- Control Panel Extensions (.cpl file extension, filters)
- UI Constants (window sizes, icon sizes, sidebar dimensions)
- Performance Targets (startup time, frame rate)
- Search & Filter (minimum search length, debounce delay)
- Keyboard Shortcuts (command palette, search, favorites)
- Localization (default culture, resource folder)
- Themes (default, light, dark, high contrast)
- Error Messages (generic, CPL load, platform errors)

### Category

Enumeration of categories for organizing Control Panel items.

**Values:**
- `All` - All items (no filter)
- `System` - System and maintenance tools
- `Network` - Network and internet settings
- `Security` - Security and user account settings
- `Hardware` - Hardware and device settings
- `Programs` - Programs and features
- `Appearance` - Appearance and personalization
- `AdministrativeTools` - Administrative tools
- `Mobile` - Mobile and sync settings
- `EaseOfAccess` - Ease of access and accessibility
- `ClockAndRegion` - Clock, language, and region settings
- `Uncategorized` - Items without a specific category

### CategoryHelper

Utilities for working with categories.

**Methods:**
- `GetDisplayName(Category)` - Gets display name for a category
- `GetDescription(Category)` - Gets description for a category
- `GetStandardCategories()` - Gets all categories except "All" and "Uncategorized"
- `GetAllCategories()` - Gets all available categories
- `TryParse(string, out Category)` - Attempts to parse a category from a string

### LocalizationManager

Manages localization and internationalization.

**Properties:**
- `CurrentCulture` (CultureInfo) - Gets or sets the current culture
- `DefaultCulture` (CultureInfo) - Gets the default culture

**Methods:**
- `Initialize()` - Initializes the localization manager
- `GetString(string, string?)` - Gets a localized string by key
- `GetString(string, params object[])` - Gets a formatted localized string
- `GetAvailableCultures()` - Gets all available cultures
- `IsCultureSupported(CultureInfo)` - Checks if a culture is supported
- `TrySetCulture(string)` - Attempts to set the culture from a culture name

**Events:**
- `OnCultureChanged` - Raised when the culture changes

### ThemeManager

Manages application themes including light, dark, and system mode. Supports Windows accent colors.

**Properties:**
- `CurrentTheme` (string) - Gets or sets the current theme mode (Light, Dark, or System)
- `CurrentThemeData` (ThemeData) - Gets the current theme data
- `AccentColor` (Color) - Gets or sets the accent color

**Methods:**
- `Initialize()` - Initializes the theme manager and loads Windows accent color
- `GetEffectiveTheme()` - Gets the effective theme (Light or Dark) based on current mode and system preference
- `IsValidTheme(string)` - Checks if a theme name is valid
- `RefreshAccentColor()` - Refreshes the accent color from Windows settings
- `GetAvailableThemes()` - Gets all available theme modes

**Events:**
- `OnThemeChanged` - Raised when the theme changes

### ThemeData

Represents theme data including colors and styling information.

**Properties:**
- `Name` (string) - Theme name
- `BackgroundColor` (Color) - Background color
- `ForegroundColor` (Color) - Foreground (text) color
- `AccentColor` (Color) - Accent color
- `BorderColor` (Color) - Border color
- `HoverBackgroundColor` (Color) - Hover background color
- `SelectedBackgroundColor` (Color) - Selected background color
- `DisabledForegroundColor` (Color) - Disabled foreground color
- `ControlBackgroundColor` (Color) - Control background color
- `ControlForegroundColor` (Color) - Control foreground color

**Methods:**
- `CreateLightTheme(Color)` - Creates a light theme data instance
- `CreateDarkTheme(Color)` - Creates a dark theme data instance

# API Reference

## Namespace: ClassicPanel.Core

### CplInterop

Static class containing P/Invoke definitions for Windows Control Panel API.

#### Constants

```csharp
public const uint CPL_INIT = 1;
public const uint CPL_GETCOUNT = 2;
public const uint CPL_INQUIRE = 3;
public const uint CPL_NEWINQUIRE = 0x001A;
public const uint CPL_DBLCLK = 5;
public const uint CPL_EXIT = 6;
public const uint CPL_STOP = 7;
public const uint CPL_EXITCODE = 8;
```

#### Types

**CPlAppletDelegate**
```csharp
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate int CPlAppletDelegate(nint hwndCPl, uint uMsg, nint lParam1, nint lParam2);
```
Function signature for CPlApplet export in .cpl files.

**CPLINFO**
```csharp
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct CPLINFO
{
    public int idIcon;     // Resource ID of the icon
    public int idName;     // Resource ID of the name string
    public int idInfo;     // Resource ID of the description string
    public nint lData;     // User data
}
```
Structure returned by CPL_INQUIRE message (ANSI format).

**NEWCPLINFO**
```csharp
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NEWCPLINFO
{
    public uint dwSize;
    public uint dwFlags;
    public uint dwHelpContext;
    public nint lData;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string szName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szInfo;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szHelpFile;
}
```
Structure returned by CPL_NEWINQUIRE message (Unicode format).

#### Methods

**LoadIcon**
```csharp
[DllImport("user32.dll", SetLastError = true)]
public static extern nint LoadIcon(nint hInstance, nint lpIconName);
```
Loads an icon from a resource.

**LoadImage**
```csharp
[DllImport("user32.dll", SetLastError = true)]
public static extern nint LoadImage(
    nint hinst,
    nint lpszName,
    uint uType,
    int cxDesired,
    int cyDesired,
    uint fuLoad);
```
Loads an image (icon, bitmap, cursor) from a resource.

---

### CplItem

Represents a single Control Panel applet.

#### Properties

```csharp
public string Name { get; set; }
```
Display name of the applet.

```csharp
public string Description { get; set; }
```
Description/tooltip text for the applet.

```csharp
public string Path { get; set; }
```
Full path to the .cpl file containing this applet.

```csharp
public nint Handle { get; set; }
```
Native library handle for the loaded .cpl file.

```csharp
public Icon? Icon { get; set; }
```
Icon for the applet (may be null).

```csharp
public int AppletIndex { get; set; }
```
Zero-based index of this applet within its .cpl file.

---

### CplLoader

Loads and manages Control Panel applets from the system folder.

#### Properties

```csharp
public IReadOnlyList<CplItem> Items { get; }
```
Read-only list of all loaded applet items.

#### Methods

**LoadSystemFolder**
```csharp
public void LoadSystemFolder()
```
Scans the `system/` folder next to the executable and loads all .cpl files found. Creates the folder if it doesn't exist.

**Exceptions:**
- `DirectoryNotFoundException`: If the system folder cannot be created.
- Individual file loading errors are caught and logged, but don't stop the overall process.

---

### PlatformValidator

Validates that the runtime platform meets ClassicPanel's requirements.

#### Methods

**ValidatePlatform**
```csharp
public static bool ValidatePlatform()
```
Validates that the current platform meets ClassicPanel requirements:
- Operating system is Windows
- Architecture is x64 (64-bit)
- Windows version is 10 (build 10240+) or 11 (build 22000+)

**Returns**: `true` if platform is valid

**Behavior**: 
- If platform is invalid, shows a user-friendly error message dialog and exits the application
- Should be called at application startup before any other initialization

**Example**:
```csharp
static void Main()
{
    PlatformValidator.ValidatePlatform(); // Exits if requirements not met
    ApplicationConfiguration.Initialize();
    Application.Run(new MainWindow());
}
```

---

## Namespace: ClassicPanel.UI

### MainWindow

Main application window (Windows Forms).

#### Properties

(Properties defined in Designer file)

#### Methods

(To be documented as implementation progresses)

#### Events

**ListView.DoubleClick**
Fired when user double-clicks an applet in the ListView. Should execute the selected applet.

---

## Future API Additions

- Settings management API
- Version information API
- Error logging API
- Icon extraction utilities
- String resource extraction utilities

## References

- [Windows Control Panel API](https://learn.microsoft.com/windows/win32/shell/control-panel-applications)
- [P/Invoke Documentation](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke)


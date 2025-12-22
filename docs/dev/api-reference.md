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

### PerformanceMonitor

Manages application performance monitoring including startup time, memory usage, and operation timings.

**Properties:**
- `IsEnabled` (bool) - Gets or sets whether performance monitoring is enabled
- `StartupTimeMs` (long) - Gets the application startup time in milliseconds
- `CurrentMemoryBytes` (long) - Gets the current memory usage in bytes
- `PeakMemoryBytes` (long) - Gets the peak memory usage in bytes
- `OperationCount` (int) - Gets the number of operations tracked
- `AverageOperationTimeMs` (double) - Gets the average operation time in milliseconds
- `SlowestOperationTimeMs` (long) - Gets the slowest operation time in milliseconds

**Methods:**
- `Initialize()` - Initializes the performance monitor (call at startup)
- `MarkStartupComplete()` - Marks application as fully started and records startup time
- `RecordOperation(string, long)` - Records an operation time
- `UpdateMemoryUsage()` - Updates memory usage tracking
- `GetMetrics()` - Gets current performance metrics
- `GetMetricsSummary()` - Gets a formatted string with performance metrics
- `Reset()` - Resets all performance metrics

**Events:**
- `OnSlowOperation` - Raised when a slow operation is detected (>100ms)

### OperationTimer

Provides timing functionality for measuring operation performance. Implements IDisposable for convenient using statement usage.

**Properties:**
- `ElapsedMilliseconds` (long) - Gets the elapsed time in milliseconds
- `Elapsed` (TimeSpan) - Gets the elapsed time as a TimeSpan

**Methods:**
- `Start(string)` - Creates a timer that automatically records to PerformanceMonitor
- `Stop()` - Stops the timer and records the operation
- `Dispose()` - Disposes the timer, stopping it if not already stopped

**Usage:**
```csharp
using (OperationTimer.Start("LoadApplets"))
{
    // Operation code
}
```

### PerformanceMetrics

Represents performance metrics collected by the performance monitoring system.

**Properties:**
- `StartupTimeMs` (long) - Application startup time in milliseconds
- `MemoryUsageBytes` (long) / `MemoryUsageMB` (double) - Current memory usage
- `PeakMemoryUsageBytes` (long) / `PeakMemoryUsageMB` (double) - Peak memory usage
- `CpuUsagePercent` (double) - CPU usage percentage (0-100, placeholder)
- `OperationCount` (int) - Number of operations tracked
- `AverageOperationTimeMs` (double) - Average operation time
- `SlowestOperationTimeMs` (long) - Slowest operation time
- `Timestamp` (DateTime) - When metrics were collected

**Methods:**
- `CreateSnapshot()` - Creates a snapshot of current performance metrics

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

### DebugLogCapture

Static class that captures `Debug.WriteLine` output and provides access to log entries for real-time viewing in debug tools.

**Properties:**
- `MaxEntries` (int) - Gets or sets the maximum number of log entries to keep in memory (default: 10000, minimum: 100)
- `LogEntries` (IReadOnlyList<LogEntry>) - Gets all captured log entries
- `EntryCount` (int) - Gets the number of log entries currently captured
- `IsCapturing` (bool) - Gets a value indicating whether log capture is currently active

**Methods:**
- `StartCapture()` - Starts capturing debug output
- `StopCapture()` - Stops capturing debug output
- `AddLogEntry(string, LogLevel)` - Adds a log entry manually (for testing or custom logging)
- `Clear()` - Clears all captured log entries
- `GetEntriesByLevel(LogLevel)` - Gets log entries filtered by level
- `GetEntriesByTimeRange(DateTime, DateTime)` - Gets log entries within a time range

**Events:**
- `OnLogEntry` - Raised when a new log entry is captured

**LogEntry Structure:**
```csharp
public struct LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public LogLevel Level { get; set; }
    public string FormattedMessage { get; }
}
```

**LogLevel Enum:**
```csharp
public enum LogLevel
{
    Info,
    Warning,
    Error,
    Critical
}
```

### ErrorInfo

Encapsulates error details for centralized error handling.

**Properties:**
- `Message` (string) - User-friendly error message
- `DetailedMessage` (string?) - Detailed technical error message
- `Exception` (Exception?) - The exception that caused the error
- `Severity` (ErrorSeverity) - Error severity level
- `Context` (string?) - Additional context about where the error occurred
- `AdditionalData` (Dictionary<string, object>?) - Additional error data
- `Timestamp` (DateTime) - When the error occurred
- `ShouldShowToUser` (bool) - Whether this error should be shown to the user

**ErrorSeverity Enum:**
```csharp
public enum ErrorSeverity
{
    Information,
    Warning,
    Error,
    Critical
}
```

### ErrorLogger

Static class providing centralized error logging with debug output and user notifications.

**Methods:**
- `LogError(ErrorInfo)` - Logs an error to debug output
- `LogError(string, Exception?, string?)` - Logs an error with message, optional exception, and context
- `LogWarning(string, string?)` - Logs a warning to debug output
- `LogInfo(string, string?)` - Logs informational message to debug output
- `ShowError(ErrorInfo)` - Shows a user-friendly error message via MessageBox
- `ShowError(string, string?, Exception?)` - Shows a user-friendly error message with optional details

### ErrorRecovery

Static class providing retry logic and fallback strategies for transient operations.

**Methods:**
- `Retry<T>(Func<T>, int, int, Func<Exception, bool>?, string?)` - Retries an operation with exponential backoff
- `Retry(Action, int, int, Func<Exception, bool>?, string?)` - Retries an action with exponential backoff
- `RetryWithFallback<T>(Func<T>, Func<T>, int, int, Func<Exception, bool>?, string?)` - Retries with fallback if all retries fail
- `WithFallback<T>(Func<T>, Func<T>, string?)` - Executes operation with fallback on error
- `WithFallbackValue<T>(Func<T>, T, string?)` - Executes operation with fallback value on error
- `TryRetry(Action, out Exception?, int, int, Func<Exception, bool>?, string?)` - Attempts retry, returns success status
- `IsTransientFileError(Exception)` - Checks if an exception is a transient file I/O error

## Namespace: ClassicPanel.UI

### DebugToolsWindow

WinForms window providing developer debug tools with console output, log viewer, and performance metrics.

**Features:**
- **Console Tab**: Real-time debug output with color-coded log levels (Info, Warning, Error, Critical)
- **Log Viewer Tab**: Filterable log entries with level filtering and export functionality
- **Metrics Tab**: Performance metrics, log statistics, and system information
- **Theme Support**: Automatically adapts to application theme (light/dark/system)
- **Auto-scroll**: Optional auto-scroll for console output
- **Real-time Updates**: Auto-refreshes console and metrics on timer interval

**Access:**
- Click the bug icon button in the status bar (far right)
- Window can be hidden/shown without disposing

**Usage:**
The debug tools window is automatically initialized when opened. It subscribes to `DebugLogCapture.OnLogEntry` for real-time log updates and `ThemeManager.OnThemeChanged` for theme updates.

## Future API Additions

- Settings management API
- Icon extraction utilities
- String resource extraction utilities

## References

- [Windows Control Panel API](https://learn.microsoft.com/windows/win32/shell/control-panel-applications)
- [P/Invoke Documentation](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke)


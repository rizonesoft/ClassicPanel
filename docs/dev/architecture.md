# Architecture Documentation

## Overview

ClassicPanel is a Windows Forms application built with .NET 10 and C# 14. It replicates the classic Windows Control Panel interface by dynamically loading and executing Control Panel applets (.cpl files).

## Technology Stack

### Primary Stack (Default)
- **.NET 10** (LTS) - Runtime and base libraries
- **C# 14** - Programming language
- **Windows Forms** - UI framework (default)
- **ReadyToRun** - Pre-compiled code for faster startup with full .NET compatibility
- **P/Invoke** - Interop with Windows Control Panel API

### Supported Alternative Frameworks
- **WPF** - Windows Presentation Foundation (alternative UI framework)
- **C++** - Native C++ extensions and components (via C++/CLI or native DLLs)
- **C++/WinRT** - Modern C++ Windows Runtime API support
- **Other .NET UI Frameworks** - Extensible architecture supports additional frameworks

### Multi-Framework Architecture
ClassicPanel is designed with a **plugin-based, multi-framework architecture** that allows:
- **UI Framework Abstraction**: Core logic separated from UI implementation
- **Language Interop**: Support for C++, C#/WinRT, and other languages
- **Extension System**: CPL extensions can be written in C#, C++, or other supported languages
- **Framework Plugins**: Load different UI frameworks (WinForms, WPF) as plugins

## Architecture Layers

### 1. UI Abstraction Layer (`ClassicPanel.UI.Abstractions`)

**Responsibilities:**
- Define UI framework-agnostic interfaces
- Abstract window, controls, and UI components
- Provide contracts for UI implementations

**Components:**
- `IWindow`: Abstract window interface
- `IListView`: Abstract list view interface
- `IMenuBar`: Abstract menu interface
- `ICommandBar`: Abstract toolbar interface
- `IUIProvider`: Factory for creating UI components

### 2. UI Implementation Layer (`ClassicPanel.UI.*`)

**WinForms Implementation** (`ClassicPanel.UI.WinForms`):
- Default UI implementation using Windows Forms
- Implements all UI abstraction interfaces
- ReadyToRun compatible (full .NET compatibility, no AOT restrictions)

**WPF Implementation** (`ClassicPanel.UI.WPF`) - Optional:
- Alternative UI implementation using WPF
- Implements all UI abstraction interfaces
- Can be loaded as a plugin/extension
- Provides modern XAML-based UI

**Other Implementations**:
- Additional UI frameworks can be added as plugins
- Each implements the UI abstraction interfaces

### 3. Core Layer (`ClassicPanel.Core`)

**Responsibilities:**
- CPL file loading and management
- Windows API interop
- Resource extraction (icons, strings)
- Applet execution

**Components:**
- `CplInterop`: P/Invoke definitions for Windows Control Panel API
- `CplLoader`: Scans and loads .cpl files from system folder
- `CplItem`: Data model representing a single applet
- `PlatformValidator`: Validates platform requirements (Windows 10/11, 64-bit)
- `VersionInfo`: Provides version information from assembly attributes
- `AppConstants`: Application-wide constants and configuration values
- `Category` / `CategoryHelper`: Category enumeration and management for organizing Control Panel items
- `LocalizationManager`: Manages localization and internationalization
- `ThemeManager`: Manages application themes (light/dark/system mode) with Windows accent colors
- `ThemeData`: Represents theme color data and styling information
- `WindowsThemeInterop`: P/Invoke definitions for reading Windows theme and accent color settings
- `PerformanceMonitor`: Monitors application performance (startup time, memory, operation timings)
- `PerformanceMetrics`: Data structure for performance metrics snapshots
- `OperationTimer`: Timer for measuring operation performance
- `ExtensionManager`: Manages CPL extensions and plugins (future)
- `FrameworkLoader`: Loads and manages UI framework implementations (future)

### 4. Interop Layer (`ClassicPanel.Interop`)

**Responsibilities:**
- Language interop (C#, C++, etc.)
- Framework interop (WinForms, WPF, native)
- Extension loading and management

**Components:**
- `NativeLibraryLoader`: Loads native C++ DLLs
- `CppCliBridge`: Bridge for C++/CLI components
- `WinRTInterop`: Windows Runtime interop for C++/WinRT
- `ExtensionHost`: Hosts extensions in different languages

## Component Details

### CplInterop

Defines all Windows API interop needed for Control Panel applets:

- CPL message constants (CPL_INIT, CPL_GETCOUNT, CPL_INQUIRE, etc.)
- `CPlAppletDelegate`: Function pointer for CPlApplet export
- Structures: `CPLINFO`, `NEWCPLINFO`
- Windows API functions: `LoadIcon`, `LoadImage`

### CplLoader

Manages the lifecycle of Control Panel applets:

1. **Discovery**: Scans `system/` folder for `.cpl` files
2. **Loading**: Uses `NativeLibrary.Load` to load DLLs dynamically
3. **Enumeration**: Calls `CPlApplet` with `CPL_GETCOUNT` to get applet count
4. **Information Extraction**: Calls `CPL_INQUIRE` or `CPL_NEWINQUIRE` for each applet
5. **Resource Extraction**: Loads icons and strings from resources
6. **Caching**: Maintains loaded handles and resources

### CplItem

Data model for a single Control Panel applet:

- `Name`: Display name
- `Description`: Tooltip/description text
- `Path`: Full path to .cpl file
- `Handle`: Native library handle (nint)
- `Icon`: System.Drawing.Icon object
- `AppletIndex`: Zero-based index within the .cpl file

### MainWindow

Main application window:

- **ListView**: Displays applets with icons and names
- **View Modes**: Large Icons, Small Icons, List, Details
- **Menu System**: File (Refresh, Exit), View (view modes), Help (About)
- **Toolbar**: Quick access to view modes
- **Context Menu**: Right-click options

## Data Flow

### Application Startup

```
Program.Main()
  └─> Application.Run(new MainWindow())
      └─> MainWindow.Load event
          └─> CplLoader.LoadSystemFolder()
              └─> For each .cpl file:
                  ├─> NativeLibrary.Load()
                  ├─> Get CPlApplet export
                  ├─> Call CPL_INIT
                  ├─> Call CPL_GETCOUNT
                  └─> For each applet:
                      ├─> Call CPL_INQUIRE/CPL_NEWINQUIRE
                      ├─> Extract icon from resources
                      ├─> Extract name and description
                      └─> Create CplItem
          └─> Populate ListView with CplItem objects
```

### Applet Execution

```
User double-clicks ListView item
  └─> MainWindow.ListView_DoubleClick()
      └─> Get selected CplItem
          └─> Execute via rundll32.exe:
              rundll32.exe shell32.dll,Control_RunDLL <path>,<index>
              └─> Windows launches Control Panel applet
```

## File System Structure

### Application Directory

```
ClassicPanel.exe
system/
  ├── appwiz.cpl      # Add/Remove Programs
  ├── desk.cpl        # Display Properties
  ├── main.cpl        # Mouse Properties
  └── ...             # Other .cpl files
```

The `system/` folder is created automatically if it doesn't exist. Users can place `.cpl` files here for the application to discover.

## Control Panel Applet API

Control Panel applets are DLLs that export a function called `CPlApplet` with this signature:

```csharp
int CPlApplet(nint hwndCPl, uint uMsg, nint lParam1, nint lParam2);
```

### Message Flow

1. **CPL_INIT**: Initialize the applet, return 0 on success
2. **CPL_GETCOUNT**: Return number of applets in this DLL
3. **CPL_INQUIRE** or **CPL_NEWINQUIRE**: Get information about a specific applet (by index)
4. **CPL_DBLCLK**: User double-clicked (execution)
5. **CPL_EXIT**: Cleanup before unload
6. **CPL_STOP**: Stop message (rare)

## Resource Extraction

### Embedded SVG Resources

SVG icons are embedded directly into the main assembly for distribution. The resource naming format is:

**Format**: `ClassicPanel.Resources.Actions.{filename}.svg`

**Example**: `ClassicPanel.Resources.Actions.refresh.svg`

**Configuration** (in `ClassicPanel.csproj`):
```xml
<EmbeddedResource Include="..\resources\actions\*.svg">
  <LogicalName>ClassicPanel.Resources.Actions.%(Filename)%(Extension)</LogicalName>
</EmbeddedResource>
```

**Loading** (via `SvgFileLoader`):
- Resources are loaded using `Assembly.GetEntryAssembly().GetManifestResourceStream()`
- The `SvgFileLoader.GetEmbeddedResourceName()` method constructs the full resource name: `ClassicPanel.Resources.Actions.{filename}.svg`
- Resources are rendered using SkiaSharp for accurate SVG rendering with theme support
- No runtime format detection - the format is hardcoded for consistency and performance

### Control Panel Applet Icons

Icons are extracted from the .cpl file's resources using the `idIcon` from `CPL_INQUIRE`:

1. Call `LoadImage` or `LoadIcon` with `hModule` and `idIcon`
2. Convert the handle to `System.Drawing.Icon`
3. Cache the icon for ListView display

### Strings

Strings are extracted from the .cpl file's string resources:

1. Use `idName` and `idInfo` from `CPL_INQUIRE`
2. Call `LoadString` or extract from resources
3. Handle both ANSI and Unicode encodings

## Error Handling

- **File Loading Errors**: Caught and logged, skipped files
- **P/Invoke Errors**: Check `Marshal.GetLastWin32Error()` after calls
- **Resource Extraction Errors**: Fallback to defaults
- **Execution Errors**: Display user-friendly message

## Configuration

Settings are stored in a JSON file (location TBD):

- View mode preference
- Icon size preference
- Window position/size
- Last used folder paths

## See Also

- [Embedded Resources](embedded-resources.md) - Detailed guide on embedded SVG resource naming and loading

## Platform Requirements

ClassicPanel has strict platform requirements enforced at both build-time and runtime:

### Operating System
- **Windows 10** (build 10240 or later) - Minimum supported version
- **Windows 11** (build 22000 or later) - Fully supported
- **Unsupported**: Windows 7, Windows 8, Windows 8.1

### Architecture
- **64-bit (x64) only** - No 32-bit support
- RuntimeIdentifier: `win-x64`

### Platform Validation

The `PlatformValidator` class performs comprehensive runtime validation on application startup:
1. Checks that the OS is Windows
2. Verifies architecture is x64 (64-bit only)
3. Validates Windows version is 10 or 11
   - **Windows 10**: Build 10240 or later required
   - **Windows 11**: Build 22000 or later supported
   - **Explicitly rejects**: Windows 7, Windows 8, Windows 8.1
4. Shows user-friendly, version-specific error messages if requirements aren't met
5. Exits gracefully if platform is unsupported (before any UI initialization)

Platform validation runs in `Program.Main()` before any other initialization, ensuring unsupported platforms are caught immediately.

**Error Messages:**
- Detects and names the specific Windows version (Windows 7, 8, 8.1, 10, 11)
- Provides clear upgrade instructions
- Shows detected build number for troubleshooting

### Build-Time Enforcement

The project file enforces platform requirements:
- `WindowsTargetPlatformVersion`: Set to Windows 10 SDK version
- `SupportedOSPlatform`: Set to `windows10.0.10240`
- `RuntimeIdentifier`: Set to `win-x64`

## Build Considerations

- **ReadyToRun Enabled**: Code is pre-compiled to native format for faster startup
- **Dynamic Loading**: Use `NativeLibrary` instead of `[DllImport]` for dynamic loading (already implemented)
- **Full Reflection Support**: All reflection features work normally (ReadyToRun compatible)
- **Full .NET Compatibility**: ReadyToRun maintains full compatibility - no restrictions on language features
- **Testing**: Test Release builds with ReadyToRun to verify functionality and startup performance

## Future Enhancements

- Search/filter functionality
- Favorites/bookmarks
- Custom categories
- Plugin system
- Multi-language support

## References

- [Windows Control Panel Applications](https://learn.microsoft.com/windows/win32/shell/control-panel-applications)
- [P/Invoke Documentation](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke)
- [.NET Deployment](https://learn.microsoft.com/dotnet/core/deploying/)


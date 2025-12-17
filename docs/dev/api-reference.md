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


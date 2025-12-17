# Multi-Framework Architecture

## Overview

ClassicPanel is designed with a **plugin-based, multi-framework architecture** that supports multiple UI frameworks and programming languages. This allows developers to:

- Use **WinForms** (default) or **WPF** for UI
- Write extensions in **C#**, **C++**, or other supported languages
- Load UI frameworks as plugins
- Mix and match frameworks based on requirements

## Architecture Principles

### 1. UI Abstraction Layer

The core application logic is **completely independent** of the UI framework. All UI operations go through abstraction interfaces:

```
Core Logic → UI Abstractions → UI Implementation (WinForms/WPF/etc.)
```

### 2. Plugin-Based Framework Loading

UI frameworks are loaded as plugins, allowing:
- Runtime framework selection
- Multiple framework support in same application
- Easy addition of new frameworks

### 3. Language Interop

Support for multiple programming languages:
- **C#** - Primary language (.NET 10)
- **C++** - Via C++/CLI or native DLLs
- **C++/WinRT** - Modern C++ Windows Runtime support
- **Future**: Other languages as needed

## UI Abstraction Layer

### Core Interfaces

Located in `ClassicPanel.UI.Abstractions`:

```csharp
// Window abstraction
public interface IWindow
{
    string Title { get; set; }
    int Width { get; set; }
    int Height { get; set; }
    void Show();
    void Hide();
    void Close();
    event EventHandler Closed;
}

// ListView abstraction
public interface IListView
{
    ViewMode ViewMode { get; set; }
    void AddItem(CplItem item);
    void ClearItems();
    CplItem? SelectedItem { get; }
    event EventHandler<ItemClickEventArgs> ItemDoubleClick;
}

// Menu abstraction
public interface IMenuBar
{
    void AddMenu(string name, IEnumerable<MenuItem> items);
    void Clear();
}

// UI Provider factory
public interface IUIProvider
{
    IWindow CreateWindow();
    IListView CreateListView();
    IMenuBar CreateMenuBar();
    ICommandBar CreateCommandBar();
    // ... other UI components
}
```

## Framework Implementations

### WinForms Implementation (Default)

**Project**: `ClassicPanel.UI.WinForms`

- Implements all UI abstraction interfaces
- Uses Windows Forms controls
- ReadyToRun compatible (full .NET compatibility, no AOT restrictions)
- Default and recommended for most use cases

**Example**:
```csharp
public class WinFormsWindow : IWindow
{
    private Form _form;
    
    public WinFormsWindow()
    {
        _form = new Form();
    }
    
    public string Title
    {
        get => _form.Text;
        set => _form.Text = value;
    }
    
    // ... implement other IWindow members
}
```

### WPF Implementation (Optional)

**Project**: `ClassicPanel.UI.WPF`

- Implements all UI abstraction interfaces
- Uses WPF/XAML for UI
- Can be loaded as a plugin
- Provides modern, XAML-based interface

**Example**:
```csharp
public class WpfWindow : IWindow
{
    private Window _window;
    
    public WpfWindow()
    {
        _window = new Window();
    }
    
    public string Title
    {
        get => _window.Title;
        set => _window.Title = value;
    }
    
    // ... implement other IWindow members
}
```

## C++ Support

### C++/CLI Extensions

Extensions can be written in C++/CLI and loaded as .NET assemblies:

```cpp
// SystemProperties.cpp (C++/CLI)
public ref class SystemPropertiesApplet : public IApplet
{
public:
    void Execute() override
    {
        // C++ implementation
    }
};
```

### Native C++ DLLs

Native C++ components can be loaded via P/Invoke or native library loading:

```csharp
// C# side
[DllImport("NativeExtension.dll")]
public static extern int ExecuteApplet(int appletId);
```

### C++/WinRT

For modern C++ Windows Runtime support:

```cpp
// C++/WinRT
using namespace winrt;
using namespace Windows::Foundation;

class SystemPropertiesApplet
{
public:
    IAsyncAction ExecuteAsync();
};
```

## Extension System

### C# Extensions

C# .NET extensions with ReadyToRun (current system):

```csharp
public class MyExtension : IApplet
{
    public void Execute()
    {
        // C# implementation
    }
}
```

### C++ Extensions

C++ extensions can be:

1. **C++/CLI** - Managed C++ (.NET compatible)
2. **Native DLL** - Pure C++ with C interface
3. **C++/WinRT** - Modern C++ Windows Runtime

## Framework Selection

### Configuration-Based

Users/developers can select framework via settings:

```json
{
  "uiFramework": "WinForms",  // or "WPF"
  "defaultFramework": "WinForms"
}
```

### Runtime Detection

The system can detect available frameworks:

```csharp
var availableFrameworks = FrameworkDetector.DetectAvailableFrameworks();
// Returns: ["WinForms", "WPF"]
```

## Build System

### Multi-Target Framework

Support building for multiple frameworks:

```xml
<Project>
  <PropertyGroup>
    <TargetFrameworks>net10.0-windows;net10.0-windows-wpf</TargetFrameworks>
  </PropertyGroup>
</Project>
```

### Conditional Compilation

Use conditional compilation for framework-specific code:

```csharp
#if WPF
    // WPF-specific code
#elif WINFORMS
    // WinForms-specific code
#endif
```

## Performance Considerations

### Framework Overhead

- **WinForms**: Lower overhead, faster startup
- **WPF**: Higher overhead, richer UI capabilities
- **Native C++**: Lowest overhead, but more complex interop

### Recommendations

- **Default**: Use WinForms for best performance
- **Rich UI**: Use WPF for advanced UI needs
- **Performance Critical**: Use native C++ components

## Migration Path

### Current State (WinForms Only)

1. Core logic is framework-agnostic
2. UI is tightly coupled to WinForms
3. Extensions are C# only

### Target State (Multi-Framework)

1. ✅ UI abstraction layer created
2. ✅ WinForms provider implements abstractions
3. ✅ WPF provider available as plugin
4. ✅ C++ extension support
5. ✅ Framework selection mechanism

## Examples

### Creating a WPF Extension

```csharp
// Extension uses UI abstractions, works with any framework
public class MyExtension : IApplet
{
    private IUIProvider _uiProvider;
    
    public MyExtension(IUIProvider uiProvider)
    {
        _uiProvider = uiProvider;
    }
    
    public void Execute()
    {
        var window = _uiProvider.CreateWindow();
        window.Title = "My Extension";
        window.Show();
    }
}
```

### Creating a C++ Extension

```cpp
// Native C++ extension
extern "C" {
    __declspec(dllexport) int ExecuteApplet(int appletId)
    {
        // C++ implementation
        return 0;
    }
}
```

## Future Enhancements

- **MAUI Support**: Cross-platform UI framework
- **Avalonia Support**: Cross-platform XAML framework
- **Rust Extensions**: Via native interop
- **Python Extensions**: Via Python.NET

## References

- [Architecture Documentation](architecture.md)
- [Extension Developer Guide](../extensions/developer-guide.md) (to be created)
- [C++ Interop Guide](../interop/cpp-guide.md) (to be created)


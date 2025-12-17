# WinForms UI Implementation

This project provides the WinForms implementation of the ClassicPanel UI abstraction layer. This is the **default and recommended** UI implementation for ClassicPanel.

## Overview

The WinForms implementation provides complete implementations of all UI abstraction interfaces using Windows Forms controls. This ensures:

- **Full Compatibility**: All abstraction interfaces are fully implemented
- **Performance**: WinForms provides lower overhead and faster startup
- **ReadyToRun Compatible**: Full .NET compatibility maintained (no AOT restrictions)
- **Native Windows Integration**: Seamless integration with Windows UI

## Components

### WinFormsUIProvider
The main factory class that implements `IUIProvider` and creates all WinForms UI components.

### Implementations

- **WinFormsWindow** - Implements `IWindow` using `System.Windows.Forms.Form`
- **WinFormsListView** - Implements `IListView` using `System.Windows.Forms.ListView`
- **WinFormsMenuBar** - Implements `IMenuBar` using `System.Windows.Forms.MenuStrip`
- **WinFormsCommandBar** - Implements `ICommandBar` using `System.Windows.Forms.ToolStrip`
- **WinFormsStatusBar** - Implements `IStatusBar` using `System.Windows.Forms.StatusStrip`
- **WinFormsTreeView** - Implements `ITreeView` using `System.Windows.Forms.TreeView`
- **WinFormsDialog** - Implements `IDialog` using `System.Windows.Forms.Form` with dialog styling
- **WinFormsContextMenu** - Implements `IContextMenu` using `System.Windows.Forms.ContextMenuStrip`

## Usage

The WinForms provider is automatically registered as the default framework:

```csharp
var loader = new FrameworkLoader();
var winFormsProvider = new WinFormsUIProvider();
loader.RegisterFramework(winFormsProvider);

// Use the provider to create UI components
var window = winFormsProvider.CreateWindow();
window.Title = "ClassicPanel";
window.Show();
```

## Features

- Complete implementation of all UI abstraction interfaces
- Event handling for all UI interactions
- Icon support in ListView
- Full menu system with submenus and shortcuts
- Toolbar/command bar support
- Status bar with multiple panels
- Tree view for hierarchical data
- Modal and modeless dialogs
- Context menus with submenus

## Performance

WinForms is the recommended UI framework for ClassicPanel because:
- Lower memory footprint
- Faster startup time
- Better performance for large lists
- Native Windows controls

## ReadyToRun Compatibility

This implementation is fully compatible with ReadyToRun compilation:
- All .NET features available (reflection, dynamic types, etc.)
- No trimming restrictions
- Full compatibility with .NET 10


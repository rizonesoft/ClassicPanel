# UI Abstraction Layer

This project provides the abstraction layer for multi-framework UI support in ClassicPanel. It defines framework-agnostic interfaces that allow the application to work with different UI frameworks (WinForms, WPF, etc.) through a common API.

## Overview

The UI Abstraction Layer separates the core application logic from the UI framework implementation. This allows:

- **Framework Independence**: Core logic doesn't depend on a specific UI framework
- **Plugin Support**: UI frameworks can be loaded as plugins
- **Multiple Frameworks**: Support for WinForms, WPF, and future frameworks
- **Easy Testing**: UI components can be mocked for unit testing

## Architecture

```
Core Logic → UI Abstractions → UI Implementation (WinForms/WPF/etc.)
```

## Core Interfaces

### IWindow
Represents an abstract window with properties and events for window management.

### IListView
Represents an abstract list view for displaying Control Panel items with various view modes.

### IMenuBar
Represents an abstract menu bar for application menus.

### ICommandBar
Represents an abstract command bar (toolbar) for quick actions.

### IStatusBar
Represents an abstract status bar for displaying status information.

### IUIProvider
Factory interface for creating UI components. Each UI framework implementation provides an IUIProvider that creates framework-specific implementations of the abstract interfaces.

### IFrameworkLoader
Interface for loading and managing UI framework implementations as plugins.

## Usage Example

```csharp
// Get the UI provider (loaded from plugin or default)
var uiProvider = frameworkLoader.LoadDefaultFramework();

// Create UI components using the provider
var window = uiProvider.CreateWindow();
window.Title = "ClassicPanel";
window.Width = 800;
window.Height = 600;

var listView = uiProvider.CreateListView();
listView.ViewMode = ListViewMode.LargeIcons;

// Add items
listView.AddItem(cplItem);

// Show the window
window.Show();
uiProvider.Run(window);
```

## Framework Implementations

- **WinForms**: `ClassicPanel.UI.WinForms` (default implementation)
- **WPF**: `ClassicPanel.UI.WPF` (optional, plugin-based)

## Data Models

The abstraction layer includes data models for:
- `MenuItemData`: Menu item configuration
- `CommandButtonData`: Command button configuration
- Event argument classes for various UI events

## Notes

- All interfaces are designed to be framework-agnostic
- Event handling uses standard .NET event patterns
- The abstraction layer does not depend on any specific UI framework
- Framework implementations are loaded as plugins at runtime



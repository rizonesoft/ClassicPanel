# Extensions Shared Components

This folder contains shared components and utilities that can be used across multiple ClassicPanel extensions.

## Purpose

- **Shared Utilities**: Common helper classes and functions
- **Shared Interfaces**: Common interfaces for extension development
- **Shared Resources**: Shared resources (icons, strings, etc.)
- **Extension Base Classes**: Base classes for common extension patterns

## Usage

Extensions can reference shared components by:
1. Adding a project reference to the shared components project (when created)
2. Using shared namespaces and utilities
3. Extending base classes for common functionality

## Structure

```
Shared/
├── README.md              # This file
├── Utilities/             # Shared utility classes (when created)
├── Interfaces/            # Shared interfaces (when created)
└── Resources/             # Shared resources (when created)
```

## Future Components

When needed, this folder will contain:
- `ClassicPanel.Extensions.Shared.csproj` - Shared library project
- Common extension interfaces and base classes
- Shared utility functions for CPL interop
- Common resource files


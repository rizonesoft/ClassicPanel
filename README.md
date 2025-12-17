# ClassicPanel

**The power of direct control, refined.**

ClassicPanel brings back the familiar Windows Control Panel interface with modern enhancements. Direct access to 100+ system settings and utilities, refined for speed, search, and seamless navigation.

## Overview

ClassicPanel is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11. It replicates the classic Windows 7 Control Panel interface, allowing users to manage Control Panel applets (.cpl files) in a familiar, traditional interface with modern performance improvements.

The application is built using cutting-edge .NET 10 with Native AOT compilation, resulting in a single, portable .exe file that requires no .NET runtime installation. ClassicPanel goes beyond a simple Control Panel replica — it's designed to be:

- **Fast**: Sub-second startup, optimized performance, efficient memory usage
- **Intuitive**: Real-time search with fuzzy matching, favorites system, command palette, category sidebar
- **Beautiful**: Modern UI with light/dark themes, smooth animations, professional styling
- **Comprehensive**: Support for 100+ Windows Control Panel items with extensible architecture

## Technology Stack

- **Framework**: .NET 10 (LTS)
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Deployment**: Native AOT (Ahead-of-Time Compilation)
- **Target OS**: Windows 10 and Windows 11

## Features

- **Classic Interface**: Traditional ListView-based interface that feels familiar
- **Direct Access**: Direct access to 100+ system settings and utilities
- **Modern Enhancements**: 
  - Real-time search with fuzzy matching
  - Favorites and Quick Access system
  - Command Palette (Ctrl+K) for quick actions
  - Category-based sidebar for intuitive navigation
  - Light/Dark/High Contrast themes
- **Multiple View Modes**: Large Icons, Small Icons, List, and Details views
- **Complete Control Panel Items**: All Windows XP/7/8/10 Control Panel items including Hardware, Security, System, Network, Programs, Appearance, and more
- **Portable**: Single-file executable, no installation required
- **Standalone**: No .NET runtime installation required (Native AOT)
- **Extensible**: CPL extension system for developers

## Building

### Prerequisites

- Visual Studio 2026 (or VS Code with C# Dev Kit)
- .NET 10 SDK (LTS release)

### Build Instructions

```bash
dotnet build -c Release
dotnet publish -c Release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## Project Structure

```
ClassicPanel/
├── src/                    # Source code
│   ├── UI/                # User interface components
│   ├── Core/              # Core business logic
│   └── Resources/         # Application resources
├── build/                 # Build outputs and scripts
│   ├── installer/         # InnoSetup installation scripts
│   ├── debug/             # Debug build output
│   └── release/           # Release build output
├── resources/             # Additional resources
│   └── installer/         # Installation script templates
├── docs/                  # Documentation
│   ├── dev/              # Developer documentation
│   └── user/             # User-facing documentation
├── standards/             # Coding standards and guidelines
└── prompts/              # AI prompt templates
```

## Development

See [Developer Documentation](docs/dev/README.md) for detailed development guidelines.

## License

Copyright © 2025 Rizonetech (Pty) Ltd

Website: https://rizonesoft.com
Developer: Derick Payne


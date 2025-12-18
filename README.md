# ClassicPanel

**The power of direct control, refined.**

ClassicPanel brings back the familiar Windows Control Panel interface with modern enhancements. Direct access to 100+ system settings and utilities, refined for speed, search, and seamless navigation.

## Overview

ClassicPanel is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11. It replicates the classic Windows 7 Control Panel interface, allowing users to manage Control Panel applets (.cpl files) in a familiar, traditional interface with modern performance improvements.

The application is built using .NET 10, resulting in a single, self-contained .exe file with full WinForms compatibility. ClassicPanel goes beyond a simple Control Panel replica — it's designed to be:

- **Fast**: Sub-second startup, optimized performance, efficient memory usage
- **Intuitive**: Real-time search with fuzzy matching, favorites system, command palette, category sidebar
- **Beautiful**: Modern UI with light/dark themes, smooth animations, professional styling
- **Comprehensive**: Support for 100+ Windows Control Panel items with extensible architecture

## Technology Stack

- **Framework**: .NET 10 (LTS)
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Deployment**: Framework-dependent executable with ReadyToRun + Quick JIT (requires .NET 10 runtime - installer can bundle .NET runtime)

**Note**: Framework-dependent deployment requires .NET 10 runtime (installer can bundle .NET runtime). ReadyToRun pre-compiles code at build time for instant execution, while Quick JIT provides fast compilation for dynamic code at runtime. Both work together to optimize startup performance while maintaining full .NET compatibility.

## Platform Requirements

- **Operating System**: Windows 10 (build 10240 or later) or Windows 11 (build 22000 or later)
- **Architecture**: 64-bit (x64) only - 32-bit systems are not supported
- **Unsupported**: Windows 7, Windows 8, Windows 8.1

ClassicPanel automatically validates platform requirements on startup and displays a user-friendly error message if your system doesn't meet the requirements.

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
- **Portable**: Single executable with dependencies, no installation required
- **Framework-Dependent**: Requires .NET 10 runtime (installer can bundle .NET runtime for automatic installation)
- **Extensible**: CPL extension system for developers

## Building

### Prerequisites

- Visual Studio 2026 (or VS Code with C# Dev Kit)
- .NET 10 SDK (LTS release)

### Build Instructions

**Using Build Scripts (Recommended):**

```bash
# Build Debug configuration
.\build\build.bat Debug

# Build Release configuration (automatically publishes)
.\build\build.bat Release

# Build Debug and force publish
.\build\build.bat Debug --publish
```

**Manual Build:**

```bash
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

**Build Outputs:**
- Debug: `build/debug/ClassicPanel.exe` (~290 KB)
- Release: `build/release/ClassicPanel.exe` (~2.6 MB, framework-dependent with ReadyToRun)

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
│   │   └── net10.0-windows/win-x64/ClassicPanel.exe (~290 KB)
│   ├── release/           # Release build output
│   │   └── ClassicPanel.exe (~2.6 MB)
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


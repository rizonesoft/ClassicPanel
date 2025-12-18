# ClassicPanel Project Structure

## Overview

This document describes the complete directory and file structure for the ClassicPanel project.

## Root Directory

```
ClassicPanel/
├── src/                          # Source code
├── build/                        # Build outputs and scripts
├── resources/                    # Additional resources
├── docs/                         # Documentation
├── standards/                    # Coding standards and guidelines
├── prompts/                      # AI prompt templates
├── Graphics/                     # Graphics assets (existing)
├── README.md                     # Main project README
├── TODO.md                       # Development roadmap
├── PROJECT_STRUCTURE.md          # This file
└── .gitignore                    # Git ignore rules
```

## Source Code (`src/`)

```
src/
├── ClassicPanel.csproj           # .NET 10 project file (ReadyToRun enabled)
├── Program.cs                    # Application entry point
├── UI/
│   ├── MainWindow.cs             # Main window class
│   └── MainWindow.Designer.cs    # Windows Forms designer file
├── Core/
│   ├── CplInterop.cs             # P/Invoke definitions for Control Panel API
│   ├── CplLoader.cs              # CPL file loading and management
│   ├── PlatformValidator.cs      # Platform validation (OS version, architecture)
│   ├── VersionInfo.cs            # Version information from assembly
│   ├── AppConstants.cs           # Application-wide constants
│   ├── Category.cs                # Category enumeration and management
│   ├── Localization/              # Localization infrastructure
│   │   ├── LocalizationManager.cs # Localization manager
│   │   └── README.md              # Localization documentation
│   ├── Theme/                     # Theme system infrastructure
│   │   ├── ThemeManager.cs        # Theme manager (light/dark/system mode)
│   │   ├── ThemeData.cs           # Theme data (colors and styling)
│   │   └── WindowsThemeInterop.cs # Windows API interop for theme/accent colors
│   └── Performance/                # Performance monitoring infrastructure
│       ├── PerformanceMonitor.cs  # Main performance monitoring class
│       ├── PerformanceMetrics.cs   # Performance metrics data structure
│       ├── OperationTimer.cs      # Operation timing utility
│       └── README.md              # Performance monitoring documentation
│   └── PlatformValidator.cs      # Platform validation (OS version, architecture)
├── Extensions/                   # CPL extension projects
│   ├── ExtensionTemplate/        # Extension template project
│   │   ├── ExtensionTemplate.csproj
│   │   ├── ExtensionTemplate.cs
│   │   └── README.md
│   └── Shared/                   # Shared components for extensions
│       └── README.md
└── Resources/
    └── App.ico                   # Application icon
```

## Build System (`build/`)

```
build/
├── build.bat                     # Build script (Windows)
├── build.sh                      # Build script (Linux/macOS)
├── installer/                    # InnoSetup installer scripts
├── release/                      # Release build output
│   ├── ClassicPanel.exe          # ~2.6 MB (framework-dependent with ReadyToRun)
│   └── system/                   # Release extension DLLs (framework-dependent)
│       └── ExtensionName.dll     # ~3-5000 KB each
├── debug/                        # Debug build output
│   └── net10.0-windows/win-x64/ # Framework/RID subdirectory
│       ├── ClassicPanel.exe      # ~290 KB
│       └── system/               # Debug extension DLLs
│           └── ExtensionName.dll
└── obj/                          # Intermediate build files
```

## Resources (`resources/`)

```
resources/
└── installer/
    └── ClassicPanel.iss.template # InnoSetup installer template
```

## Documentation (`docs/`)

```
docs/
├── dev/                          # Developer documentation
│   ├── README.md                 # Developer docs index
│   ├── getting-started.md        # Development setup guide
│   ├── architecture.md           # Application architecture
│   ├── build-system.md           # Build instructions
│   ├── api-reference.md          # API documentation
│   └── testing.md                # Testing guidelines
└── user/                         # User-facing documentation
    ├── README.md                 # User docs index
    ├── introduction.md           # What is ClassicPanel?
    ├── installation.md           # Installation guide
    ├── quick-start.md            # Quick start guide
    ├── user-manual.md            # Complete user manual
    ├── faq.md                    # Frequently asked questions
    └── troubleshooting.md        # Troubleshooting guide
```

## Standards (`standards/`)

```
standards/
├── coding-standards.md           # C# coding standards and conventions
└── commit-messages.md            # Git commit message format
```

## Prompt Templates (`prompts/`)

```
prompts/
├── feature-template.md           # Template for feature development
├── general-request-template.md   # Template for general requests
└── polish-template.md            # Template for polish and refinement
```

## Key Files

### Project Configuration
- **`src/ClassicPanel.csproj`**: .NET 10 project with ReadyToRun + Quick JIT enabled
- **`.gitignore`**: Git ignore rules for .NET projects

### Documentation
- **`README.md`**: Main project overview
- **`TODO.md`**: Complete development roadmap with phases and tasks

### Build Scripts
- **`build/build.bat`**: Windows build script for Release/Debug builds

## Technology Stack

- **.NET 10** (LTS)
- **C# 14**
- **Windows Forms** (WinForms)
- **ReadyToRun + Quick JIT** (Pre-compiled code at build time + fast runtime compilation for dynamic code, full .NET compatibility maintained)
- **P/Invoke** (Platform Invocation Services)

## Development Workflow

1. Use prompt templates from `prompts/` for AI-assisted development
2. Follow coding standards from `standards/`
3. Update documentation in `docs/` as you develop
4. Use `build/build.bat` to build and publish
5. Check `TODO.md` for tasks and roadmap

## Next Steps

See [TODO.md](TODO.md) for the complete development roadmap starting from Phase 0.


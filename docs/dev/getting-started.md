# Getting Started

This guide will help you set up your development environment and get started with ClassicPanel development.

## Prerequisites

- **Visual Studio 2026** (Professional or higher recommended)
  - Location: `C:\Program Files\Microsoft Visual Studio\18\Professional`
- **.NET 10 SDK** (LTS release)
  - Installed with Visual Studio 2026
- **Windows 10 or Windows 11** development machine

## Initial Setup

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/ClassicPanel.git
cd ClassicPanel
```

### 2. Verify .NET 10 SDK Installation (For Development Only)

**Note**: This is for DEVELOPERS only. End users do NOT need to install .NET - the runtime is included in the distributed executable.

```bash
dotnet --version
```

Should show version `10.x.x` or higher.

### 3. Restore Dependencies

**Using the solution file (recommended):**
```bash
dotnet restore ClassicPanel.sln
```

**Or from the project directory:**
```bash
cd src
dotnet restore
```

### 4. Build the Project

**Using the solution file (recommended - builds main app + all extensions):**
```bash
dotnet build ClassicPanel.sln -c Release -p:GenerateAssemblyInfo=false
```

**Or from the project directory:**
```bash
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

### 5. Run the Application

```bash
cd src
dotnet run
```

**Or from Visual Studio:**
- Open `ClassicPanel.sln`
- The solution includes:
  - **Main Application** folder: ClassicPanel (main app)
  - **Extensions** folder: All extension projects (e.g., ExtensionTemplate)
- Press F5 to run

## Solution Structure

The solution file (`ClassicPanel.sln`) organizes all projects:

```
ClassicPanel.sln
├── Main Application
│   └── ClassicPanel            # Main application project
└── Extensions
    └── ExtensionTemplate        # Extension template project
    └── ... (future extensions)
```

**Solution Features:**
- Organized into solution folders (Main Application, Extensions)
- Platform: win-x64 only (Debug|win-x64, Release|win-x64)
- Extensions automatically build when main app builds (via MSBuild targets)
- All projects use shared build configuration (Directory.Build.props)

## Project Structure

```
ClassicPanel/
├── ClassicPanel.sln           # Solution file (main app + extensions)
├── src/
│   ├── ClassicPanel.csproj    # Main application project
│   ├── Program.cs              # Entry point
│   ├── UI/
│   │   └── MainWindow.cs       # Main window
│   ├── Core/
│   │   ├── CplInterop.cs       # P/Invoke definitions
│   │   └── CplLoader.cs        # CPL loading logic
│   ├── Extensions/             # Extension projects
│   │   └── ExtensionTemplate/  # Extension template
│   └── Resources/
│       └── App.ico             # Application icon
├── build/                      # Build outputs
├── docs/                       # Documentation
├── standards/                  # Coding standards
└── prompts/                    # AI prompt templates
```

## Development Workflow

### Using Visual Studio 2026

1. Open `ClassicPanel.sln` in Visual Studio 2026
2. Solution Explorer shows:
   - **Main Application** folder → ClassicPanel project
   - **Extensions** folder → All extension projects
3. Select the configuration (Debug|win-x64 or Release|win-x64)
4. Build: `Ctrl+Shift+B` or `Build > Build Solution`
   - Builds main app and all extensions automatically
5. Run: `F5` or `Debug > Start Debugging`
6. Debug: Set breakpoints and use debugger tools

### Using Command Line

**Using the solution file (recommended):**
```bash
# Build solution (main app + all extensions)
dotnet build ClassicPanel.sln -c Debug -p:GenerateAssemblyInfo=false
dotnet build ClassicPanel.sln -c Release -p:GenerateAssemblyInfo=false
```

**Or from the project directory:**
```bash
cd src
# Build all configurations
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Run (uses Debug by default)
dotnet run

# Build Release (Framework-Dependent with ReadyToRun)
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Run specific builds
.\build\debug\ClassicPanel.exe
.\build\release\ClassicPanel.exe
```

**Build Outputs:**
- Debug: `build/debug/ClassicPanel.exe` (~290 KB)
- Release: `build/release/ClassicPanel.exe` (~2.6 MB, framework-dependent)

Or use the build script:
```bash
.\build\build.bat Release
```

## Build Considerations

Since ClassicPanel uses ReadyToRun + Quick JIT compilation:

- **Full .NET Compatibility**: All features work, including reflection and dynamic types
- **ReadyToRun Enabled**: Pre-compiles code at build time for instant execution while maintaining full compatibility
- **Quick JIT Enabled**: Fast compilation for dynamic code at runtime (Tier 0), then recompiles hot paths with full optimization (Tier 1)
- **Test Debug builds** during development: `.\build\build.bat Debug`
- **Test Release builds** before major releases: `.\build\build.bat Release`
- **Use NativeLibrary** for dynamic loading instead of `[DllImport]` (we use this for .cpl files)

## Testing with Control Panel Applets

1. Create a `system` folder next to the executable
2. Copy `.cpl` files from `C:\Windows\System32\` (e.g., `appwiz.cpl`, `desk.cpl`)
3. Run the application
4. Applets should appear in the ListView

**Note**: Some system `.cpl` files may require administrator privileges to execute.

## Common Tasks

### Adding a New Feature

1. Check [TODO.md](../../TODO.md) for planned features
2. Use the [feature template](../../prompts/feature-template.md)
3. Follow [coding standards](../../standards/coding-standards.md)
4. Update documentation
5. Test thoroughly
6. Commit and push

### Fixing a Bug

1. Reproduce the bug
2. Identify the root cause
3. Write a fix
4. Test the fix
5. Update tests if applicable
6. Commit and push

### Updating Documentation

- Developer docs: `docs/dev/`
- User docs: `docs/user/`
- Keep docs in sync with code changes

## Next Steps

- Read the [Architecture Documentation](architecture.md)
- Review [Coding Standards](../../standards/coding-standards.md)
- Check [TODO.md](../../TODO.md) for tasks
- Explore the codebase

## Getting Help

- Check existing documentation in `docs/`
- Review code comments
- Check [TODO.md](../../TODO.md) for context
- Consult [standards](../../standards/) for guidelines


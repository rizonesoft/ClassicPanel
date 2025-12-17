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

### 2. Verify .NET 10 Installation

```bash
dotnet --version
```

Should show version `10.x.x` or higher.

### 3. Restore Dependencies

```bash
cd src
dotnet restore
```

### 4. Build the Project

```bash
dotnet build -c Release
```

### 5. Run the Application

```bash
dotnet run
```

Or from Visual Studio:
- Open `ClassicPanel.sln`
- Press F5 to run

## Project Structure

```
ClassicPanel/
├── src/
│   ├── ClassicPanel.csproj    # Project file
│   ├── Program.cs              # Entry point
│   ├── UI/
│   │   └── MainWindow.cs       # Main window
│   ├── Core/
│   │   ├── CplInterop.cs       # P/Invoke definitions
│   │   └── CplLoader.cs        # CPL loading logic
│   └── Resources/
│       └── App.ico             # Application icon
├── build/                      # Build outputs
├── docs/                       # Documentation
├── standards/                  # Coding standards
└── prompts/                    # AI prompt templates
```

## Development Workflow

### Using Visual Studio 2026

1. Open the solution/project in Visual Studio 2026
2. Select the configuration (Debug/Release)
3. Build: `Ctrl+Shift+B` or `Build > Build Solution`
4. Run: `F5` or `Debug > Start Debugging`
5. Debug: Set breakpoints and use debugger tools

### Using Command Line

```bash
# Build
dotnet build -c Release

# Run
dotnet run

# Publish (Native AOT)
dotnet publish -c Release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## Native AOT Considerations

Since ClassicPanel uses Native AOT compilation:

- **Avoid reflection** where possible (some reflection is supported but limited)
- **Test with AOT builds** regularly: `dotnet publish -c Release`
- **Use NativeLibrary** for dynamic loading instead of `[DllImport]`
- Some .NET features may not be available in AOT mode

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


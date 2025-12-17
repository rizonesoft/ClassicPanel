# Build System

ClassicPanel uses .NET 10 SDK with MSBuild for building applications with ReadyToRun compilation. Builds output to the `build/` directory.

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 (optional, for IDE support)

## Build Configurations

### Debug
- Includes debug symbols
- No optimizations
- Faster build times

### Release
- Optimized code
- Single-file executable
- Self-contained (includes .NET runtime bundled in the executable - end users do NOT need to install .NET separately)

## Building

### Command Line

#### Standard Build
Use the provided build script:
```bash
.\build\build.bat Release
```

Or manually:
```bash
cd src
dotnet build -c Release -p:OutputPath=..\build\release\ -p:IntermediateOutputPath=..\build\obj\release\ -p:GenerateAssemblyInfo=false
```

#### Run Application
```bash
dotnet run
```

#### Publish (Self-Contained Single File with ReadyToRun)
```bash
dotnet publish -c Release `
  -p:PublishSingleFile=true `
  -p:PublishReadyToRun=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:SelfContained=true `
  -p:RuntimeIdentifier=win-x64
```

Output will be in: `build/publish/`

Or use the build script which automatically publishes Release builds:
```bash
.\build\build.bat Release
```

### Visual Studio 2026

1. Open `src/ClassicPanel.csproj` in Visual Studio
2. Select configuration: Debug or Release
3. Build: `Ctrl+Shift+B` or `Build > Build Solution`
4. Run: `F5` or `Debug > Start Debugging`
5. Publish: `Build > Publish` (configure for self-contained single file with ReadyToRun)

## Project File Configuration

Key settings in `ClassicPanel.csproj`:

```xml
<PropertyGroup>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <WindowsTargetPlatformVersion>10.0.19041.0</WindowsTargetPlatformVersion>
  <SupportedOSPlatform>windows10.0.10240</SupportedOSPlatform>
  <UseWindowsForms>true</UseWindowsForms>
</PropertyGroup>
```

**Note**: ClassicPanel uses .NET 10 with ReadyToRun compilation. `SelfContained`, `PublishSingleFile`, and `PublishReadyToRun` are specified during publish operations.

**Important Distinction:**
- **`SelfContained=true`** - Includes the .NET runtime in the executable (end users do NOT need .NET installed). This is what makes ClassicPanel work without requiring .NET installation.
- **`PublishReadyToRun=true`** - Enabled optimization that pre-compiles code to native format for faster startup. ReadyToRun maintains full .NET compatibility - all features work (reflection, dynamic types, etc.).

All .NET features are fully supported (reflection, dynamic types, etc.).

## Build Output

### Standard Build
- **Debug**: `build/debug/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
  - Includes debug symbols (`.pdb` files)
  - All runtime DLLs in same directory
  - Intermediate files: `build/obj/debug/`
- **Release**: `build/release/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
  - Optimized code
  - All runtime DLLs in same directory
  - Intermediate files: `build/obj/release/`

**Note**: Standard builds output to TFM/RID subdirectories (`net10.0-windows/win-x64/`). This is standard .NET behavior. The executables are framework-dependent and require .NET 10 runtime to be installed.

### Self-Contained Publish
- **Location**: `build/publish/ClassicPanel.exe`
- **Size**: ~122 MB (includes entire .NET runtime bundled inside + ReadyToRun pre-compiled code)
- **Contains**: Single `.exe` file (self-contained, includes .NET runtime bundled inside)
- **End Users**: Do NOT need to install .NET separately - the runtime is included in the executable
- **ReadyToRun**: Enabled for faster startup - pre-compiles code to native format
- **Distribution**: This is the file you distribute to end users

## Build Scripts

Build scripts are located in the `build/` directory and provide a convenient way to build and publish ClassicPanel.

### build.bat (Windows)

The Windows build script supports Debug and Release configurations with automatic publishing for Release builds.

**Usage:**
```batch
# Build Debug configuration
.\build\build.bat Debug

# Build Release configuration (automatically publishes)
.\build\build.bat Release

# Build Debug and force publish
.\build\build.bat Debug --publish
```

**Features:**
- Supports Debug and Release configurations
- Automatically publishes Release builds (self-contained with ReadyToRun)
- Optional `--publish` flag to force publishing for any configuration
- Comprehensive error handling and validation
- File size reporting for published executables
- Clear output messages and status reporting

**Output:**
- Debug builds: `build/debug/net10.0-windows/win-x64/ClassicPanel.exe`
- Release builds: `build/release/net10.0-windows/win-x64/ClassicPanel.exe`
- Published builds: `build/publish/ClassicPanel.exe` (~122 MB)

### build.sh (Linux/macOS/Unix)

The cross-platform build script provides the same features as the Windows script.

**Usage:**
```bash
# Build Debug configuration
./build/build.sh Debug

# Build Release configuration (automatically publishes)
./build/build.sh Release

# Build Debug and force publish
./build/build.sh Debug --publish
```

**Features:**
- Same functionality as build.bat
- Cross-platform compatibility
- Proper error handling and validation

## Troubleshooting

### Build Output Issues

**Error**: "Duplicate assembly attributes"
- **Solution**: Use `GenerateAssemblyInfo=false` when redirecting output paths

**Error**: Large executable size
- **Solution**: This is normal for self-contained deployments with ReadyToRun. The executable includes the entire .NET runtime (~115-125 MB). ReadyToRun adds pre-compiled native code which increases size slightly but improves startup performance. End users do NOT need to install .NET separately - it's all bundled in the single executable file.

### Build Errors

**Error**: ".NET 10 SDK not found"
- **Solution**: Install .NET 10 SDK or verify installation

**Error**: "WinForms not available"
- **Solution**: Ensure `<UseWindowsForms>true</UseWindowsForms>` in `.csproj`

## Continuous Integration

Example GitHub Actions workflow:

```yaml
name: Build

on: [push, pull_request]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'
      - name: Build
        run: dotnet build -c Release
      - name: Publish
        run: dotnet publish -c Release -p:PublishSingleFile=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64
```

## Distribution Builds

For distribution, create a release build:

1. Clean previous builds
2. Build in Release configuration
3. Publish as self-contained single file with ReadyToRun
4. Test the standalone executable from `build/publish/ClassicPanel.exe`
5. Package with InnoSetup (see `build/installer/`)

**Complete Distribution Build Workflow:**
```bash
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Test the published executable
.\build\publish\ClassicPanel.exe
```

The published executable is ready for distribution at `build/publish/ClassicPanel.exe` (~122 MB, self-contained, includes .NET runtime).

## Extension Build Strategy

**IMPORTANT**: ClassicPanel uses a hybrid deployment model:
- **Main Application** (ClassicPanel.exe): Self-contained with ReadyToRun (~120 MB)
- **Extensions** (.cpl/.dll files): Framework-dependent (typically 50 KB - 5 MB each)

Extensions **DO NOT** use self-contained deployment. They rely on the .NET runtime provided by the main ClassicPanel.exe application. This prevents having 100+ extensions each being 120 MB (which would total 12+ GB).

### Extension Output Configuration

Extension projects are automatically configured by `Directory.Build.props`:
- Projects in `src/Extensions/` are automatically detected
- Extensions output to `system/` subfolder:
  - Debug: `build/debug/system/net10.0-windows/win-x64/ExtensionName.dll`
  - Release: `build/release/system/net10.0-windows/win-x64/ExtensionName.dll`
  - Published: `build/publish/system/net10.0-windows/win-x64/ExtensionName.dll`
- Extensions are automatically configured as framework-dependent (NOT self-contained)
- Extensions use the runtime from the main ClassicPanel.exe

### Building Extensions

```bash
# Build a specific extension
cd src/Extensions/ExtensionName
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Build all extensions (when build script is created)
.\build\build-extensions.bat
```

See [Extension Deployment Guide](extension-deployment.md) for detailed information on building extensions.

## References

- [.NET CLI Documentation](https://learn.microsoft.com/dotnet/core/tools/)
- [.NET Deployment](https://learn.microsoft.com/dotnet/core/deploying/)
- [MSBuild Documentation](https://learn.microsoft.com/visualstudio/msbuild/msbuild)
- [Extension Deployment Guide](extension-deployment.md) - Building framework-dependent extensions


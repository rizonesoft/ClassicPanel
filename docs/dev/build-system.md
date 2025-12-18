# Build System

ClassicPanel uses .NET 10 SDK with MSBuild for building applications with ReadyToRun + Quick JIT compilation. Builds output to the `build/` directory.

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
- Framework-dependent (requires .NET 10 runtime - installer can bundle .NET runtime)

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

#### Build Release (Framework-Dependent with ReadyToRun)
```bash
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

Output will be in: `build/release/`

Or use the build script:
```bash
.\build\build.bat Release
```

### Visual Studio 2026

1. Open `ClassicPanel.sln` in Visual Studio
2. Solution includes:
   - **Main Application** folder: ClassicPanel project
   - **Extensions** folder: All extension projects
3. Select configuration: Debug|win-x64 or Release|win-x64
4. Build: `Ctrl+Shift+B` or `Build > Build Solution`
   - Automatically builds main app and all extensions
5. Run: `F5` or `Debug > Start Debugging`
6. Build: Right-click ClassicPanel project → `Build` (Release configuration outputs to `build/release/`)

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

**Note**: ClassicPanel uses .NET 10 with ReadyToRun + Quick JIT compilation. Both are enabled in the project file for Release builds.

**Important Distinction:**
- **`SelfContained=false`** - Framework-dependent deployment (requires .NET 10 runtime). The installer can bundle the .NET 10 runtime installer for automatic installation.
- **ReadyToRun** - Pre-compiles code at build time to native format for instant execution. ReadyToRun maintains full .NET compatibility - all features work (reflection, dynamic types, etc.).
- **Quick JIT** - Fast compilation for dynamic/reflection code at runtime (Tier 0), then recompiles frequently used methods with full optimization (Tier 1). Works alongside ReadyToRun to optimize startup performance.

All .NET features are fully supported (reflection, dynamic types, etc.).

## Build Output

### Standard Build
- **Debug**: `build/debug/ClassicPanel.exe` (~290 KB)
  - Includes debug symbols (`.pdb` files)
  - All runtime DLLs in same directory
  - Intermediate files: `build/obj/debug/`
- **Release**: `build/release/ClassicPanel.exe` (~2.6 MB)
  - Optimized code with ReadyToRun + Quick JIT
  - ReadyToRun pre-compiles code at build time
  - Quick JIT handles dynamic code at runtime
  - All runtime DLLs in same directory
  - Intermediate files: `build/obj/release/`

**Note**: Debug builds output to TFM/RID subdirectories (`net10.0-windows/win-x64/`). Release builds output directly to `build/release/` as configured in `Directory.Build.props`. The executables are framework-dependent and require .NET 10 runtime to be installed.

**Release Build Details:**
- **Size**: ~2.6 MB (executable) + ~11 MB (SkiaSharp native DLL) = ~14 MB total
- **Contains**: Executable and DLLs (framework-dependent, requires .NET 10 runtime)
- **End Users**: Need .NET 10 runtime installed (installer can bundle .NET runtime installer)
- **ReadyToRun**: Enabled for faster startup - pre-compiles code to native format at build time
- **Quick JIT**: Enabled for faster startup - fast compilation for dynamic code at runtime, then recompiles hot paths with full optimization
- **Distribution**: This is the file you distribute to end users

## Build Scripts

Build scripts are located in the `build/` directory and provide a convenient way to build ClassicPanel.

### build.bat (Windows)

The Windows build script supports Debug and Release configurations.

**Usage:**
```batch
# Build Debug configuration
.\build\build.bat Debug

# Build Release configuration
.\build\build.bat Release

# Build Debug configuration
.\build\build.bat Debug
```

**Features:**
- Supports Debug and Release configurations
- Builds Release configuration (framework-dependent with ReadyToRun)
- Comprehensive error handling and validation
- File size reporting for Release executables
- Clear output messages and status reporting

**Output:**
- Debug builds: `build/debug/ClassicPanel.exe`
- Release builds: `build/release/ClassicPanel.exe`
- Release builds: `build/release/ClassicPanel.exe` (~2.6 MB)

### build.sh (Linux/macOS/Unix)

The cross-platform build script provides the same features as the Windows script.

**Usage:**
```bash
# Build Debug configuration
./build/build.sh Debug

# Build Release configuration
./build/build.sh Release

# Build Debug configuration
./build/build.sh Debug
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
      - name: Build Release
        run: dotnet build -c Release -p:GenerateAssemblyInfo=false
```

## Distribution Builds

For distribution, create a release build:

1. Clean previous builds
2. Build in Release configuration
3. Build Release configuration (framework-dependent with ReadyToRun)
4. Test the standalone executable from `build/release/ClassicPanel.exe`
5. Package with InnoSetup (see `build/installer/`) - installer should check for and install .NET 10 runtime

**Complete Distribution Build Workflow:**
```bash
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Test the Release build
.\build\release\ClassicPanel.exe
```

The Release executable is ready for distribution at `build/release/ClassicPanel.exe` (~2.6 MB, framework-dependent, requires .NET 10 runtime).

## Extension Build Strategy

**IMPORTANT**: ClassicPanel uses a hybrid deployment model:
- **Main Application** (ClassicPanel.exe): Framework-dependent with ReadyToRun (~2.6 MB, requires .NET 10 runtime)
- **Extensions** (.cpl/.dll files): Framework-dependent (typically 50 KB - 5 MB each)

Extensions **DO NOT** use self-contained deployment. They rely on the .NET runtime provided by the main ClassicPanel.exe application. This prevents having 100+ extensions each being 120 MB (which would total 12+ GB).

### Extension Output Configuration

Extension projects are automatically configured by `Directory.Build.props`:
- Projects in `src/Extensions/` are automatically detected
- Extensions output to `system/` subfolder:
  - Debug: `build/debug/system/net10.0-windows/win-x64/ExtensionName.dll`
  - Release: `build/release/system/net10.0-windows/win-x64/ExtensionName.dll`
  - Release: `build/release/system/ExtensionName.dll`
- Extensions are automatically configured as framework-dependent (NOT self-contained)
- Extensions use the runtime from the main ClassicPanel.exe

### Building Extensions

Extensions are automatically built when building the main application. MSBuild targets in `Directory.Build.targets` automatically discover and build all extensions.

```bash
# Build main app + extensions (automatic)
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
# Extensions are automatically discovered and built

# Build extensions independently
dotnet build -t:BuildExtensions -c Release -p:GenerateAssemblyInfo=false

# Build a specific extension
cd src/Extensions/ExtensionName
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

**Available Extension Targets:**
- `BuildExtensions` - Build all extensions
- `CleanExtensions` - Clean all extensions
- `RebuildExtensions` - Rebuild all extensions
- `BuildExtensions` - Build all extensions (Release configuration)

See [Extension Deployment Guide](extension-deployment.md) for detailed information on building extensions.

## References

- [.NET CLI Documentation](https://learn.microsoft.com/dotnet/core/tools/)
- [.NET Deployment](https://learn.microsoft.com/dotnet/core/deploying/)
- [MSBuild Documentation](https://learn.microsoft.com/visualstudio/msbuild/msbuild)
- [Extension Deployment Guide](extension-deployment.md) - Building framework-dependent extensions


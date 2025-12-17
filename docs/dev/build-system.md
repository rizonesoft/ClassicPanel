# Build System

ClassicPanel uses .NET 10 SDK with MSBuild for building, and Native AOT for standalone executable generation.

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
- Native AOT compilation
- Single-file executable

## Building

### Command Line

#### Standard Build
```bash
cd src
dotnet build -c Release
```

#### Run Application
```bash
dotnet run
```

#### Publish (Native AOT)
```bash
dotnet publish -c Release `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:SelfContained=true `
  -p:RuntimeIdentifier=win-x64
```

Output will be in: `bin/Release/net10.0-windows/win-x64/publish/`

### Visual Studio 2026

1. Open `src/ClassicPanel.csproj` in Visual Studio
2. Select configuration: Debug or Release
3. Build: `Ctrl+Shift+B` or `Build > Build Solution`
4. Run: `F5` or `Debug > Start Debugging`
5. Publish: `Build > Publish` (configure for Native AOT)

## Project File Configuration

Key settings in `ClassicPanel.csproj`:

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <UseWindowsForms>true</UseWindowsForms>
</PropertyGroup>
```

## Build Output

### Standard Build
- Location: `bin/Release/net10.0-windows/`
- Contains: `.exe`, `.dll`, `.pdb` files

### Native AOT Publish
- Location: `bin/Release/net10.0-windows/win-x64/publish/`
- Contains: Single `.exe` file (standalone, no dependencies)

## Build Scripts

### build.bat (Windows)
```batch
@echo off
dotnet build -c Release
if %ERRORLEVEL% EQU 0 (
    echo Build succeeded!
    dotnet publish -c Release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
)
```

### build.sh (Cross-platform)
```bash
#!/bin/bash
dotnet build -c Release
if [ $? -eq 0 ]; then
    echo "Build succeeded!"
    dotnet publish -c Release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
fi
```

## Troubleshooting

### Native AOT Issues

**Error**: "Trimming is not supported"
- **Solution**: Ensure `PublishAot` is set correctly in `.csproj`

**Error**: "Type is not available in AOT"
- **Solution**: Avoid unsupported reflection or dynamic types

**Error**: Large executable size
- **Solution**: This is normal for Native AOT. The executable includes the entire runtime.

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
        run: dotnet publish -c Release -p:PublishSingleFile=true
```

## Distribution Builds

For distribution, create a release build:

1. Clean previous builds
2. Build in Release configuration
3. Publish with Native AOT
4. Test the standalone executable
5. Package with InnoSetup (see `build/installer/`)

## References

- [.NET CLI Documentation](https://learn.microsoft.com/dotnet/core/tools/)
- [Native AOT Deployment](https://learn.microsoft.com/dotnet/core/deploying/native-aot/)
- [MSBuild Documentation](https://learn.microsoft.com/visualstudio/msbuild/msbuild)


# Extension Deployment Strategy

## Overview

ClassicPanel uses a **framework-dependent deployment model**:
- **Main Application**: Framework-dependent with ReadyToRun + Quick JIT (requires .NET 10 runtime)
- **Extensions**: Framework-dependent (rely on .NET 10 runtime)

This approach ensures:
- ✅ Small application size (~2.6 MB main app, not ~120 MB)
- ✅ Extensions are small and lightweight (typically 50 KB - 5 MB each)
- ✅ All components share the same .NET 10 runtime
- ✅ Installer can bundle .NET 10 runtime for automatic installation

## Architecture

```
ClassicPanel/
├── ClassicPanel.exe          # Framework-dependent (~2.6 MB) - Requires .NET 10 runtime
└── system/
    ├── SystemProperties.cpl  # Framework-dependent (~200 KB) - Uses .NET 10 runtime
    ├── TaskManager.cpl       # Framework-dependent (~150 KB)
    ├── RegistryEditor.cpl    # Framework-dependent (~300 KB)
    └── ... (100+ extensions) # All framework-dependent
```

**Total Size**: ~2.6 MB (main app) + ~10-50 MB (all extensions combined) = ~13-53 MB (plus .NET 10 runtime if not installed)

## Extension Build Configuration

### ❌ DO NOT Use Self-Contained for Extensions

**Wrong** (would create 100+ × 120 MB = 12+ GB):
```xml
<PropertyGroup>
  <SelfContained>true</SelfContained>  <!-- ❌ DON'T DO THIS -->
  <PublishSingleFile>true</PublishSingleFile>
</PropertyGroup>
```

### ✅ DO Use Framework-Dependent for Extensions

**Correct** (small DLLs that use main app's runtime):
```xml
<PropertyGroup>
  <TargetFramework>net10.0-windows</TargetFramework>
  <UseWindowsForms>true</UseWindowsForms>
  <!-- NO SelfContained - extensions use main app's runtime -->
  <!-- NO PublishSingleFile - regular DLL output -->
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
</PropertyGroup>
```

## Build Outputs

### Main Application
- **Location**: `build/release/ClassicPanel.exe`
- **Type**: Framework-dependent with ReadyToRun + Quick JIT
- **Size**: ~2.6 MB
- **Contains**: Application code only (requires .NET 10 runtime)

### Extensions
- **Debug Build**: `build/debug/system/ExtensionName.dll`
- **Release Build**: `build/release/system/ExtensionName.dll`
- **Type**: Framework-dependent DLLs
- **Size**: Typically 3 KB - 5 MB each (depending on functionality)
- **Contains**: Extension code only (no runtime)

**Note**: 
- Extension output paths are automatically configured by `Directory.Build.props`
- Projects in `src/Extensions/` are automatically detected and configured to output to the `system/` subfolder
- Extensions are NOT published separately - they're framework-dependent DLLs that use the main app's runtime
- For distribution, copy extension DLLs from `build/release/system/` to the `system/` folder next to the published `ClassicPanel.exe`

## Extension Project Template

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <OutputType>Library</OutputType>  <!-- DLL, not executable -->
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    
    <!-- Framework-dependent - NO SelfContained -->
    <!-- Extensions use the runtime from ClassicPanel.exe -->
  </PropertyGroup>
</Project>
```

## Why This Works

1. **Main App Provides Runtime**: When ClassicPanel.exe starts, it loads the .NET runtime
2. **Extensions Load into Same Process**: Extensions are loaded as DLLs into the main app's process
3. **Shared Runtime**: All extensions use the same runtime instance from the main app
4. **No Redundancy**: Each extension doesn't need its own copy of the runtime

## Building Extensions with MSBuild Targets

MSBuild targets are automatically configured in `Directory.Build.targets` to discover and build all extensions.

### Automatic Extension Building

Extensions are **automatically built** when you build the main application:

```bash
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
# Extensions are automatically discovered and built
```

### Building Extensions Independently

You can build extensions independently using MSBuild targets:

```bash
cd src

# Build all extensions (Debug)
dotnet build -t:BuildExtensions -c Debug -p:GenerateAssemblyInfo=false

# Build all extensions (Release)
dotnet build -t:BuildExtensions -c Release -p:GenerateAssemblyInfo=false

# Clean all extensions
dotnet build -t:CleanExtensions -c Release -p:GenerateAssemblyInfo=false

# Rebuild all extensions
dotnet build -t:RebuildExtensions -c Release -p:GenerateAssemblyInfo=false

# Publish all extensions
dotnet build -t:PublishExtensions -c Release -p:GenerateAssemblyInfo=false
```

### Available Targets

- **`BuildExtensions`**: Builds all discovered extension projects
- **`CleanExtensions`**: Cleans all extension projects
- **`RebuildExtensions`**: Rebuilds all extension projects
- **`PublishExtensions`**: Publishes all extension projects (framework-dependent)

### How It Works

1. **Discovery**: MSBuild automatically discovers all `.csproj` files in `src/Extensions/`
2. **Automatic Building**: When building the main app, extensions are built automatically
3. **Output Location**: Extensions output to `system/` subfolder (configured in `Directory.Build.props`)
4. **Framework-Dependent**: All extensions are built as framework-dependent (not self-contained)

## Testing Extensions

1. Build main app (self-contained): `.\build\build.bat Release`
2. Build extensions (framework-dependent): Build to `system/` folder
3. Run ClassicPanel.exe - it will load extensions from `system/` folder
4. Extensions automatically use the runtime from the main app

## Benefits of This Approach

### File Size
- **Self-contained extensions**: 100 × 120 MB = 12 GB ❌
- **Framework-dependent extensions**: 120 MB + (100 × 500 KB avg) = ~170 MB ✅

### Distribution
- Single download with main app
- Extensions are small and lightweight
- Easy to update individual extensions

### Performance
- All extensions share the same runtime instance
- No redundant runtime loading
- Faster extension loading (no runtime initialization)

### Compatibility
- All extensions use the same .NET version (ensured by main app)
- No version conflicts between extensions
- Consistent behavior across all extensions

## Important Notes

1. **Main App Must Be Self-Contained**: The main ClassicPanel.exe MUST be self-contained because it provides the runtime
2. **Extensions Must Match Framework**: Extensions must target the same .NET version as the main app
3. **Runtime Identifier**: Both main app and extensions should use `win-x64` (or match)
4. **Testing**: Always test extensions with the actual self-contained main app

## Example: Creating a New Extension

1. Create extension project in `src/Extensions/MyExtension/`
2. Set `OutputType` to `Library` (DLL)
3. **Do NOT** set `SelfContained=true`
4. Configure build output to `system/` folder
5. Build - extension will be small (no runtime included)
6. Copy to `system/` folder next to ClassicPanel.exe
7. Run ClassicPanel.exe - extension loads and uses main app's runtime

## Troubleshooting

### Extension fails to load
- **Check**: Extension targets same .NET version as main app
- **Check**: Extension is compiled for `win-x64`
- **Check**: Extension is in `system/` folder next to ClassicPanel.exe

### Extension shows error about missing .NET
- **Cause**: Extension was built as self-contained (shouldn't be)
- **Fix**: Rebuild extension without `SelfContained=true`

### Multiple runtime versions conflict
- **Cause**: Some extensions might have different .NET versions
- **Fix**: Ensure all extensions target `net10.0-windows` (same as main app)

## References

- [.NET Framework-Dependent Deployments](https://learn.microsoft.com/dotnet/core/deploying/#framework-dependent-deployments)
- [Self-Contained Deployments](https://learn.microsoft.com/dotnet/core/deploying/#self-contained-deployments)
- [Extension Development Guide](cpl-extension-guide.md) (when created)


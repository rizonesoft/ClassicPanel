# Testing Guidelines

## Overview

Testing strategy for ClassicPanel includes unit tests, integration tests, and manual testing on target platforms.

## Testing Types

### Unit Tests

Test individual components in isolation:

- `CplLoader` functionality
- `CplInterop` P/Invoke definitions
- Resource extraction utilities
- Configuration management

### Integration Tests

Test component interactions:

- End-to-end CPL loading
- Icon extraction from real .cpl files
- Applet enumeration
- ListView population

### Manual Testing

Test user-facing features:

- UI interactions
- View mode switching
- Applet execution
- Error handling and user feedback

## Test Structure

```
ClassicPanel.Tests/
├── ClassicPanel.Tests.csproj
├── Core/
│   ├── CplLoaderTests.cs
│   └── CplInteropTests.cs
└── Integration/
    └── CplLoadingTests.cs
```

## Writing Tests

### Example Unit Test

```csharp
using Xunit;
using ClassicPanel.Core;

namespace ClassicPanel.Tests.Core;

public class CplLoaderTests
{
    [Fact]
    public void LoadSystemFolder_CreatesFolderIfMissing()
    {
        // Arrange
        var loader = new CplLoader();
        var systemPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "system");
        
        // Clean up if exists
        if (Directory.Exists(systemPath))
            Directory.Delete(systemPath, true);

        // Act
        loader.LoadSystemFolder();

        // Assert
        Assert.True(Directory.Exists(systemPath));
    }
}
```

## Testing with Real CPL Files

1. Copy sample .cpl files to test directory
2. Test with known .cpl files (e.g., `appwiz.cpl`, `desk.cpl`)
3. Test error cases (corrupted files, missing exports)

## Release Build Testing

**Important**: Always test with all builds, especially the published build before releases.

```bash
cd src

# Build all configurations
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Publish self-contained executable
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Test Debug build
.\build\debug\net10.0-windows\win-x64\ClassicPanel.exe

# Test Release build
.\build\release\net10.0-windows\win-x64\ClassicPanel.exe

# Test Published build (MOST IMPORTANT - this is what users get)
.\build\publish\ClassicPanel.exe
```

**Build Locations:**
- Debug: `build/debug/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
- Release: `build/release/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
- Published: `build/publish/ClassicPanel.exe` (~122 MB, self-contained with ReadyToRun)

The published build creates a self-contained single-file executable with ReadyToRun that matches the distribution build. ReadyToRun pre-compiles code for faster startup.

## Test Checklist

Before committing:

- [ ] All unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing on Windows 10
- [ ] Manual testing on Windows 11
- [ ] Test with Release build (self-contained single file with ReadyToRun)
- [ ] Test error cases
- [ ] Test with various .cpl files

## Continuous Integration

Tests should run automatically on:
- Pull requests
- Pushes to main branch
- Scheduled runs

## References

- [xUnit Documentation](https://xunit.net/)
- [.NET Testing Documentation](https://learn.microsoft.com/dotnet/core/testing/)


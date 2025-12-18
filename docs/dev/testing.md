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

**Important**: Always test with Debug build during development, and test Release build before major releases.

```bash
cd src

# Build all configurations
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Build Debug and Release (framework-dependent executables)
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Test Debug build (for development testing)
.\build\debug\ClassicPanel.exe

# Test Release build (before major releases)
.\build\release\ClassicPanel.exe
```

**Build Locations:**
- Debug: `build/debug/ClassicPanel.exe` (~290 KB)
- Release: `build/release/ClassicPanel.exe` (~2.6 MB, framework-dependent with ReadyToRun + Quick JIT)

The Release build uses ReadyToRun + Quick JIT for optimal startup performance. ReadyToRun pre-compiles code at build time for instant execution, while Quick JIT provides fast compilation for dynamic code at runtime.

## Test Checklist

Before committing:

- [ ] All unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing on Windows 10
- [ ] Manual testing on Windows 11
- [ ] Test with Debug build (for development testing)
- [ ] Test with Release build (before major releases)
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


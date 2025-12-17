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

## Native AOT Testing

**Important**: Test with Native AOT builds, not just regular builds.

```bash
dotnet publish -c Release
# Test the published executable
```

Some features may work in regular builds but fail in AOT builds.

## Test Checklist

Before committing:

- [ ] All unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing on Windows 10
- [ ] Manual testing on Windows 11
- [ ] Test with Native AOT build
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


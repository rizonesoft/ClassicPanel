# Coding Standards

## Language & Framework
- **Language**: C# 14
- **Framework**: .NET 10 (LTS)
- **UI Framework**: Windows Forms (WinForms)
- **Compilation**: ReadyToRun + Quick JIT
  - **ReadyToRun**: Pre-compiles code at build time for instant execution (full .NET compatibility)
  - **Quick JIT**: Fast compilation for dynamic code at runtime (Tier 0), then recompiles hot paths with full optimization (Tier 1)

## Code Style

### Naming Conventions
- **Classes**: PascalCase (e.g., `CplLoader`, `MainWindow`)
- **Methods**: PascalCase (e.g., `LoadSystemFolder`, `GetAppletCount`)
- **Properties**: PascalCase (e.g., `Name`, `Icon`, `Path`)
- **Fields**: camelCase with underscore prefix for private (e.g., `_items`, `_currentViewMode`)
- **Constants**: PascalCase (e.g., `CPL_INIT`, `DefaultIconSize`)
- **Local Variables**: camelCase (e.g., `filePath`, `appletCount`)
- **Parameters**: camelCase (e.g., `hModule`, `appletIndex`)

### File Organization
- One class per file
- File name matches class name
- Namespace matches folder structure
- Use partial classes for Designer files

### Indentation & Formatting
- Use 4 spaces for indentation (not tabs)
- Use Unix-style line endings (LF)
- Remove trailing whitespace
- Maximum line length: 120 characters
- Use Visual Studio's default C# formatting (Ctrl+K, Ctrl+D)

### Comments & Documentation
- Use XML documentation comments for all public members
- Use `///` for XML comments
- Use `//` for inline explanations
- Document complex algorithms
- Document non-obvious workarounds
- Include examples in documentation when helpful

Example:
```csharp
/// <summary>
/// Loads all Control Panel applets from the system folder.
/// </summary>
/// <returns>The number of applets loaded.</returns>
/// <exception cref="DirectoryNotFoundException">Thrown when system folder does not exist.</exception>
public int LoadApplets()
{
    // Implementation
}
```

## Project Structure

### Namespace Organization
```
ClassicPanel
├── ClassicPanel.Core        // Core business logic
│   ├── CplInterop          // P/Invoke definitions
│   ├── CplLoader           // CPL file loading
│   └── CplItem             // Data models
├── ClassicPanel.UI          // User interface
│   └── MainWindow          // Main form
└── ClassicPanel.Resources   // Resources and constants
```

## Code Quality

### Error Handling
- Always use try-catch blocks for external operations (file I/O, P/Invoke, etc.)
- Log errors using debug output (`System.Diagnostics.Debug.WriteLine`)
- Show user-friendly error messages for user-facing errors
- Never silently swallow exceptions without logging
- Use specific exception types when possible

### Resource Management
- Dispose of resources that implement `IDisposable`
- Use `using` statements for deterministic cleanup
- For NativeLibrary handles, track and cleanup on application exit
- Close file handles, streams, and other resources promptly

### Embedded Resources
- SVG files are embedded in the main assembly using `EmbeddedResource` in the project file
- Resource naming format: `ClassicPanel.Resources.Actions.{filename}.svg`
- Use `SvgFileLoader.GetEmbeddedResourceName()` to construct resource names
- Load resources using `Assembly.GetEntryAssembly().GetManifestResourceStream()`
- Example: `ClassicPanel.Resources.Actions.refresh.svg`

### Null Safety
- Use nullable reference types (`string?`, `Icon?`)
- Check for null before dereferencing
- Use null-coalescing operators (`??`, `??=`) when appropriate
- Use null-conditional operators (`?.`, `?[]`) when appropriate

### P/Invoke Guidelines
- Define all P/Invoke methods in `CplInterop` class
- Use `nint` for handles and pointers (not `IntPtr`)
- Specify `CharSet` explicitly in structs
- Use `SetLastError = true` and check `Marshal.GetLastWin32Error()` after calls
- Document expected return values and error codes

## Performance

### Build Considerations
- Use `NativeLibrary.Load` instead of `[DllImport]` for dynamic loading (already implemented)
- Prefer value types over reference types when appropriate for performance
- Test Release builds with ReadyToRun + Quick JIT to verify functionality and startup performance
- Full .NET compatibility - ReadyToRun + Quick JIT maintains full support for reflection and dynamic types

### UI Performance
- Avoid blocking the UI thread
- Use `BeginInvoke` for long-running operations
- Cache icons and resources
- Use efficient ListView population (batch updates when possible)

## Best Practices

### General
- Keep methods focused and single-purpose
- Extract complex logic into separate methods
- Use meaningful variable names
- Avoid magic numbers (use constants)
- Prefer composition over inheritance
- Keep classes cohesive and loosely coupled

### WinForms Specific
- Use Designer files for UI layout
- Handle events in code-behind (not in Designer)
- Use data binding where appropriate
- Implement proper window state management
- Handle DPI awareness correctly

### .NET 10 / C# 14 Features
- Use modern C# features (pattern matching, records, etc.) when beneficial
- Use `file`-scoped namespaces
- Prefer `Span<T>` and `Memory<T>` for performance-critical code
- Use `IAsyncEnumerable<T>` for async sequences
- Leverage primary constructors when appropriate

## Testing

### Unit Testing
- Write unit tests for core logic
- Test error cases
- Test edge cases
- Use descriptive test method names

### Integration Testing
- Test with real .cpl files
- Test on clean Windows systems
- Test on Windows 10 and Windows 11
- Test with Release builds (self-contained single file)

## Version Control

### Commit Messages
- Follow conventional commit format (see `commit-messages.md`)
- Write clear, descriptive commit messages
- Reference TODO items or issues when applicable

### Branching
- Use `main` branch for stable code
- Create feature branches for significant changes
- Keep commits atomic and logical

## Documentation Requirements

### Code Comments
- All public APIs must have XML documentation
- Complex algorithms must be commented
- Non-obvious code must be explained

### External Documentation
- Keep `docs/dev/` updated with architecture changes
- Keep `docs/user/` updated with feature changes
- Update `README.md` for major changes
- Update `TODO.md` as tasks are completed

## References
- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/)
- [C# 14 Language Reference](https://learn.microsoft.com/dotnet/csharp/)
- [WinForms Documentation](https://learn.microsoft.com/dotnet/desktop/winforms/)
- [.NET Deployment](https://learn.microsoft.com/dotnet/core/deploying/)


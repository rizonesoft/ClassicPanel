# Developer Documentation

Welcome to the ClassicPanel developer documentation. This section contains technical documentation for developers working on the project.

## Getting Started

1. [Getting Started Guide](getting-started.md) - Set up your development environment
2. [Architecture](architecture.md) - Understand the application architecture
3. [Build System](build-system.md) - Learn how to build and publish the application

## Documentation Index

### Core Documentation
- [Architecture](architecture.md) - Application architecture and design
- [Multi-Framework Architecture](multi-framework-architecture.md) - WPF, C++, and multi-framework support
- [API Reference](api-reference.md) - API documentation
- [Build System](build-system.md) - Build and deployment instructions
- [Testing](testing.md) - Testing guidelines and procedures

### Development Guides
- [Getting Started](getting-started.md) - Initial setup and first steps
- [CPL Interop Guide](cpl-interop.md) - Control Panel interop implementation
- [Deployment Explained](deployment-explained.md) - Understanding framework-dependent vs ReadyToRun + Quick JIT
- [Performance Comparison](performance-comparison.md) - Framework-dependent vs self-contained performance analysis
- [Extension Deployment](extension-deployment.md) - Building framework-dependent extensions (IMPORTANT: Extensions are framework-dependent)

## Standards

See the [standards](../standards/) folder for:
- [Coding Standards](../standards/coding-standards.md)
- [Commit Message Format](../standards/commit-messages.md)

## Project Structure

```
ClassicPanel/
├── src/                    # Source code
│   ├── UI/                # User interface
│   ├── Core/              # Core business logic
│   └── Resources/         # Resources
├── docs/                  # Documentation
│   ├── dev/              # Developer docs (this folder)
│   └── user/             # User-facing docs
├── standards/             # Coding standards and guidelines
├── prompts/              # AI prompt templates
└── build/                # Build outputs
```

## Contributing

1. Read the [coding standards](../standards/coding-standards.md)
2. Follow the [commit message format](../standards/commit-messages.md)
3. Update documentation as you make changes
4. Test thoroughly before committing
5. Use the prompt templates from `prompts/` folder when working with AI tools

## Resources

- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/)
- [C# 14 Language Reference](https://learn.microsoft.com/dotnet/csharp/)
- [WinForms Documentation](https://learn.microsoft.com/dotnet/desktop/winforms/)
- [.NET Deployment](https://learn.microsoft.com/dotnet/core/deploying/)
- [Windows Control Panel API](https://learn.microsoft.com/windows/win32/shell/control-panel-applications)


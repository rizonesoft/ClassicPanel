# Feature Development Template

---

## Developer Persona

You are a senior C# .NET 10 developer with extensive experience in Windows Forms, Native AOT compilation, P/Invoke interop, and modern desktop application development. You have deep knowledge of Windows API, Control Panel architecture, performance optimization, and creating beautiful, intuitive user interfaces. You follow best practices, write clean, maintainable code, and always prioritize user experience, performance, and code quality.

## Project Context

**ClassicPanel** is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11. It goes beyond a simple Control Panel - it's designed to be:

- **Fast**: Sub-second startup, optimized performance, efficient memory usage
- **Intuitive**: Search, favorites, command palette, category sidebar, smooth navigation
- **Beautiful**: Modern UI with themes, animations, professional styling, responsive design
- **Comprehensive**: Support for 100+ settings/utilities with extensible architecture

### Technology Stack
- **Framework**: .NET 10 (LTS) with Native AOT
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Target OS**: Windows 10 and Windows 11 (x64)
- **Deployment**: Standalone executable (no .NET runtime required)

### Key Features & Vision
- **Classic Windows Control Panel Items**: Complete implementation of all Windows XP/7/8/10 Control Panel items including:
  - Hardware & Devices (Printers, Mouse, Keyboard, Sound, Display, Personalization, Power Options, Device Manager, Game Controllers, Pen & Touch, Scanners & Cameras)
  - User Accounts & Security (User Accounts, Parental Controls, Windows Security, BitLocker, Credential Manager)
  - Programs & Features (Programs and Features, Windows Features, Default Programs, AutoPlay, Windows Update)
  - Administrative Tools (Computer Management, Event Viewer, Services, Task Scheduler, Performance Monitor, etc.)
  - Network (Network and Sharing Center, Network Connections, Windows Firewall, Internet Options)
  - System Tools (System Properties, Fonts, Folder Options, Indexing Options, Search, Backup & Restore, Recovery, Action Center)
  - Mobile & Sync (Sync Center, Offline Files, Windows Mobility Center)
  - Appearance (Date and Time, Region and Language, Ease of Access, Speech Recognition)
- Category-based sidebar for intuitive navigation (scales to 100+ items)
- Real-time search with fuzzy matching
- Favorites and Quick Access system
- Command Palette (Ctrl+K) for quick actions
- Light/Dark/High Contrast themes with customization
- Smooth animations and transitions
- Full accessibility support (keyboard navigation, screen readers)
- Portable and installed modes (settings in file/registry)
- CPL extension system for developers
- System Properties clone as first extension
- Troubleshooting tools integration
- Update checking system
- Exception handling with intuitive dialogs
- SVG icon support library
- Ribbon UI implementation
- Multi-framework architecture supporting WinForms (default), WPF, C++, and future UI frameworks

### Project Structure
```
ClassicPanel/
├── src/
│   ├── ClassicPanel.csproj
│   ├── Core/                    # Business logic, CPL interop
│   ├── UI/                      # User interface components (WinForms)
│   │   └── Abstractions/        # UI abstraction layer for multi-framework support
│   ├── Extensions/              # CPL extension projects
│   │   └── system/              # Compiled .cpl files go here
│   └── Resources/
├── docs/
│   ├── dev/                    # Developer documentation
│   │   ├── architecture.md     # Core architecture
│   │   ├── multi-framework-architecture.md  # Multi-framework design
│   │   └── ...
│   └── user/                   # User-facing documentation
├── standards/                  # Coding standards and guidelines
├── build/                      # Build outputs and scripts
└── Graphics/                   # Icons and images
```

## Task Description

[Describe the specific feature or TODO item to implement]

**Phase**: [e.g., Phase 4: User Interface Components]
**Task ID**: [e.g., 4.12: Search & Filter System]
**Related Features**: [List related features from TODO.md if applicable]

## Requirements

- [ ] Requirement 1
- [ ] Requirement 2
- [ ] Requirement 3

## Implementation Steps

### 1. Code Implementation
- [ ] **Analyze requirements** - Review TODO.md, architecture docs, and existing code
- [ ] **Design solution** - Consider performance, extensibility, and user experience
- [ ] **Implement core functionality** - Follow SOLID principles and design patterns
- [ ] **Add comprehensive error handling** - Use try-catch, null checks, validation
- [ ] **Follow coding standards** - See `standards/coding-standards.md`
- [ ] **Add XML documentation comments** - Document all public APIs and complex logic
- [ ] **Use appropriate naming conventions** - PascalCase for public, camelCase for private
- [ ] **Optimize for performance** - Consider async/await, lazy loading, caching
- [ ] **Ensure Native AOT compatibility** - Avoid reflection, dynamic types, unsupported features
- [ ] **Add accessibility support** - Keyboard navigation, screen reader labels

### 2. Testing
- [ ] Write unit tests (if applicable) for core logic
- [ ] Test with sample .cpl files or realistic data
- [ ] Test error cases and edge conditions
- [ ] Test on Windows 10 (multiple versions)
- [ ] Test on Windows 11 (multiple versions)
- [ ] Test with Native AOT build (not just regular build)
- [ ] Test performance (startup time, memory usage, responsiveness)
- [ ] Test accessibility (keyboard navigation, screen readers)
- [ ] Test with high DPI displays (125%, 150%, 200%)
- [ ] Test theme switching if UI component

### 3. Build & Run
```bash
# Build in Release mode
dotnet build -c Release

# Test with Native AOT publish
dotnet publish -c Release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

# Run application
dotnet run
```
- [ ] Build succeeds without errors or warnings
- [ ] Native AOT compilation succeeds
- [ ] Application runs without crashes
- [ ] Feature works as expected
- [ ] No memory leaks detected
- [ ] Performance meets targets (< 1s startup, responsive UI)

### 4. Documentation Updates (CRITICAL - Always Required)

**Developer Documentation** (`docs/dev/`):
- [ ] Update architecture documentation if structure changed
- [ ] Update API reference if new APIs added
- [ ] Update build system docs if build process changed
- [ ] Update getting started guide if setup changed
- [ ] Document new patterns or approaches used
- [ ] Update CPL extension guide if extension system changed
- [ ] Add code examples if introducing new concepts

**User Documentation** (`docs/user/`):
- [ ] Update user manual if user-facing features added
- [ ] Update quick start guide if onboarding changed
- [ ] Update FAQ if common questions arise
- [ ] Update troubleshooting guide if new issues possible
- [ ] Add screenshots for new UI features
- [ ] Update feature list in introduction

**Standards Documentation** (`standards/`):
- [ ] Update coding standards if new patterns established
- [ ] Update commit message examples if format changes
- [ ] Document new best practices discovered

**Project Documentation**:
- [ ] Update README.md if major features added
- [ ] Update TODO.md:
  - [ ] Mark completed tasks as `- [x]` (change `- [ ]` to `- [x]`)
  - [ ] Add new tasks if needed (as `- [ ]`)
  - [ ] Update task descriptions if requirements changed
- [ ] Update PROJECT_STRUCTURE.md if structure changed

**Code Documentation**:
- [ ] All public APIs have XML documentation
- [ ] Complex algorithms have inline comments
- [ ] Non-obvious code is explained
- [ ] Examples provided where helpful

### 5. Code Review Checklist
- [ ] Code follows standards from `standards/coding-standards.md`
- [ ] Error handling is comprehensive and user-friendly
- [ ] Comments explain complex logic and non-obvious decisions
- [ ] No hardcoded values (use constants/config/settings)
- [ ] Memory/resource cleanup is correct (using statements, Dispose calls)
- [ ] No warnings in build output
- [ ] Code is testable and follows SOLID principles
- [ ] Performance considerations addressed (async, lazy loading, caching)
- [ ] Accessibility support included (keyboard nav, ARIA labels)
- [ ] Native AOT compatibility verified
- [ ] UI is responsive and handles edge cases gracefully

### 6. Update TODO.md (Before Committing)
- [ ] Open `TODO.md`
- [ ] Find the task(s) you completed
- [ ] Change `- [ ]` to `- [x]` for completed tasks
- [ ] Add any new tasks discovered during implementation (as `- [ ]`)
- [ ] Update task descriptions if needed
- [ ] Save TODO.md

### 7. Git Commit & Push
```bash
git add .
git commit -m "feat: [Brief description of feature]

- Detailed change 1
- Detailed change 2
- Performance: [if performance improvements]
- UI: [if UI changes]
- Docs: [if documentation updated]

Phase: [X.Y]
Task: [X.Y.Z]
Closes: [GitHub issue if applicable]"

git push origin main
```
- [ ] Commit message follows format from `standards/commit-messages.md`
- [ ] All related files committed (code, tests, docs, TODO.md)
- [ ] Changes pushed to GitHub
- [ ] Commit references TODO.md task (Phase: X.Y, Task: X.Y.Z)

## References & Resources

### Required Reading Before Implementation
- [Coding Standards](standards/coding-standards.md) - **MUST READ**
- [Architecture Documentation](docs/dev/architecture.md) - **MUST READ**
- [Multi-Framework Architecture](docs/dev/multi-framework-architecture.md) - **MUST READ** (for UI components)
- [API Reference](docs/dev/api-reference.md) - For existing APIs
- [TODO List](TODO.md) - For task context and related features (now includes 100+ Windows Control Panel items)

### Additional Resources
- [Getting Started Guide](docs/dev/getting-started.md)
- [Build System](docs/dev/build-system.md)
- [Commit Message Format](standards/commit-messages.md)
- [Windows Control Panel API](https://learn.microsoft.com/windows/win32/shell/control-panel-applications)
- [Native AOT Deployment](https://learn.microsoft.com/dotnet/core/deploying/native-aot/)

## Quality Standards

### Code Quality
- Clean, readable, maintainable code
- Follow SOLID principles
- DRY (Don't Repeat Yourself)
- Appropriate use of design patterns
- Comprehensive error handling

### Performance
- Startup time: < 1 second
- Memory usage: Efficient, no leaks
- UI responsiveness: 60 FPS for animations
- Lazy loading for non-critical features
- Caching where appropriate

### User Experience
- Intuitive and discoverable
- Fast and responsive
- Beautiful and modern
- Accessible to all users
- Helpful error messages

### Documentation
- **Always update documentation** - This is not optional
- Keep docs in sync with code changes
- Document patterns, not just APIs
- Include examples and use cases
- Make docs discoverable and searchable

**Remember**: ClassicPanel should be fast, intuitive, beautiful, and comprehensive. Every feature should enhance the user experience and maintain the high quality standards of the application.

# Polish & Refinement Template

---

**You are a senior C# .NET 10 developer.** ClassicPanel must be **fast** (<1s startup), **intuitive** (search, favorites), **beautiful** (themes, animations), and **comprehensive** (100+ items).

**CRITICAL REQUIREMENTS:**
1. ✅ Follow `standards/coding-standards.md`
2. ✅ Always update documentation (docs/dev/, docs/user/, standards/)
3. ✅ Mark TODO.md tasks as `- [x]` when complete
4. ✅ Test with Release build (includes ReadyToRun for faster startup)
5. ✅ Ensure performance maintained or improved
6. ✅ No breaking changes (backward compatible)
7. ✅ Update TODO.md before committing

**Build & Test:**
```bash
dotnet build -c Release
dotnet publish -c Release -p:PublishSingleFile=true
```

**Commit Format:**
```
polish: [Area/Component] improvements
Phase: [X.Y], Task: [X.Y.Z]
```

---

## Developer Persona

You are a senior C# .NET 10 developer with extensive experience in Windows Forms, P/Invoke interop, and modern desktop application development. You have deep knowledge of Windows API, Control Panel architecture, performance optimization, and creating beautiful, intuitive user interfaces. You follow best practices, write clean, maintainable code, and always prioritize user experience, performance, and code quality.

**IMPORTANT**: ClassicPanel uses .NET 10 with ReadyToRun compilation for faster startup. ReadyToRun maintains full .NET compatibility - all features are available (reflection, dynamic types, etc.). Native AOT is NOT used.

## Project Context

**ClassicPanel** is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11. It goes beyond a simple Control Panel - it's designed to be:

- **Fast**: Sub-second startup, optimized performance, efficient memory usage
- **Intuitive**: Search, favorites, command palette, category sidebar, smooth navigation
- **Beautiful**: Modern UI with themes, animations, professional styling, responsive design
- **Comprehensive**: Support for 100+ settings/utilities with extensible architecture

### Technology Stack
- **Framework**: .NET 10 (LTS) with ReadyToRun compilation for faster startup
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Target OS**: Windows 10 and Windows 11 (x64)
- **Deployment**: Self-contained single-file executable with ReadyToRun (includes .NET runtime - no separate .NET installation required for end users; ReadyToRun enables faster startup)

### Key Features & Vision
- **Classic Windows Control Panel Items**: Complete implementation of all Windows XP/7/8/10 Control Panel items (100+ items across Hardware, Security, System, Network, Programs, Appearance, Mobile, and Backup categories) - match original Windows layouts exactly
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
│   ├── Core/              # Business logic, CPL interop
│   ├── UI/                # User interface components
│   ├── Extensions/        # CPL extension projects
│   └── Resources/
├── docs/
│   ├── dev/              # Developer documentation
│   └── user/             # User-facing documentation
├── standards/            # Coding standards and guidelines
└── build/                # Build outputs and scripts
```

## Area to Polish

[Specify the feature, component, or code section to improve. Be specific about what needs polishing.]

**Files Affected**: [List relevant files and their paths]

**Phase**: [e.g., Phase 7: Polish & Enhancement]
**Task ID**: [e.g., 7.1: Visual Polish]

## Current Issues

- [ ] Issue 1: [Detailed description of the problem]
- [ ] Issue 2: [Detailed description of the problem]
- [ ] Issue 3: [Detailed description of the problem]

## Improvements to Make

### Code Quality
- [ ] Refactor for better readability and maintainability
- [ ] Improve error handling (comprehensive, user-friendly)
- [ ] Add missing XML documentation comments
- [ ] Remove code duplication (DRY principle)
- [ ] Optimize performance bottlenecks
- [ ] Fix memory leaks or resource cleanup issues
- [ ] Improve code organization and structure
- [ ] Apply SOLID principles where applicable
- [ ] Improve naming conventions for clarity
- [ ] Add defensive programming (null checks, validation)

### User Experience
- [ ] Improve visual appearance (alignment with design system)
- [ ] Enhance error messages (actionable, helpful)
- [ ] Add loading indicators with smooth animations
- [ ] Improve responsiveness (reduce UI blocking)
- [ ] Add keyboard shortcuts for common actions
- [ ] Improve tooltips (contextual, helpful)
- [ ] Enhance accessibility (keyboard nav, screen readers)
- [ ] Improve empty states (helpful, encouraging)
- [ ] Better visual feedback for user actions
- [ ] Smooth transitions and animations
- [ ] Consistent spacing and layout (8px grid)

### Stability & Reliability
- [ ] Fix edge cases and boundary conditions
- [ ] Add defensive programming and validation
- [ ] Improve error recovery mechanisms
- [ ] Add input validation (comprehensive)
- [ ] Handle null references properly
- [ ] Improve exception handling (catch, log, recover)
- [ ] Add logging for debugging
- [ ] Handle resource cleanup properly

### Performance
- [ ] Optimize algorithms and data structures
- [ ] Add caching where appropriate
- [ ] Reduce memory allocations
- [ ] Optimize UI updates (avoid unnecessary redraws)
- [ ] Profile and fix bottlenecks
- [ ] Implement lazy loading where beneficial
- [ ] Use async/await for long operations
- [ ] Optimize icon/image loading
- [ ] Reduce startup time if applicable

### Visual Polish
- [ ] Apply consistent styling (design system)
- [ ] Improve typography (fonts, sizes, weights, spacing)
- [ ] Refine spacing and layout (8px grid)
- [ ] Add subtle shadows and depth
- [ ] Improve icon quality and consistency
- [ ] Enhance color usage (theme-aware)
- [ ] Polish micro-interactions
- [ ] Ensure high-DPI awareness
- [ ] Test on multiple DPI settings (100%, 125%, 150%, 200%)

## Testing

- [ ] Test all affected functionality thoroughly
- [ ] Test error cases and edge conditions
- [ ] Test on Windows 10 (multiple versions)
- [ ] Test on Windows 11 (multiple versions)
- [ ] Test with Release build (includes ReadyToRun for faster startup)
- [ ] Verify no regressions in existing features
- [ ] Performance testing (compare before/after)
- [ ] Accessibility testing (keyboard nav, screen readers)
- [ ] High DPI testing (125%, 150%, 200%)
- [ ] Theme switching testing (light/dark/high contrast)
- [ ] Memory leak testing
- [ ] Stress testing with large datasets

## Build & Test (CRITICAL - Always Required)

**After completing tasks, always build, publish, and test:**

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

**Build Checklist:**
- [ ] Debug build succeeds without warnings
- [ ] Release build succeeds without warnings
- [ ] Publish succeeds (creates `build/publish/ClassicPanel.exe` ~122 MB)
- [ ] Debug build runs smoothly
- [ ] Release build runs smoothly
- [ ] Published build runs smoothly (CRITICAL - test this!)
- [ ] Build outputs to correct directories:
  - Debug: `build/debug/net10.0-windows/win-x64/`
  - Release: `build/release/net10.0-windows/win-x64/`
  - Published: `build/publish/`
- [ ] Improvements verified visually and functionally in all builds
- [ ] Performance maintained or improved
- [ ] No new memory leaks introduced

## Documentation Updates (CRITICAL - Always Required)

**Developer Documentation** (`docs/dev/`):
- [ ] Update code comments (inline documentation)
- [ ] Update developer documentation if patterns changed
- [ ] Update architecture docs if structure changed
- [ ] Update API reference if APIs modified
- [ ] Document refactoring decisions
- [ ] Update examples if code patterns changed

**User Documentation** (`docs/user/`):
- [ ] Update user documentation if UX changed
- [ ] Update screenshots if UI changed
- [ ] Update quick start if workflow changed
- [ ] Update FAQ if new questions arise

**Standards Documentation** (`standards/`):
- [ ] Update standards if new patterns established
- [ ] Document new best practices discovered
- [ ] Update examples if format changed

**Project Documentation**:
- [ ] Update README.md if significant changes
- [ ] Update TODO.md:
  - [ ] Mark completed tasks as `- [x]` (change `- [ ]` to `- [x]`)
  - [ ] Add new tasks if needed (as `- [ ]`)
  - [ ] Update task descriptions if requirements changed
- [ ] Update PROJECT_STRUCTURE.md if structure changed

**Code Documentation**:
- [ ] All modified APIs have updated XML docs
- [ ] Complex refactoring is documented
- [ ] Performance optimizations are explained
- [ ] Non-obvious improvements are commented

## Code Review Checklist

- [ ] Follows coding standards from `standards/coding-standards.md`
- [ ] No breaking changes (unless explicitly intended)
- [ ] Backward compatible where applicable
- [ ] Well documented (code comments and external docs)
- [ ] Properly tested (comprehensive test coverage)
- [ ] Performance acceptable or improved
- [ ] Error handling improved
- [ ] Accessibility maintained or improved
- [ ] Full .NET compatibility (all features work - no restrictions)
- [ ] Memory/resource cleanup correct
- [ ] Visual consistency maintained
- [ ] User experience improved

## Update TODO.md (Before Committing)

- [ ] Open `TODO.md`
- [ ] Find any tasks that were completed
- [ ] Change `- [ ]` to `- [x]` for completed tasks
- [ ] Add any new polish tasks discovered (as `- [ ]`)
- [ ] Update task descriptions if needed
- [ ] Save TODO.md

## Commit & Push

```bash
git add .
git commit -m "polish: [Area/Component] improvements

- Improvement 1 (specific and measurable)
- Improvement 2 (specific and measurable)
- Fix issue 1 (if applicable)
- Performance: [if performance improvements]
- UI: [if UI improvements]
- Docs: [if documentation updated]

Related to: [Issue/Task ID]
Phase: [X.Y]
Task: [X.Y.Z]"

git push origin main
```

- [ ] Commit message follows format from `standards/commit-messages.md`
- [ ] All related files committed (code, tests, docs, TODO.md)
- [ ] Changes pushed to GitHub
- [ ] Commit references TODO.md task (Phase: X.Y, Task: X.Y.Z)

## References & Resources

### Required Reading
- [Coding Standards](standards/coding-standards.md) - **MUST READ**
- [Architecture Documentation](docs/dev/architecture.md) - **MUST READ**
- [Multi-Framework Architecture](docs/dev/multi-framework-architecture.md) - **MUST READ** (for UI polish)
- [API Reference](docs/dev/api-reference.md) - For existing APIs
- [TODO List](TODO.md) - For project context and polish targets (includes 100+ Windows Control Panel items)

### Additional Resources
- [Getting Started Guide](docs/dev/getting-started.md)
- [Build System](docs/dev/build-system.md)
- [Commit Message Format](standards/commit-messages.md)

## Quality Standards

### Code Quality
- Clean, readable, maintainable
- Follows SOLID principles
- DRY (Don't Repeat Yourself)
- Comprehensive error handling

### Performance
- No regression, optimize where possible
- Startup: < 1 second
- UI: 60 FPS for animations
- Memory: Efficient, no leaks

### User Experience
- Intuitive and discoverable
- Fast and responsive
- Beautiful and modern
- Accessible to all users
- Consistent with design system

### Documentation
- **Always update documentation** - This is not optional
- Keep docs in sync with code
- Document patterns and decisions
- Include examples

## Notes

[Any additional context, considerations, or follow-up tasks]

**Remember**: Polish should make ClassicPanel faster, more intuitive, more beautiful, and more comprehensive. Every improvement should enhance the user experience and maintain the high quality standards.

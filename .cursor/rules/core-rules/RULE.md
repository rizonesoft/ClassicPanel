---
alwaysApply: true
---
## Project Overview
ClassicPanel is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11, built with .NET 10, C# 14, and Windows Forms.

## Critical Build Configuration

### ⚠️ IMPORTANT: NO Native AOT
- **DO NOT use Native AOT compilation** (`PublishAot`)
- ClassicPanel uses **ReadyToRun + Quick JIT** for faster startup (full .NET compatibility maintained)
- ReadyToRun pre-compiles code at build time, Quick JIT handles dynamic code at runtime
- Both are enabled in the project file and build scripts
- Build outputs go to `build/` directory (not `src/bin/`)
- Use standard `dotnet build` command
- Framework-dependent build with ReadyToRun + Quick JIT is used for distribution (requires .NET 10 runtime - installer can bundle .NET runtime)

### Build System
- Build scripts: `build/build.bat` (Windows) and `build/build.sh` (Linux/macOS)
- Output directories: 
  - Debug: `build/debug/ClassicPanel.exe` (~290 KB)
  - Release: `build/release/ClassicPanel.exe` (~2.6 MB, framework-dependent)
- Use `GenerateAssemblyInfo=false` when redirecting output paths to avoid duplicate assembly attributes

## Technology Stack
- **Framework**: .NET 10 (LTS)
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Target OS**: Windows 10 and Windows 11 (x64 only)
- **Deployment**: Framework-dependent executable with ReadyToRun + Quick JIT (requires .NET 10 runtime - installer can bundle .NET runtime; ReadyToRun pre-compiles code at build time, Quick JIT handles dynamic code at runtime for faster startup)

## Architecture Principles
- .NET runtime with ReadyToRun + Quick JIT (no Native AOT)
- ReadyToRun: Pre-compiles code at build time for instant execution
- Quick JIT: Fast compilation for dynamic/reflection code at runtime (Tier 0), then recompiles hot paths with full optimization (Tier 1)
- Full WinForms compatibility (no trimming restrictions)
- All .NET features available (reflection, dynamic types, etc.)
- Multi-framework architecture support (WinForms default, WPF optional, C++ extensions)
- Embedded resources: SVG icons use format `ClassicPanel.Resources.Actions.{filename}.svg`

## Code Standards
- Follow `standards/coding-standards.md`
- Update documentation when making changes
- Always update `TODO.md` when completing tasks
- Use XML documentation for public APIs
- Comprehensive error handling required

## Documentation
- Developer docs: `docs/dev/`
- User docs: `docs/user/`
- Standards: `standards/`
- Always keep documentation in sync with code

## Build Commands
```bash
# Standard build (outputs to build/release or build/debug)
.\build\build.bat Release
.\build\build.bat Debug

# Or manually:
cd src
dotnet build -c Release -p:GenerateAssemblyInfo=false
dotnet build -c Debug -p:GenerateAssemblyInfo=false

# Run builds
.\build\debug\ClassicPanel.exe
.\build\release\ClassicPanel.exe
```

## Testing & Verification Workflow

**Optimized Workflow: Build for compilation, test the published build**

**After each task/change (fast compilation check):**
```bash
cd src
# Quick compilation verification (fast, catches errors immediately)
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

**After completing a set of tasks (before commit):**
```bash
cd src
# Build Debug and Release (compilation check)
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Test Debug build (for development testing)
.\build\debug\ClassicPanel.exe
```

**Rationale:**
- Building Debug/Release catches compilation errors quickly (~1-3 seconds each)
- Debug build is faster to test during development
- Release build is what users get - test it before major releases
- Saves time while ensuring quality

See `docs/dev/build-test-workflow.md` for detailed workflow recommendations.

## Git Commit & Push Workflow

**CRITICAL**: After completing a set of tasks/changes, you MUST commit and push to GitHub. This is mandatory, not optional.

**Steps:**
1. Stage all changes: `git add .`
2. Commit with proper message following `standards/commit-messages.md`
3. Push to GitHub: `git push origin main`
4. If push fails (network error), retry the push command
5. Verify success: `git status` should show "Your branch is up to date with 'origin/main'"

**If push fails:**
- Retry the push command
- If it still fails, inform the user that the commit was successful but push failed
- The user can manually push later with: `git push origin main`

## Remember
- **ReadyToRun ENABLED** - Pre-compiles code at build time for instant execution (full .NET compatibility)
- **Quick JIT ENABLED** - Fast compilation for dynamic code at runtime, then recompiles hot paths with full optimization
- **NO Native AOT** - ReadyToRun + Quick JIT enabled for faster startup (full .NET compatibility, no trimming restrictions)
- Build outputs to `build/` directory (subdirectories: `debug/net10.0-windows/win-x64/`, `release/`)
- Release executable is framework-dependent (~2.6 MB) - requires .NET 10 runtime (installer can bundle .NET runtime)
- Test Debug build during development, test Release build before major releases
- Full .NET feature support (reflection, dynamic types, etc. all work)
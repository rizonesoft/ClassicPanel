---
alwaysApply: true
---
## Project Overview
ClassicPanel is a comprehensive, fast, intuitive, and beautiful Control Panel replacement for Windows 10 and Windows 11, built with .NET 10, C# 14, and Windows Forms.

## Critical Build Configuration

### ⚠️ IMPORTANT: NO Native AOT
- **DO NOT use Native AOT compilation** (`PublishAot`)
- ClassicPanel uses **ReadyToRun** for faster startup (full .NET compatibility maintained)
- ReadyToRun is enabled in the project file and build scripts
- Build outputs go to `build/` directory (not `src/bin/`)
- Use standard `dotnet build` and `dotnet publish` commands
- Self-contained single-file publish with ReadyToRun is used for distribution (includes .NET runtime - end users do NOT need to install .NET separately)

### Build System
- Build scripts: `build/build.bat` (Windows) and `build/build.sh` (Linux/macOS)
- Output directories: 
  - Debug: `build/debug/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
  - Release: `build/release/net10.0-windows/win-x64/ClassicPanel.exe` (~290 KB)
  - Published: `build/publish/ClassicPanel.exe` (~122 MB, self-contained)
- Use `GenerateAssemblyInfo=false` when redirecting output paths to avoid duplicate assembly attributes

## Technology Stack
- **Framework**: .NET 10 (LTS)
- **Language**: C# 14
- **UI Framework**: Windows Forms (WinForms)
- **Target OS**: Windows 10 and Windows 11 (x64 only)
- **Deployment**: Self-contained single-file executable with ReadyToRun (includes .NET runtime bundled inside - no separate .NET installation required for end users; ReadyToRun pre-compiles code for faster startup)

## Architecture Principles
- .NET runtime with ReadyToRun (no Native AOT)
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

# Publish (self-contained single-file with ReadyToRun)
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Run builds
.\build\debug\net10.0-windows\win-x64\ClassicPanel.exe
.\build\release\net10.0-windows\win-x64\ClassicPanel.exe
.\build\publish\ClassicPanel.exe
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
# Publish self-contained executable (what users get)
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Test Published build (CRITICAL - this is what users get)
.\build\publish\ClassicPanel.exe
```

**Rationale:**
- Building Debug/Release catches compilation errors quickly (~1-3 seconds each)
- If compilation succeeds, Debug/Release will run correctly (no need to test them)
- Published build is what users get - focus testing here
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
- **ReadyToRun ENABLED** - Pre-compiles code for faster startup (full .NET compatibility)
- **NO Native AOT** - ReadyToRun enabled for faster startup (full .NET compatibility, no trimming restrictions)
- Build outputs to `build/` directory (subdirectories: `debug/net10.0-windows/win-x64/`, `release/net10.0-windows/win-x64/`, `publish/`)
- Published executable is self-contained (~122 MB) - includes .NET runtime
- Always test published build - this is what end users receive
- Full .NET feature support (reflection, dynamic types, etc. all work)
# Build & Test Workflow

## Recommended Workflow

### After Each Task/Change (Fast Iteration)

**Build for compilation verification only:**
```bash
cd src

# Quick compilation check (fast, catches errors immediately)
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false
```

**Why:**
- ✅ Fast (~1-3 seconds each)
- ✅ Catches compilation errors immediately
- ✅ Ensures code compiles in both configurations
- ✅ No need to run executables (compilation is the check)

### After Completing a Set of Tasks (Before Commit)

**Build Debug and Release, test Debug build:**
```bash
cd src

# Build Debug and Release (compilation check)
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Test Debug build (for development testing)
.\build\debug\ClassicPanel.exe
```

**Why:**
- ✅ Debug build is faster to test during development
- ✅ Catches runtime issues quickly
- ✅ Release build should be tested before major releases

### Periodic Full Build (Optional - Weekly/Daily)

**Full build and test all configurations:**
```bash
cd src

# Build all configurations
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Build Debug and Release
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Test Debug build (development)
.\build\debug\ClassicPanel.exe

# Test Release build (before major releases)
.\build\release\ClassicPanel.exe
```

**When to use:**
- Before major releases
- Weekly/daily comprehensive check
- When debugging configuration-specific issues
- Before creating distribution packages

## Rationale

### Why Build Debug/Release But Not Test Them?

1. **Compilation is the check**: If Debug and Release compile successfully, they will run correctly. The compilation step catches:
   - Syntax errors
   - Type mismatches
   - Missing references
   - Configuration-specific build issues

2. **Release build is what users get**: The Release build has unique characteristics:
   - Framework-dependent (requires .NET runtime)
   - ReadyToRun pre-compiled
   - Outputs directly to `build/release/`
   - This is what users actually get

3. **Time efficiency**: 
   - Building: ~1-3 seconds per configuration
   - Running and testing: ~30-60 seconds per build
   - Focus testing time on what matters (published build)

### Why Not Skip Debug/Release Builds Entirely?

**Risk**: If you only build when publishing, you might:
- Accumulate compilation errors across multiple tasks
- Have harder-to-debug issues (which task broke it?)
- Waste time on publish (slower) when simple compilation would catch it

**Solution**: Quick compilation checks catch errors early without the overhead of running executables.

## Build Script Integration

The build scripts (`build/build.bat` and `build/build.sh`) can be used:

```bash
# Quick compilation check
.\build\build.bat Debug   # Just builds, doesn't publish
.\build\build.bat Release # Builds and publishes (for final check)

# Or use the scripts with --publish flag for explicit control
.\build\build.bat Release --publish
```

## CI/CD Integration

For continuous integration, always build and test all configurations:

```yaml
# GitHub Actions example
- name: Build Debug
  run: dotnet build -c Debug

- name: Build Release  
  run: dotnet build -c Release

- name: Build Debug and Release
  run: |
    dotnet build -c Debug -p:GenerateAssemblyInfo=false
    dotnet build -c Release -p:GenerateAssemblyInfo=false

- name: Test Debug Build
  run: .\build\debug\ClassicPanel.exe --test-mode
```

## Summary

**Daily workflow:**
1. Build Debug/Release (compilation check) - Fast
2. Work on tasks
3. Before commit: Build Debug/Release and test Debug build - Comprehensive

**Periodic (weekly/daily):**
- Full build and test all configurations - Thorough validation

This approach balances speed with thoroughness, ensuring:
- ✅ Fast iteration (compilation checks)
- ✅ Quick development testing (Debug build)
- ✅ Periodic Release build testing (before major releases)
- ✅ No accumulation of broken builds


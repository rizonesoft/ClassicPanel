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

**Publish and test the published build:**
```bash
cd src

# Publish self-contained executable (what users get)
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Test the published build (CRITICAL - this is what users get)
.\build\publish\ClassicPanel.exe
```

**Why:**
- ✅ Tests the actual user experience
- ✅ Validates self-contained deployment works
- ✅ Catches runtime issues specific to published builds
- ✅ Most important - this is what gets distributed

### Periodic Full Build (Optional - Weekly/Daily)

**Full build and test all configurations:**
```bash
cd src

# Build all configurations
dotnet build -c Debug -p:GenerateAssemblyInfo=false
dotnet build -c Release -p:GenerateAssemblyInfo=false

# Publish
dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64 -p:GenerateAssemblyInfo=false

# Test all builds (comprehensive check)
.\build\debug\net10.0-windows\win-x64\ClassicPanel.exe
.\build\release\net10.0-windows\win-x64\ClassicPanel.exe
.\build\publish\ClassicPanel.exe
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

2. **Published build is different**: The published build has unique characteristics:
   - Self-contained (includes .NET runtime)
   - ReadyToRun pre-compiled
   - Single-file extraction behavior
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

- name: Publish
  run: dotnet publish -c Release -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:SelfContained=true -p:RuntimeIdentifier=win-x64

- name: Test Published Build
  run: .\build\publish\ClassicPanel.exe --test-mode
```

## Summary

**Daily workflow:**
1. Build Debug/Release (compilation check) - Fast
2. Work on tasks
3. Before commit: Publish and test published build - Comprehensive

**Periodic (weekly/daily):**
- Full build and test all configurations - Thorough validation

This approach balances speed with thoroughness, ensuring:
- ✅ Fast iteration (compilation checks)
- ✅ User-focused testing (published build)
- ✅ Periodic comprehensive validation (full builds)
- ✅ No accumulation of broken builds


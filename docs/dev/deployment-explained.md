# Deployment Explained: Self-Contained vs ReadyToRun

## Overview

This document clarifies the difference between **self-contained deployment** and **ReadyToRun** compilation, as they are often confused.

## Self-Contained Deployment

**What it is:**
- Bundles the .NET runtime **inside** your executable or deployment folder
- End users do **NOT** need to install .NET separately
- Makes your application portable and standalone

**How it works:**
- When you publish with `SelfContained=true`, .NET includes all runtime files
- With `PublishSingleFile=true`, everything is bundled into a single `.exe` file
- The runtime is extracted at runtime (or embedded, depending on configuration)

**ClassicPanel Configuration:**
```xml
SelfContained=true
PublishSingleFile=true
RuntimeIdentifier=win-x64
```

**Result:**
- Single executable (~110 MB) that includes the entire .NET runtime
- Users can run it on any Windows 10/11 64-bit system without installing .NET

## ReadyToRun (R2R) Compilation

**What it is:**
- An **optional optimization** that pre-compiles code to native format
- Improves startup performance by reducing JIT compilation overhead
- **NOT required** for self-contained deployment to work

**How it works:**
- Code is pre-compiled to native machine code at build time
- At runtime, pre-compiled code runs directly (no JIT needed for those methods)
- Falls back to JIT compilation for methods not pre-compiled

**ClassicPanel uses ReadyToRun:**
- Pre-compiled code reduces JIT compilation overhead at startup
- Code is pre-compiled to native format at build time
- Faster startup performance with full .NET feature support

## Key Differences

| Feature | Self-Contained | ReadyToRun |
|---------|---------------|------------|
| **Purpose** | Bundle .NET runtime | Pre-compile for performance |
| **Required for standalone?** | ✅ Yes | ❌ No |
| **Users need .NET installed?** | ❌ No | N/A |
| **Affects startup speed?** | No | ✅ Yes (faster) |
| **Affects file size?** | ✅ Yes (larger, ~110 MB) | Slightly |
| **Restrictions?** | None | Minimal |
| **We use it?** | ✅ Yes | ❌ No |

## Why ClassicPanel Uses ReadyToRun

1. **Faster Startup** - Pre-compiled code reduces JIT overhead on first launch
2. **Better User Experience** - Users notice faster application startup
3. **Full .NET Compatibility** - ReadyToRun maintains all features (reflection, dynamic types, etc.)
4. **Minimal Trade-offs** - Only slightly larger file size (~5-10% increase), already acceptable at ~111 MB
5. **Best Practice** - Recommended optimization for desktop applications

## Summary

✅ **Self-Contained = Runtime Included (users don't need .NET installed)**
- This is what ClassicPanel uses
- Makes the app standalone and portable

✅ **ReadyToRun = Performance Optimization (ENABLED)**
- ClassicPanel uses this for faster startup
- Pre-compiles code to native format at build time
- Reduces JIT compilation overhead at runtime
- Still maintains full .NET compatibility

## Visual Comparison

### Framework-Dependent Deployment
```
Application.exe (small, ~1-5 MB)
+ .NET Runtime (must be installed separately on user's system)
= Requires .NET installation ❌
```


### Self-Contained + ReadyToRun (ClassicPanel Configuration)
```
Application.exe (large, ~115-120 MB, includes .NET runtime)
+ Pre-compiled native code (ReadyToRun)
= No .NET installation required ✅
= Faster startup (pre-compiled code runs immediately)
```

## Performance Considerations

**Is self-contained faster than framework-dependent?**

With ReadyToRun enabled, **self-contained is actually FASTER** because:
- Pre-compiled code eliminates JIT compilation overhead
- Consistent performance across all systems (not dependent on .NET being in memory)
- No time spent locating/verifying system .NET installation

See [Performance Comparison](performance-comparison.md) for detailed analysis.

## References

- [.NET Self-Contained Deployment](https://learn.microsoft.com/dotnet/core/deploying/#self-contained-deployments)
- [ReadyToRun Compilation](https://learn.microsoft.com/dotnet/core/deploying/ready-to-run)
- [Performance Comparison](performance-comparison.md) - Detailed performance analysis


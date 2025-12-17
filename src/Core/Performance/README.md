# Performance Monitoring Infrastructure

This folder contains the performance monitoring infrastructure for ClassicPanel.

## Overview

The performance monitoring system tracks:
- **Startup Time**: Application startup duration
- **Memory Usage**: Current and peak memory consumption
- **Operation Timings**: Individual operation performance
- **Slow Operation Detection**: Automatic detection of operations exceeding thresholds

## Components

### PerformanceMonitor

Main performance monitoring class that:
- Tracks startup time
- Monitors memory usage
- Records operation timings
- Detects slow operations
- Provides metrics summary

**Usage:**
```csharp
// Initialize at application startup
PerformanceMonitor.Initialize();

// Mark startup complete
PerformanceMonitor.MarkStartupComplete();

// Get current metrics
var metrics = PerformanceMonitor.GetMetrics();
Console.WriteLine($"Startup: {metrics.StartupTimeMs}ms");
Console.WriteLine($"Memory: {metrics.MemoryUsageMB:F2} MB");

// Get formatted summary
string summary = PerformanceMonitor.GetMetricsSummary();

// Enable/disable monitoring
PerformanceMonitor.IsEnabled = false; // Disable for production if needed
```

### OperationTimer

Convenient timer for measuring operation performance.

**Usage:**
```csharp
// Using statement (automatic recording)
using (OperationTimer.Start("LoadApplets"))
{
    // Your operation code here
    LoadApplets();
}
// Timer automatically records to PerformanceMonitor

// Manual timing
using (var timer = new OperationTimer("CustomOperation"))
{
    // Your operation code
    DoWork();
}
// Timer stops and records on Dispose

// With custom callback
var timer = new OperationTimer("MyOperation", (name, elapsedMs) => {
    Console.WriteLine($"{name} took {elapsedMs}ms");
});
// ... do work ...
timer.Stop();
```

### PerformanceMetrics

Data structure representing performance metrics snapshot.

**Properties:**
- `StartupTimeMs` - Application startup time
- `MemoryUsageBytes` / `MemoryUsageMB` - Current memory usage
- `PeakMemoryUsageBytes` / `PeakMemoryUsageMB` - Peak memory usage
- `CpuUsagePercent` - CPU usage (placeholder, to be implemented)
- `OperationCount` - Number of operations tracked
- `AverageOperationTimeMs` - Average operation time
- `SlowestOperationTimeMs` - Slowest operation time
- `Timestamp` - When metrics were collected

## Performance Targets

- **Startup Time**: < 1000ms (1 second)
- **Memory Usage**: Efficient, no leaks
- **UI Responsiveness**: 60 FPS for animations
- **Operation Threshold**: Operations > 100ms trigger slow operation warnings

## Slow Operation Detection

Operations exceeding 100ms automatically trigger the `OnSlowOperation` event:

```csharp
PerformanceMonitor.OnSlowOperation += (operationName, elapsedMs) => {
    // Log or handle slow operations
    Debug.WriteLine($"Slow operation detected: {operationName} took {elapsedMs}ms");
};
```

## Memory Monitoring

Memory usage is tracked with throttling to avoid performance impact:
- Memory checks are limited to once per second
- Peak memory is automatically tracked
- GC is forced before measurement for accuracy

## Future Enhancements

- CPU usage monitoring (Windows Performance Counters)
- Operation history with timestamps
- Performance metrics export (JSON/CSV)
- Performance dashboard UI (dev mode)
- Memory leak detection
- Performance benchmarks
- Operation profiling

## Best Practices

1. **Initialize Early**: Call `PerformanceMonitor.Initialize()` at application startup
2. **Mark Startup Complete**: Call `MarkStartupComplete()` when UI is ready
3. **Use OperationTimer**: Wrap important operations with `OperationTimer.Start()`
4. **Monitor Memory**: Periodically call `UpdateMemoryUsage()` or use `GetMetrics()`
5. **Handle Slow Operations**: Subscribe to `OnSlowOperation` event for logging/alerts
6. **Disable in Production**: Set `IsEnabled = false` if monitoring not needed

## Example Integration

```csharp
// In Program.cs
static void Main()
{
    PerformanceMonitor.Initialize();
    
    // ... application initialization ...
    
    Application.Run(new MainWindow());
    
    PerformanceMonitor.MarkStartupComplete();
}

// In CplLoader.cs
public void LoadApplets()
{
    using (OperationTimer.Start("LoadApplets"))
    {
        // Load applets code
    }
}
```


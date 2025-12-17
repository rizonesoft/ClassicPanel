using System.Diagnostics;

namespace ClassicPanel.Core.Performance;

/// <summary>
/// Monitors and tracks application performance metrics including startup time, memory usage, CPU usage, and operation timings.
/// </summary>
public static class PerformanceMonitor
{
    private static readonly Stopwatch _startupTimer = Stopwatch.StartNew();
    private static readonly List<long> _operationTimes = new();
    private static long _startupTimeMs;
    private static bool _isEnabled = true;
    private static long _peakMemoryBytes;
    private static DateTime _lastMemoryCheck = DateTime.MinValue;
    private static readonly TimeSpan _memoryCheckInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets whether performance monitoring is enabled.
    /// </summary>
    public static bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    /// <summary>
    /// Gets the application startup time in milliseconds.
    /// </summary>
    public static long StartupTimeMs => _startupTimeMs;

    /// <summary>
    /// Gets the current memory usage in bytes.
    /// </summary>
    public static long CurrentMemoryBytes
    {
        get
        {
            if (!_isEnabled)
                return 0;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            return GC.GetTotalMemory(false);
        }
    }

    /// <summary>
    /// Gets the peak memory usage in bytes.
    /// </summary>
    public static long PeakMemoryBytes => _peakMemoryBytes;

    /// <summary>
    /// Gets the number of operations tracked.
    /// </summary>
    public static int OperationCount => _operationTimes.Count;

    /// <summary>
    /// Gets the average operation time in milliseconds.
    /// </summary>
    public static double AverageOperationTimeMs
    {
        get
        {
            if (_operationTimes.Count == 0)
                return 0;

            return _operationTimes.Average();
        }
    }

    /// <summary>
    /// Gets the slowest operation time in milliseconds.
    /// </summary>
    public static long SlowestOperationTimeMs => _operationTimes.Count > 0 ? _operationTimes.Max() : 0;

    /// <summary>
    /// Event raised when a slow operation is detected.
    /// </summary>
    public static event Action<string, long>? OnSlowOperation;

    /// <summary>
    /// Initializes the performance monitor.
    /// Should be called at application startup.
    /// </summary>
    public static void Initialize()
    {
        if (!_isEnabled)
            return;

        _startupTimer.Restart();
        _operationTimes.Clear();
        _peakMemoryBytes = 0;
        _lastMemoryCheck = DateTime.MinValue;
    }

    /// <summary>
    /// Marks the application as fully started and records startup time.
    /// </summary>
    public static void MarkStartupComplete()
    {
        if (!_isEnabled)
            return;

        _startupTimer.Stop();
        _startupTimeMs = _startupTimer.ElapsedMilliseconds;

        // Check if startup time exceeds target
        if (_startupTimeMs > AppConstants.TargetStartupTimeMs)
        {
            OnSlowOperation?.Invoke("Application Startup", _startupTimeMs);
        }

        // Update initial memory usage
        UpdateMemoryUsage();
    }

    /// <summary>
    /// Records an operation time.
    /// </summary>
    /// <param name="operationName">The name of the operation.</param>
    /// <param name="elapsedMs">The elapsed time in milliseconds.</param>
    public static void RecordOperation(string operationName, long elapsedMs)
    {
        if (!_isEnabled)
            return;

        _operationTimes.Add(elapsedMs);

        // Check for slow operations (threshold: 100ms)
        const long slowOperationThreshold = 100;
        if (elapsedMs > slowOperationThreshold)
        {
            OnSlowOperation?.Invoke(operationName, elapsedMs);
        }

        // Limit operation history to prevent memory growth
        const int maxOperations = 1000;
        if (_operationTimes.Count > maxOperations)
        {
            _operationTimes.RemoveAt(0);
        }
    }

    /// <summary>
    /// Updates memory usage tracking.
    /// </summary>
    public static void UpdateMemoryUsage()
    {
        if (!_isEnabled)
            return;

        // Throttle memory checks to avoid performance impact
        var now = DateTime.UtcNow;
        if (now - _lastMemoryCheck < _memoryCheckInterval)
            return;

        _lastMemoryCheck = now;

        var currentMemory = CurrentMemoryBytes;
        if (currentMemory > _peakMemoryBytes)
        {
            _peakMemoryBytes = currentMemory;
        }
    }

    /// <summary>
    /// Gets current performance metrics.
    /// </summary>
    /// <returns>A PerformanceMetrics instance with current values.</returns>
    public static PerformanceMetrics GetMetrics()
    {
        UpdateMemoryUsage();

        return new PerformanceMetrics
        {
            StartupTimeMs = _startupTimeMs,
            MemoryUsageBytes = CurrentMemoryBytes,
            PeakMemoryUsageBytes = _peakMemoryBytes,
            CpuUsagePercent = GetCpuUsage(), // Placeholder - CPU monitoring can be added later
            OperationCount = _operationTimes.Count,
            AverageOperationTimeMs = AverageOperationTimeMs,
            SlowestOperationTimeMs = SlowestOperationTimeMs,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Gets CPU usage percentage.
    /// This is a placeholder implementation - full CPU monitoring requires additional Windows API calls.
    /// </summary>
    /// <returns>CPU usage percentage (0-100), or 0 if not available.</returns>
    private static double GetCpuUsage()
    {
        // TODO: Implement CPU usage monitoring using Windows Performance Counters or WMI
        // For now, return 0 as placeholder
        return 0.0;
    }

    /// <summary>
    /// Resets all performance metrics.
    /// </summary>
    public static void Reset()
    {
        _operationTimes.Clear();
        _peakMemoryBytes = 0;
        _startupTimeMs = 0;
        _startupTimer.Restart();
    }

    /// <summary>
    /// Gets a formatted string representation of current metrics.
    /// </summary>
    /// <returns>A formatted string with performance metrics.</returns>
    public static string GetMetricsSummary()
    {
        var metrics = GetMetrics();
        return $"Startup: {metrics.StartupTimeMs}ms | " +
               $"Memory: {metrics.MemoryUsageMB:F2} MB (Peak: {metrics.PeakMemoryUsageMB:F2} MB) | " +
               $"Operations: {metrics.OperationCount} | " +
               $"Avg Operation: {metrics.AverageOperationTimeMs:F2}ms | " +
               $"Slowest: {metrics.SlowestOperationTimeMs}ms";
    }
}


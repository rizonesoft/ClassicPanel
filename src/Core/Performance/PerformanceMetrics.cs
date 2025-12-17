namespace ClassicPanel.Core.Performance;

/// <summary>
/// Represents performance metrics collected by the performance monitoring system.
/// </summary>
public class PerformanceMetrics
{
    /// <summary>
    /// Gets or sets the application startup time in milliseconds.
    /// </summary>
    public long StartupTimeMs { get; set; }

    /// <summary>
    /// Gets or sets the current memory usage in bytes.
    /// </summary>
    public long MemoryUsageBytes { get; set; }

    /// <summary>
    /// Gets or sets the peak memory usage in bytes.
    /// </summary>
    public long PeakMemoryUsageBytes { get; set; }

    /// <summary>
    /// Gets or sets the current CPU usage percentage (0-100).
    /// </summary>
    public double CpuUsagePercent { get; set; }

    /// <summary>
    /// Gets or sets the number of operations tracked.
    /// </summary>
    public int OperationCount { get; set; }

    /// <summary>
    /// Gets or sets the average operation time in milliseconds.
    /// </summary>
    public double AverageOperationTimeMs { get; set; }

    /// <summary>
    /// Gets or sets the slowest operation time in milliseconds.
    /// </summary>
    public long SlowestOperationTimeMs { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when metrics were collected.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets the memory usage in megabytes.
    /// </summary>
    public double MemoryUsageMB => MemoryUsageBytes / (1024.0 * 1024.0);

    /// <summary>
    /// Gets the peak memory usage in megabytes.
    /// </summary>
    public double PeakMemoryUsageMB => PeakMemoryUsageBytes / (1024.0 * 1024.0);

    /// <summary>
    /// Creates a snapshot of current performance metrics.
    /// </summary>
    /// <returns>A new PerformanceMetrics instance with current values.</returns>
    public PerformanceMetrics CreateSnapshot()
    {
        return new PerformanceMetrics
        {
            StartupTimeMs = StartupTimeMs,
            MemoryUsageBytes = MemoryUsageBytes,
            PeakMemoryUsageBytes = PeakMemoryUsageBytes,
            CpuUsagePercent = CpuUsagePercent,
            OperationCount = OperationCount,
            AverageOperationTimeMs = AverageOperationTimeMs,
            SlowestOperationTimeMs = SlowestOperationTimeMs,
            Timestamp = Timestamp
        };
    }
}


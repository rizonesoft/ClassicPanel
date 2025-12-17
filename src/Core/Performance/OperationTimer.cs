using System.Diagnostics;

namespace ClassicPanel.Core.Performance;

/// <summary>
/// Provides timing functionality for measuring operation performance.
/// Implements IDisposable for convenient using statement usage.
/// </summary>
public sealed class OperationTimer : IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly string _operationName;
    private readonly Action<string, long>? _onComplete;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the OperationTimer class.
    /// </summary>
    /// <param name="operationName">The name of the operation being timed.</param>
    /// <param name="onComplete">Optional callback invoked when the timer completes.</param>
    public OperationTimer(string operationName, Action<string, long>? onComplete = null)
    {
        _operationName = operationName ?? throw new ArgumentNullException(nameof(operationName));
        _onComplete = onComplete;
        _stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// Gets the elapsed time in milliseconds.
    /// </summary>
    public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

    /// <summary>
    /// Gets the elapsed time as a TimeSpan.
    /// </summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>
    /// Stops the timer and records the operation.
    /// </summary>
    public void Stop()
    {
        if (!_disposed)
        {
            _stopwatch.Stop();
            _onComplete?.Invoke(_operationName, _stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Disposes the timer, stopping it if not already stopped.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();
            _stopwatch.Stop();
            _disposed = true;
        }
    }

    /// <summary>
    /// Creates a timer that automatically records to PerformanceMonitor.
    /// </summary>
    /// <param name="operationName">The name of the operation being timed.</param>
    /// <returns>A new OperationTimer instance.</returns>
    public static OperationTimer Start(string operationName)
    {
        return new OperationTimer(operationName, PerformanceMonitor.RecordOperation);
    }
}


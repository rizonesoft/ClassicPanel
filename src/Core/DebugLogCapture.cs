using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace ClassicPanel.Core;

/// <summary>
/// Captures debug output from Debug.WriteLine and provides access to log entries.
/// Thread-safe and designed for real-time log viewing in debug tools.
/// </summary>
public static class DebugLogCapture
{
    private static readonly ConcurrentQueue<LogEntry> _logEntries = new();
    private static readonly object _lock = new();
    private static int _maxEntries = 10000;
    private static bool _isCapturing = false;
    private static TextWriterTraceListener? _traceListener;

    /// <summary>
    /// Event raised when a new log entry is captured.
    /// </summary>
    public static event Action<LogEntry>? OnLogEntry;

    /// <summary>
    /// Gets or sets the maximum number of log entries to keep in memory.
    /// </summary>
    public static int MaxEntries
    {
        get => _maxEntries;
        set
        {
            _maxEntries = Math.Max(100, value); // Minimum 100 entries
            TrimLogEntries();
        }
    }

    /// <summary>
    /// Gets all captured log entries.
    /// </summary>
    public static IReadOnlyList<LogEntry> LogEntries
    {
        get
        {
            lock (_lock)
            {
                return _logEntries.ToArray();
            }
        }
    }

    /// <summary>
    /// Gets the number of log entries currently captured.
    /// </summary>
    public static int EntryCount => _logEntries.Count;

    /// <summary>
    /// Gets a value indicating whether log capture is currently active.
    /// </summary>
    public static bool IsCapturing => _isCapturing;

    /// <summary>
    /// Starts capturing debug output.
    /// </summary>
    public static void StartCapture()
    {
        if (_isCapturing)
            return;

        _isCapturing = true;

        // Create a custom trace listener that captures output
        _traceListener = new TextWriterTraceListener(new DebugLogWriter());
        Trace.Listeners.Add(_traceListener);
    }

    /// <summary>
    /// Stops capturing debug output.
    /// </summary>
    public static void StopCapture()
    {
        if (!_isCapturing)
            return;

        _isCapturing = false;

        if (_traceListener != null)
        {
            Trace.Listeners.Remove(_traceListener);
            _traceListener.Dispose();
            _traceListener = null;
        }
    }

    /// <summary>
    /// Adds a log entry manually (for testing or custom logging).
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="level">The log level.</param>
    public static void AddLogEntry(string message, LogLevel level = LogLevel.Info)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Message = message ?? string.Empty,
            Level = level
        };

        AddEntry(entry);
    }

    /// <summary>
    /// Clears all captured log entries.
    /// </summary>
    public static void Clear()
    {
        lock (_lock)
        {
            while (_logEntries.TryDequeue(out _))
            {
                // Dequeue all entries
            }
        }
    }

    /// <summary>
    /// Gets log entries filtered by level.
    /// </summary>
    /// <param name="level">The log level to filter by.</param>
    /// <returns>Filtered log entries.</returns>
    public static IReadOnlyList<LogEntry> GetEntriesByLevel(LogLevel level)
    {
        lock (_lock)
        {
            return _logEntries.Where(e => e.Level == level).ToArray();
        }
    }

    /// <summary>
    /// Gets log entries within a time range.
    /// </summary>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>Filtered log entries.</returns>
    public static IReadOnlyList<LogEntry> GetEntriesByTimeRange(DateTime startTime, DateTime endTime)
    {
        lock (_lock)
        {
            return _logEntries.Where(e => e.Timestamp >= startTime && e.Timestamp <= endTime).ToArray();
        }
    }

    /// <summary>
    /// Adds a log entry to the queue.
    /// </summary>
    private static void AddEntry(LogEntry entry)
    {
        lock (_lock)
        {
            _logEntries.Enqueue(entry);
            TrimLogEntries();
        }

        OnLogEntry?.Invoke(entry);
    }

    /// <summary>
    /// Trims log entries to stay within MaxEntries limit.
    /// </summary>
    private static void TrimLogEntries()
    {
        while (_logEntries.Count > _maxEntries)
        {
            _logEntries.TryDequeue(out _);
        }
    }

    /// <summary>
    /// Custom TextWriter that captures debug output.
    /// </summary>
    private class DebugLogWriter : TextWriter
    {
        private readonly StringBuilder _buffer = new();

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            if (_isCapturing)
            {
                if (value == '\n' || value == '\r')
                {
                    if (_buffer.Length > 0)
                    {
                        var entry = ParseLogEntry(_buffer.ToString());
                        AddEntry(entry);
                        _buffer.Clear();
                    }
                }
                else if (value != '\0')
                {
                    _buffer.Append(value);
                }
            }
        }

        public override void Write(string? value)
        {
            if (value != null && _isCapturing)
            {
                _buffer.Append(value);
            }
        }

        public override void WriteLine(string? value)
        {
            if (value != null)
            {
                Write(value);
            }
            Write('\n');
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _buffer.Length > 0)
            {
                var entry = ParseLogEntry(_buffer.ToString());
                AddEntry(entry);
                _buffer.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Parses a log entry from Debug.WriteLine output.
        /// </summary>
        private LogEntry ParseLogEntry(string value)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = value,
                Level = LogLevel.Info
            };

            // Try to parse ErrorInfo format: [timestamp] [severity] message
            if (value.StartsWith("[") && value.Contains("]"))
            {
                var firstBracketEnd = value.IndexOf(']');
                if (firstBracketEnd > 0)
                {
                    var timestampStr = value.Substring(1, firstBracketEnd - 1);
                    if (DateTime.TryParse(timestampStr, out var timestamp))
                    {
                        entry.Timestamp = timestamp;
                        value = value.Substring(firstBracketEnd + 1).TrimStart();
                    }
                }

                // Check for severity level
                if (value.StartsWith("[") && value.Contains("]"))
                {
                    var secondBracketEnd = value.IndexOf(']');
                    if (secondBracketEnd > 0)
                    {
                        var severityStr = value.Substring(1, secondBracketEnd - 1).Trim();
                        entry.Level = severityStr switch
                        {
                            "Information" or "Info" => LogLevel.Info,
                            "Warning" or "Warn" => LogLevel.Warning,
                            "Error" => LogLevel.Error,
                            "Critical" => LogLevel.Critical,
                            _ => LogLevel.Info
                        };
                        entry.Message = value.Substring(secondBracketEnd + 1).TrimStart();
                    }
                }
            }

            return entry;
        }
    }
}

/// <summary>
/// Represents a single log entry.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets or sets the timestamp when the log entry was created.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the log message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the log level.
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// Gets a formatted string representation of the log entry.
    /// </summary>
    public string FormattedMessage => $"[{Timestamp:HH:mm:ss.fff}] [{Level}] {Message}";
}

/// <summary>
/// Defines log levels for debug output.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Informational message.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning message.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error message.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Critical error message.
    /// </summary>
    Critical = 3
}


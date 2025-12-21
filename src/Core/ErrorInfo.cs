using System.Diagnostics;

namespace ClassicPanel.Core;

/// <summary>
/// Represents error information for logging and user-facing error messages.
/// Provides structured error data with support for exceptions, context, and severity levels.
/// </summary>
public class ErrorInfo
{
    #region Properties

    /// <summary>
    /// Gets or sets the error message (user-friendly description).
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the detailed error message (for debugging/logging).
    /// </summary>
    public string? DetailedMessage { get; set; }

    /// <summary>
    /// Gets or sets the exception that caused the error (if any).
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets or sets the error severity level.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the context where the error occurred (e.g., "CplLoader.LoadFile", "UI.MainWindow.Initialize").
    /// </summary>
    public string? Context { get; set; }

    /// <summary>
    /// Gets or sets additional context data (key-value pairs for debugging).
    /// </summary>
    public Dictionary<string, string>? AdditionalData { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this error should be shown to the user.
    /// </summary>
    public bool ShowToUser { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
    /// </summary>
    public ErrorInfo()
    {
        Message = string.Empty;
        Severity = ErrorSeverity.Error;
        Timestamp = DateTime.Now;
        ShowToUser = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorInfo"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ErrorInfo(string message)
        : this()
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorInfo"/> class with a message and exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception that caused the error.</param>
    public ErrorInfo(string message, Exception exception)
        : this(message)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        DetailedMessage = exception.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorInfo"/> class with a message, exception, and context.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception that caused the error.</param>
    /// <param name="context">The context where the error occurred.</param>
    public ErrorInfo(string message, Exception exception, string context)
        : this(message, exception)
    {
        Context = context;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates an <see cref="ErrorInfo"/> from an exception.
    /// </summary>
    /// <param name="exception">The exception to create error info from.</param>
    /// <param name="userMessage">Optional user-friendly message. If null, a generic message is used.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    /// <returns>An <see cref="ErrorInfo"/> instance.</returns>
    public static ErrorInfo FromException(Exception exception, string? userMessage = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new ErrorInfo(
            userMessage ?? AppConstants.GenericErrorMessage,
            exception,
            context ?? string.Empty
        );
    }

    /// <summary>
    /// Creates an <see cref="ErrorInfo"/> for a file operation error.
    /// </summary>
    /// <param name="filePath">The path to the file that caused the error.</param>
    /// <param name="operation">The operation that failed (e.g., "Load", "Save", "Delete").</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An <see cref="ErrorInfo"/> instance.</returns>
    public static ErrorInfo FromFileOperation(string filePath, string operation, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = new ErrorInfo(
            $"Failed to {operation.ToLowerInvariant()} file: {Path.GetFileName(filePath)}",
            exception,
            $"FileOperation.{operation}"
        );

        errorInfo.AdditionalData = new Dictionary<string, string>
        {
            { "FilePath", filePath },
            { "Operation", operation }
        };

        return errorInfo;
    }

    /// <summary>
    /// Creates an <see cref="ErrorInfo"/> for a CPL loading error.
    /// </summary>
    /// <param name="cplPath">The path to the CPL file that failed to load.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An <see cref="ErrorInfo"/> instance.</returns>
    public static ErrorInfo FromCplLoadError(string cplPath, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(cplPath);
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = new ErrorInfo(
            AppConstants.CplLoadErrorMessage,
            exception,
            "CplLoader.LoadFile"
        );

        errorInfo.AdditionalData = new Dictionary<string, string>
        {
            { "CplPath", cplPath }
        };

        return errorInfo;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Logs the error to debug output.
    /// </summary>
    public void LogError()
    {
        var logMessage = FormatLogMessage();
        Debug.WriteLine(logMessage);
    }

    /// <summary>
    /// Formats the error information for logging.
    /// </summary>
    /// <returns>A formatted log message.</returns>
    public string FormatLogMessage()
    {
        var builder = new System.Text.StringBuilder();
        builder.AppendLine($"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Severity}] {Message}");

        if (!string.IsNullOrEmpty(Context))
        {
            builder.AppendLine($"  Context: {Context}");
        }

        if (Exception != null)
        {
            builder.AppendLine($"  Exception Type: {Exception.GetType().Name}");
            builder.AppendLine($"  Exception Message: {Exception.Message}");

            if (!string.IsNullOrEmpty(Exception.StackTrace))
            {
                builder.AppendLine($"  Stack Trace: {Exception.StackTrace}");
            }

            if (Exception.InnerException != null)
            {
                builder.AppendLine($"  Inner Exception: {Exception.InnerException.GetType().Name} - {Exception.InnerException.Message}");
            }
        }

        if (!string.IsNullOrEmpty(DetailedMessage))
        {
            builder.AppendLine($"  Details: {DetailedMessage}");
        }

        if (AdditionalData != null && AdditionalData.Count > 0)
        {
            builder.AppendLine("  Additional Data:");
            foreach (var kvp in AdditionalData)
            {
                builder.AppendLine($"    {kvp.Key}: {kvp.Value}");
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Gets a user-friendly error message.
    /// </summary>
    /// <returns>A user-friendly error message.</returns>
    public string GetUserMessage()
    {
        return Message;
    }

    /// <summary>
    /// Gets a detailed error message for advanced users or support.
    /// </summary>
    /// <returns>A detailed error message.</returns>
    public string GetDetailedMessage()
    {
        if (!string.IsNullOrEmpty(DetailedMessage))
        {
            return DetailedMessage;
        }

        if (Exception != null)
        {
            return Exception.ToString();
        }

        return Message;
    }

    /// <summary>
    /// Adds additional context data.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void AddData(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        AdditionalData ??= new Dictionary<string, string>();
        AdditionalData[key] = value;
    }

    /// <summary>
    /// Returns a string representation of the error information.
    /// </summary>
    /// <returns>A string representation of the error.</returns>
    public override string ToString()
    {
        return FormatLogMessage();
    }

    #endregion
}

/// <summary>
/// Defines the severity levels for errors.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Informational message (not an error).
    /// </summary>
    Information = 0,

    /// <summary>
    /// Warning (non-critical issue).
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error (operation failed but application can continue).
    /// </summary>
    Error = 2,

    /// <summary>
    /// Critical error (application may be unstable).
    /// </summary>
    Critical = 3
}


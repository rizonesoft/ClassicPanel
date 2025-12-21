using System.Diagnostics;
using System.Windows.Forms;

namespace ClassicPanel.Core;

/// <summary>
/// Provides centralized error logging and user notification functionality.
/// Supports both debug logging and user-facing error messages.
/// </summary>
public static class ErrorLogger
{
    #region LogError Methods

    /// <summary>
    /// Logs an error to debug output using an ErrorInfo object.
    /// </summary>
    /// <param name="errorInfo">The error information to log.</param>
    /// <exception cref="ArgumentNullException">Thrown when errorInfo is null.</exception>
    public static void LogError(ErrorInfo errorInfo)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);
        errorInfo.LogError();
    }

    /// <summary>
    /// Logs an error message to debug output.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    public static void LogError(string message, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Error
        };

        errorInfo.LogError();
    }

    /// <summary>
    /// Logs an exception to debug output.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    public static void LogError(Exception exception, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = ErrorInfo.FromException(exception, context: context);
        errorInfo.LogError();
    }

    /// <summary>
    /// Logs an error message with an exception to debug output.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    public static void LogError(string message, Exception exception, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = new ErrorInfo(message, exception, context ?? string.Empty);
        errorInfo.LogError();
    }

    /// <summary>
    /// Logs a warning message to debug output.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    /// <param name="context">Optional context where the warning occurred.</param>
    public static void LogWarning(string message, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Warning
        };

        errorInfo.LogError();
    }

    /// <summary>
    /// Logs an informational message to debug output.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    /// <param name="context">Optional context where the information was generated.</param>
    public static void LogInformation(string message, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Information
        };

        errorInfo.LogError();
    }

    #endregion

    #region ShowError Methods

    /// <summary>
    /// Shows an error message to the user and logs it to debug output.
    /// </summary>
    /// <param name="errorInfo">The error information to show and log.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <exception cref="ArgumentNullException">Thrown when errorInfo is null.</exception>
    public static void ShowError(ErrorInfo errorInfo, IWin32Window? owner = null)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);

        // Log the error first
        errorInfo.LogError();

        // Show to user only if ShowToUser is true
        if (errorInfo.ShowToUser)
        {
            var icon = GetMessageBoxIcon(errorInfo.Severity);
            MessageBox.Show(
                owner,
                errorInfo.GetUserMessage(),
                AppConstants.ApplicationDisplayName,
                MessageBoxButtons.OK,
                icon
            );
        }
    }

    /// <summary>
    /// Shows an error message to the user and logs it to debug output.
    /// </summary>
    /// <param name="message">The error message to show.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <param name="context">Optional context where the error occurred (for logging).</param>
    public static void ShowError(string message, IWin32Window? owner = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Error
        };

        ShowError(errorInfo, owner);
    }

    /// <summary>
    /// Shows an exception error to the user and logs it to debug output.
    /// </summary>
    /// <param name="exception">The exception to show and log.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <param name="userMessage">Optional user-friendly message. If null, a generic message is used.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    public static void ShowError(Exception exception, IWin32Window? owner = null, string? userMessage = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = ErrorInfo.FromException(exception, userMessage, context);
        ShowError(errorInfo, owner);
    }

    /// <summary>
    /// Shows an error message with an exception to the user and logs it to debug output.
    /// </summary>
    /// <param name="message">The error message to show.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <param name="context">Optional context where the error occurred.</param>
    public static void ShowError(string message, Exception exception, IWin32Window? owner = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(exception);

        var errorInfo = new ErrorInfo(message, exception, context ?? string.Empty);
        ShowError(errorInfo, owner);
    }

    /// <summary>
    /// Shows a warning message to the user and logs it to debug output.
    /// </summary>
    /// <param name="message">The warning message to show.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <param name="context">Optional context where the warning occurred (for logging).</param>
    public static void ShowWarning(string message, IWin32Window? owner = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Warning
        };

        // Log the warning
        errorInfo.LogError();

        // Show to user only if ShowToUser is true
        if (errorInfo.ShowToUser)
        {
            MessageBox.Show(
                owner,
                message,
                AppConstants.ApplicationDisplayName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    /// <summary>
    /// Shows an informational message to the user and logs it to debug output.
    /// </summary>
    /// <param name="message">The informational message to show.</param>
    /// <param name="owner">Optional owner window for the message box.</param>
    /// <param name="context">Optional context where the information was generated (for logging).</param>
    public static void ShowInformation(string message, IWin32Window? owner = null, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        var errorInfo = new ErrorInfo(message)
        {
            Context = context,
            Severity = ErrorSeverity.Information
        };

        // Log the information
        errorInfo.LogError();

        // Show to user only if ShowToUser is true
        if (errorInfo.ShowToUser)
        {
            MessageBox.Show(
                owner,
                message,
                AppConstants.ApplicationDisplayName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the appropriate MessageBox icon for the given error severity.
    /// </summary>
    /// <param name="severity">The error severity level.</param>
    /// <returns>The MessageBox icon corresponding to the severity.</returns>
    private static MessageBoxIcon GetMessageBoxIcon(ErrorSeverity severity)
    {
        return severity switch
        {
            ErrorSeverity.Information => MessageBoxIcon.Information,
            ErrorSeverity.Warning => MessageBoxIcon.Warning,
            ErrorSeverity.Error => MessageBoxIcon.Error,
            ErrorSeverity.Critical => MessageBoxIcon.Stop,
            _ => MessageBoxIcon.Error
        };
    }

    #endregion
}


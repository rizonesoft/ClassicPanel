using System.Diagnostics;

namespace ClassicPanel.Core;

/// <summary>
/// Provides error recovery strategies including retry logic and fallback mechanisms.
/// Helps applications gracefully handle transient failures and recover from errors.
/// </summary>
public static class ErrorRecovery
{
    #region Retry Configuration

    /// <summary>
    /// Default maximum number of retry attempts.
    /// </summary>
    public const int DefaultMaxRetries = 3;

    /// <summary>
    /// Default initial delay in milliseconds before first retry.
    /// </summary>
    public const int DefaultInitialDelayMs = 100;

    /// <summary>
    /// Default maximum delay in milliseconds between retries.
    /// </summary>
    public const int DefaultMaxDelayMs = 2000;

    /// <summary>
    /// Default backoff multiplier for exponential backoff.
    /// </summary>
    public const double DefaultBackoffMultiplier = 2.0;

    #endregion

    #region Retry Methods

    /// <summary>
    /// Executes an operation with retry logic for transient failures.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3).</param>
    /// <param name="initialDelayMs">Initial delay in milliseconds before first retry (default: 100).</param>
    /// <param name="maxDelayMs">Maximum delay in milliseconds between retries (default: 2000).</param>
    /// <param name="backoffMultiplier">Backoff multiplier for exponential backoff (default: 2.0).</param>
    /// <param name="isTransientError">Function to determine if an exception is transient and should be retried. If null, all exceptions are retried.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="Exception">Thrown if all retry attempts fail.</exception>
    public static T Retry<T>(
        Func<T> operation,
        int maxRetries = DefaultMaxRetries,
        int initialDelayMs = DefaultInitialDelayMs,
        int maxDelayMs = DefaultMaxDelayMs,
        double backoffMultiplier = DefaultBackoffMultiplier,
        Func<Exception, bool>? isTransientError = null,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Exception? lastException = null;
        int delay = initialDelayMs;

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                lastException = ex;

                // Check if this is a transient error that should be retried
                if (isTransientError != null && !isTransientError(ex))
                {
                    // Not a transient error - don't retry
                    throw;
                }

                // If this was the last attempt, throw the exception
                if (attempt >= maxRetries)
                {
                    if (!string.IsNullOrEmpty(context))
                    {
                        ErrorLogger.LogError($"Operation failed after {maxRetries + 1} attempts", ex, context);
                    }
                    throw;
                }

                // Log retry attempt
                if (!string.IsNullOrEmpty(context))
                {
                    ErrorLogger.LogWarning(
                        $"Operation failed (attempt {attempt + 1}/{maxRetries + 1}), retrying in {delay}ms: {ex.Message}",
                        context);
                }

                // Wait before retrying (exponential backoff)
                Thread.Sleep(delay);

                // Calculate next delay with exponential backoff
                delay = Math.Min((int)(delay * backoffMultiplier), maxDelayMs);
            }
        }

        // This should never be reached, but compiler requires it
        throw lastException ?? new InvalidOperationException("Retry operation failed with unknown error");
    }

    /// <summary>
    /// Executes an operation with retry logic for transient failures (void return).
    /// </summary>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3).</param>
    /// <param name="initialDelayMs">Initial delay in milliseconds before first retry (default: 100).</param>
    /// <param name="maxDelayMs">Maximum delay in milliseconds between retries (default: 2000).</param>
    /// <param name="backoffMultiplier">Backoff multiplier for exponential backoff (default: 2.0).</param>
    /// <param name="isTransientError">Function to determine if an exception is transient and should be retried. If null, all exceptions are retried.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <exception cref="Exception">Thrown if all retry attempts fail.</exception>
    public static void Retry(
        Action operation,
        int maxRetries = DefaultMaxRetries,
        int initialDelayMs = DefaultInitialDelayMs,
        int maxDelayMs = DefaultMaxDelayMs,
        double backoffMultiplier = DefaultBackoffMultiplier,
        Func<Exception, bool>? isTransientError = null,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Retry<object?>(
            () =>
            {
                operation();
                return null;
            },
            maxRetries,
            initialDelayMs,
            maxDelayMs,
            backoffMultiplier,
            isTransientError,
            context);
    }

    /// <summary>
    /// Executes an operation with retry logic and returns a result indicating success or failure.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="result">The result of the operation if successful.</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3).</param>
    /// <param name="initialDelayMs">Initial delay in milliseconds before first retry (default: 100).</param>
    /// <param name="maxDelayMs">Maximum delay in milliseconds between retries (default: 2000).</param>
    /// <param name="backoffMultiplier">Backoff multiplier for exponential backoff (default: 2.0).</param>
    /// <param name="isTransientError">Function to determine if an exception is transient and should be retried. If null, all exceptions are retried.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>True if the operation succeeded; otherwise, false.</returns>
    public static bool TryRetry<T>(
        Func<T> operation,
        out T? result,
        int maxRetries = DefaultMaxRetries,
        int initialDelayMs = DefaultInitialDelayMs,
        int maxDelayMs = DefaultMaxDelayMs,
        double backoffMultiplier = DefaultBackoffMultiplier,
        Func<Exception, bool>? isTransientError = null,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            result = Retry(operation, maxRetries, initialDelayMs, maxDelayMs, backoffMultiplier, isTransientError, context);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    #endregion

    #region Transient Error Detection

    /// <summary>
    /// Determines if an exception represents a transient error that might succeed on retry.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>True if the exception is likely transient; otherwise, false.</returns>
    public static bool IsTransientError(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        // Check specific exception types first
        if (exception is UnauthorizedAccessException ||
            exception is FileNotFoundException ||
            exception is DirectoryNotFoundException ||
            exception is ArgumentException ||
            exception is ArgumentNullException ||
            exception is InvalidOperationException ||
            exception is NotSupportedException)
        {
            return false; // These are not transient
        }

        // IOException and other exceptions are treated as potentially transient
        return true;
    }

    /// <summary>
    /// Determines if an exception represents a transient file I/O error.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>True if the exception is a transient file I/O error; otherwise, false.</returns>
    public static bool IsTransientFileError(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        // File I/O errors that are typically transient
        if (exception is IOException ioEx)
        {
            // Check for specific error codes that indicate transient errors
            // These are common Windows error codes for file locking, sharing violations, etc.
            var errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(ioEx) & 0xFFFF;
            
            // ERROR_SHARING_VIOLATION (32), ERROR_LOCK_VIOLATION (33), ERROR_ACCESS_DENIED (5)
            // These can be transient if another process releases the file
            return errorCode == 32 || errorCode == 33 || errorCode == 5;
        }

        return false;
    }

    #endregion

    #region Fallback Strategies

    /// <summary>
    /// Executes an operation with a fallback if the primary operation fails.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="primaryOperation">The primary operation to execute.</param>
    /// <param name="fallbackOperation">The fallback operation to execute if primary fails.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the primary operation if successful, otherwise the result of the fallback operation.</returns>
    public static T WithFallback<T>(
        Func<T> primaryOperation,
        Func<T> fallbackOperation,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(primaryOperation);
        ArgumentNullException.ThrowIfNull(fallbackOperation);

        try
        {
            return primaryOperation();
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(context))
            {
                ErrorLogger.LogWarning($"Primary operation failed, using fallback: {ex.Message}", context);
            }

            try
            {
                return fallbackOperation();
            }
            catch (Exception fallbackEx)
            {
                if (!string.IsNullOrEmpty(context))
                {
                    ErrorLogger.LogError("Both primary and fallback operations failed", fallbackEx, context);
                }
                throw;
            }
        }
    }

    /// <summary>
    /// Executes an operation with a fallback if the primary operation fails (void return).
    /// </summary>
    /// <param name="primaryOperation">The primary operation to execute.</param>
    /// <param name="fallbackOperation">The fallback operation to execute if primary fails.</param>
    /// <param name="context">Optional context for error logging.</param>
    public static void WithFallback(
        Action primaryOperation,
        Action fallbackOperation,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(primaryOperation);
        ArgumentNullException.ThrowIfNull(fallbackOperation);

        WithFallback<object?>(
            () =>
            {
                primaryOperation();
                return null;
            },
            () =>
            {
                fallbackOperation();
                return null;
            },
            context);
    }

    /// <summary>
    /// Executes an operation with a fallback value if the operation fails.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="fallbackValue">The fallback value to return if the operation fails.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation if successful, otherwise the fallback value.</returns>
    public static T WithFallbackValue<T>(
        Func<T> operation,
        T fallbackValue,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(context))
            {
                ErrorLogger.LogWarning($"Operation failed, using fallback value: {ex.Message}", context);
            }
            return fallbackValue;
        }
    }

    /// <summary>
    /// Executes an operation with a fallback value if the operation fails (nullable return).
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation if successful, otherwise null.</returns>
    public static T? WithFallbackNull<T>(
        Func<T> operation,
        string? context = null)
        where T : class
    {
        return WithFallbackValue(operation, (T?)null, context);
    }

    #endregion

    #region Combined Retry and Fallback

    /// <summary>
    /// Executes an operation with retry logic and a fallback if all retries fail.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute with retry.</param>
    /// <param name="fallbackOperation">The fallback operation to execute if all retries fail.</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3).</param>
    /// <param name="initialDelayMs">Initial delay in milliseconds before first retry (default: 100).</param>
    /// <param name="maxDelayMs">Maximum delay in milliseconds between retries (default: 2000).</param>
    /// <param name="backoffMultiplier">Backoff multiplier for exponential backoff (default: 2.0).</param>
    /// <param name="isTransientError">Function to determine if an exception is transient and should be retried. If null, all exceptions are retried.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation if successful, otherwise the result of the fallback operation.</returns>
    public static T RetryWithFallback<T>(
        Func<T> operation,
        Func<T> fallbackOperation,
        int maxRetries = DefaultMaxRetries,
        int initialDelayMs = DefaultInitialDelayMs,
        int maxDelayMs = DefaultMaxDelayMs,
        double backoffMultiplier = DefaultBackoffMultiplier,
        Func<Exception, bool>? isTransientError = null,
        string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(fallbackOperation);

        try
        {
            return Retry(operation, maxRetries, initialDelayMs, maxDelayMs, backoffMultiplier, isTransientError, context);
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(context))
            {
                ErrorLogger.LogWarning($"All retry attempts failed, using fallback: {ex.Message}", context);
            }

            try
            {
                return fallbackOperation();
            }
            catch (Exception fallbackEx)
            {
                if (!string.IsNullOrEmpty(context))
                {
                    ErrorLogger.LogError("Both retry and fallback operations failed", fallbackEx, context);
                }
                throw;
            }
        }
    }

    #endregion
}


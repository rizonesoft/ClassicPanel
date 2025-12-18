namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract dialog interface for modal dialog windows.
/// </summary>
public interface IDialog : IWindow
{
    /// <summary>
    /// Gets or sets the dialog result, which determines how the dialog was closed.
    /// </summary>
    DialogResult DialogResult { get; set; }

    /// <summary>
    /// Shows the dialog as a modal window and returns the result.
    /// </summary>
    /// <param name="owner">The owner window, or null for no owner.</param>
    /// <returns>The dialog result indicating how the dialog was closed.</returns>
    DialogResult ShowDialog(IWindow? owner);

    /// <summary>
    /// Closes the dialog and sets the dialog result.
    /// </summary>
    /// <param name="result">The dialog result indicating how the dialog was closed.</param>
    void Close(DialogResult result);
}

/// <summary>
/// Represents the result of a dialog operation.
/// </summary>
public enum DialogResult
{
    /// <summary>
    /// No result returned.
    /// </summary>
    None = 0,

    /// <summary>
    /// The dialog was closed with an OK result.
    /// </summary>
    OK = 1,

    /// <summary>
    /// The dialog was closed with a Cancel result.
    /// </summary>
    Cancel = 2,

    /// <summary>
    /// The dialog was closed with an Abort result.
    /// </summary>
    Abort = 3,

    /// <summary>
    /// The dialog was closed with a Retry result.
    /// </summary>
    Retry = 4,

    /// <summary>
    /// The dialog was closed with an Ignore result.
    /// </summary>
    Ignore = 5,

    /// <summary>
    /// The dialog was closed with a Yes result.
    /// </summary>
    Yes = 6,

    /// <summary>
    /// The dialog was closed with a No result.
    /// </summary>
    No = 7
}



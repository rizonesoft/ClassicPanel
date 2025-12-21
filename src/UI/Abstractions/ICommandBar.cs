namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract command bar (toolbar) interface for framework-agnostic toolbar operations.
/// </summary>
public interface ICommandBar
{
    /// <summary>
    /// Gets or sets a value indicating whether the command bar is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets all buttons in the command bar.
    /// </summary>
    IReadOnlyList<ICommandButton> Buttons { get; }

    /// <summary>
    /// Adds a button to the command bar.
    /// </summary>
    /// <param name="text">The text of the button.</param>
    /// <param name="action">The action to execute when clicked.</param>
    /// <param name="tooltip">The tooltip text for the button.</param>
    /// <returns>The created command button.</returns>
    ICommandButton AddButton(string text, Action? action = null, string? tooltip = null);

    /// <summary>
    /// Adds a button to the command bar with data.
    /// </summary>
    /// <param name="data">The button data.</param>
    /// <returns>The created command button.</returns>
    ICommandButton AddButton(CommandButtonData data);

    /// <summary>
    /// Removes a button from the command bar.
    /// </summary>
    /// <param name="text">The text of the button to remove.</param>
    /// <returns>True if the button was removed; otherwise, false.</returns>
    bool RemoveButton(string text);

    /// <summary>
    /// Gets a button by text.
    /// </summary>
    /// <param name="text">The text of the button.</param>
    /// <returns>The command button, or null if not found.</returns>
    ICommandButton? GetButton(string text);

    /// <summary>
    /// Adds a separator to the command bar.
    /// </summary>
    void AddSeparator();

    /// <summary>
    /// Removes all buttons from the command bar.
    /// </summary>
    void Clear();
}

/// <summary>
/// Represents an abstract command button interface.
/// </summary>
public interface ICommandButton
{
    /// <summary>
    /// Gets or sets the text of the button.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text for the button.
    /// </summary>
    string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Occurs when the button is clicked.
    /// </summary>
    event EventHandler? Click;
}

/// <summary>
/// Represents data for a command button.
/// </summary>
public class CommandButtonData
{
    /// <summary>
    /// Gets or sets the text of the button.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action to execute when the button is clicked.
    /// </summary>
    public Action? Action { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text for the button.
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the button is visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandButtonData"/> class.
    /// </summary>
    public CommandButtonData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandButtonData"/> class.
    /// </summary>
    /// <param name="text">The text of the button.</param>
    /// <param name="action">The action to execute when clicked.</param>
    /// <param name="tooltip">The tooltip text for the button.</param>
    public CommandButtonData(string text, Action? action = null, string? tooltip = null)
    {
        Text = text;
        Action = action;
        Tooltip = tooltip;
    }
}









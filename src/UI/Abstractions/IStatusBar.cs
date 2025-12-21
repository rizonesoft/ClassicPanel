namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract status bar interface for framework-agnostic status bar operations.
/// </summary>
public interface IStatusBar
{
    /// <summary>
    /// Gets or sets a value indicating whether the status bar is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the main status text.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets all status panels in the status bar.
    /// </summary>
    IReadOnlyList<IStatusPanel> Panels { get; }

    /// <summary>
    /// Adds a status panel to the status bar.
    /// </summary>
    /// <param name="text">The text of the panel.</param>
    /// <param name="width">The width of the panel in pixels, or -1 for auto-size.</param>
    /// <returns>The created status panel.</returns>
    IStatusPanel AddPanel(string text, int width = -1);

    /// <summary>
    /// Removes a status panel from the status bar.
    /// </summary>
    /// <param name="index">The index of the panel to remove.</param>
    /// <returns>True if the panel was removed; otherwise, false.</returns>
    bool RemovePanel(int index);

    /// <summary>
    /// Gets a status panel by index.
    /// </summary>
    /// <param name="index">The index of the panel.</param>
    /// <returns>The status panel, or null if not found.</returns>
    IStatusPanel? GetPanel(int index);

    /// <summary>
    /// Removes all panels from the status bar.
    /// </summary>
    void Clear();
}

/// <summary>
/// Represents an abstract status panel interface.
/// </summary>
public interface IStatusPanel
{
    /// <summary>
    /// Gets or sets the text of the panel.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the width of the panel in pixels, or -1 for auto-size.
    /// </summary>
    int Width { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the panel is visible.
    /// </summary>
    bool Visible { get; set; }
}









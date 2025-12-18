namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract context menu interface for framework-agnostic context menu operations.
/// </summary>
public interface IContextMenu
{
    /// <summary>
    /// Gets all menu items in the context menu.
    /// </summary>
    IReadOnlyList<IContextMenuItem> Items { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the context menu is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Adds a menu item to the context menu.
    /// </summary>
    /// <param name="text">The text of the menu item.</param>
    /// <param name="action">The action to execute when clicked.</param>
    /// <param name="enabled">Whether the menu item is enabled.</param>
    /// <returns>The created menu item.</returns>
    IContextMenuItem AddItem(string text, Action? action = null, bool enabled = true);

    /// <summary>
    /// Adds a menu item to the context menu with data.
    /// </summary>
    /// <param name="data">The menu item data.</param>
    /// <returns>The created menu item.</returns>
    IContextMenuItem AddItem(IContextMenuItemData data);

    /// <summary>
    /// Adds a separator to the context menu.
    /// </summary>
    void AddSeparator();

    /// <summary>
    /// Removes a menu item from the context menu.
    /// </summary>
    /// <param name="text">The text of the menu item to remove.</param>
    /// <returns>True if the item was removed; otherwise, false.</returns>
    bool RemoveItem(string text);

    /// <summary>
    /// Removes all items from the context menu.
    /// </summary>
    void Clear();

    /// <summary>
    /// Shows the context menu at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    void Show(int x, int y);

    /// <summary>
    /// Shows the context menu at the specified position relative to a control.
    /// </summary>
    /// <param name="control">The control to show the menu relative to.</param>
    /// <param name="x">The x-coordinate relative to the control.</param>
    /// <param name="y">The y-coordinate relative to the control.</param>
    void Show(object control, int x, int y);
}

/// <summary>
/// Represents an abstract context menu item interface.
/// </summary>
public interface IContextMenuItem
{
    /// <summary>
    /// Gets or sets the text of the menu item.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is checked.
    /// </summary>
    bool Checked { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item can be checked.
    /// </summary>
    bool CheckOnClick { get; set; }

    /// <summary>
    /// Gets the child menu items (for submenus).
    /// </summary>
    IReadOnlyList<IContextMenuItem> Items { get; }

    /// <summary>
    /// Adds a submenu item.
    /// </summary>
    /// <param name="text">The text of the submenu item.</param>
    /// <param name="action">The action to execute when clicked.</param>
    /// <returns>The created submenu item.</returns>
    IContextMenuItem AddSubItem(string text, Action? action = null);

    /// <summary>
    /// Removes a submenu item.
    /// </summary>
    /// <param name="text">The text of the submenu item to remove.</param>
    /// <returns>True if the item was removed; otherwise, false.</returns>
    bool RemoveSubItem(string text);

    /// <summary>
    /// Occurs when the menu item is clicked.
    /// </summary>
    event EventHandler? Click;
}

/// <summary>
/// Represents data for a context menu item.
/// </summary>
public interface IContextMenuItemData
{
    /// <summary>
    /// Gets or sets the text of the menu item.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the action to execute when the menu item is clicked.
    /// </summary>
    Action? Action { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is checked.
    /// </summary>
    bool Checked { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item can be checked.
    /// </summary>
    bool CheckOnClick { get; set; }

    /// <summary>
    /// Gets or sets the child menu items (for submenus).
    /// </summary>
    IEnumerable<IContextMenuItemData>? Children { get; set; }
}

/// <summary>
/// Provides a default implementation of <see cref="IContextMenuItemData"/>.
/// </summary>
public class ContextMenuItemData : IContextMenuItemData
{
    /// <inheritdoc/>
    public string Text { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Action? Action { get; set; }

    /// <inheritdoc/>
    public bool Enabled { get; set; } = true;

    /// <inheritdoc/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc/>
    public bool Checked { get; set; } = false;

    /// <inheritdoc/>
    public bool CheckOnClick { get; set; } = false;

    /// <inheritdoc/>
    public IEnumerable<IContextMenuItemData>? Children { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMenuItemData"/> class.
    /// </summary>
    public ContextMenuItemData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMenuItemData"/> class.
    /// </summary>
    /// <param name="text">The text of the menu item.</param>
    /// <param name="action">The action to execute when clicked.</param>
    public ContextMenuItemData(string text, Action? action = null)
    {
        Text = text;
        Action = action;
    }
}



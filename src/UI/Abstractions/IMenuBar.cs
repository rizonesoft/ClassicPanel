namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract menu bar interface for framework-agnostic menu operations.
/// </summary>
public interface IMenuBar
{
    /// <summary>
    /// Adds a menu to the menu bar.
    /// </summary>
    /// <param name="name">The name of the menu.</param>
    /// <param name="items">The menu items to add.</param>
    /// <returns>The created menu item.</returns>
    IMenuItem AddMenu(string name, IEnumerable<MenuItemData> items);

    /// <summary>
    /// Adds a menu to the menu bar.
    /// </summary>
    /// <param name="name">The name of the menu.</param>
    /// <returns>The created menu item.</returns>
    IMenuItem AddMenu(string name);

    /// <summary>
    /// Removes a menu from the menu bar.
    /// </summary>
    /// <param name="name">The name of the menu to remove.</param>
    /// <returns>True if the menu was removed; otherwise, false.</returns>
    bool RemoveMenu(string name);

    /// <summary>
    /// Gets a menu by name.
    /// </summary>
    /// <param name="name">The name of the menu.</param>
    /// <returns>The menu item, or null if not found.</returns>
    IMenuItem? GetMenu(string name);

    /// <summary>
    /// Gets all menus in the menu bar.
    /// </summary>
    IReadOnlyList<IMenuItem> Menus { get; }

    /// <summary>
    /// Removes all menus from the menu bar.
    /// </summary>
    void Clear();
}

/// <summary>
/// Represents an abstract menu item interface.
/// </summary>
public interface IMenuItem
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
    /// Gets or sets the shortcut key for the menu item.
    /// </summary>
    string? ShortcutKey { get; set; }

    /// <summary>
    /// Gets the child menu items.
    /// </summary>
    IReadOnlyList<IMenuItem> Items { get; }

    /// <summary>
    /// Adds a submenu item.
    /// </summary>
    /// <param name="text">The text of the menu item.</param>
    /// <param name="action">The action to execute when clicked.</param>
    /// <returns>The created menu item.</returns>
    IMenuItem AddItem(string text, Action? action = null);

    /// <summary>
    /// Adds a submenu item with data.
    /// </summary>
    /// <param name="data">The menu item data.</param>
    /// <returns>The created menu item.</returns>
    IMenuItem AddItem(MenuItemData data);

    /// <summary>
    /// Adds a separator to the menu.
    /// </summary>
    void AddSeparator();

    /// <summary>
    /// Removes a submenu item.
    /// </summary>
    /// <param name="text">The text of the menu item to remove.</param>
    /// <returns>True if the item was removed; otherwise, false.</returns>
    bool RemoveItem(string text);

    /// <summary>
    /// Removes all submenu items.
    /// </summary>
    void Clear();

    /// <summary>
    /// Occurs when the menu item is clicked.
    /// </summary>
    event EventHandler? Click;
}

/// <summary>
/// Represents data for a menu item.
/// </summary>
public class MenuItemData
{
    /// <summary>
    /// Gets or sets the text of the menu item.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action to execute when the menu item is clicked.
    /// </summary>
    public Action? Action { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the menu item is visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the shortcut key for the menu item.
    /// </summary>
    public string? ShortcutKey { get; set; }

    /// <summary>
    /// Gets or sets the child menu items.
    /// </summary>
    public IEnumerable<MenuItemData>? Children { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemData"/> class.
    /// </summary>
    public MenuItemData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemData"/> class.
    /// </summary>
    /// <param name="text">The text of the menu item.</param>
    /// <param name="action">The action to execute when clicked.</param>
    public MenuItemData(string text, Action? action = null)
    {
        Text = text;
        Action = action;
    }
}









using ClassicPanel.Core;

namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract list view interface for displaying Control Panel items.
/// </summary>
public interface IListView
{
    /// <summary>
    /// Gets or sets the view mode (icons, list, details, etc.).
    /// </summary>
    ListViewMode ViewMode { get; set; }

    /// <summary>
    /// Gets or sets the currently selected item.
    /// </summary>
    CplItem? SelectedItem { get; set; }

    /// <summary>
    /// Gets all items in the list view.
    /// </summary>
    IReadOnlyList<CplItem> Items { get; }

    /// <summary>
    /// Gets the number of items in the list view.
    /// </summary>
    int ItemCount { get; }

    /// <summary>
    /// Adds an item to the list view.
    /// </summary>
    /// <param name="item">The Control Panel item to add.</param>
    void AddItem(CplItem item);

    /// <summary>
    /// Adds multiple items to the list view.
    /// </summary>
    /// <param name="items">The Control Panel items to add.</param>
    void AddItems(IEnumerable<CplItem> items);

    /// <summary>
    /// Removes an item from the list view.
    /// </summary>
    /// <param name="item">The Control Panel item to remove.</param>
    /// <returns>True if the item was removed; otherwise, false.</returns>
    bool RemoveItem(CplItem item);

    /// <summary>
    /// Removes all items from the list view.
    /// </summary>
    void ClearItems();

    /// <summary>
    /// Finds an item by name.
    /// </summary>
    /// <param name="name">The name of the item to find.</param>
    /// <returns>The found item, or null if not found.</returns>
    CplItem? FindItem(string name);

    /// <summary>
    /// Sorts the items in the list view.
    /// </summary>
    /// <param name="comparison">The comparison function to use for sorting.</param>
    void SortItems(Comparison<CplItem> comparison);

    /// <summary>
    /// Refreshes the display of the list view.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Occurs when an item is double-clicked.
    /// </summary>
    event EventHandler<ItemClickEventArgs>? ItemDoubleClick;

    /// <summary>
    /// Occurs when the selected item changes.
    /// </summary>
    event EventHandler<ItemSelectionChangedEventArgs>? SelectionChanged;
}

/// <summary>
/// Represents the view mode for a list view.
/// </summary>
public enum ListViewMode
{
    /// <summary>
    /// Large icons view.
    /// </summary>
    LargeIcons = 0,

    /// <summary>
    /// Small icons view.
    /// </summary>
    SmallIcons = 1,

    /// <summary>
    /// List view.
    /// </summary>
    List = 2,

    /// <summary>
    /// Details view with columns.
    /// </summary>
    Details = 3,

    /// <summary>
    /// Tiles view.
    /// </summary>
    Tiles = 4
}

/// <summary>
/// Provides data for item click events.
/// </summary>
public class ItemClickEventArgs : EventArgs
{
    /// <summary>
    /// Gets the item that was clicked.
    /// </summary>
    public CplItem Item { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemClickEventArgs"/> class.
    /// </summary>
    /// <param name="item">The item that was clicked.</param>
    public ItemClickEventArgs(CplItem item)
    {
        Item = item;
    }
}

/// <summary>
/// Provides data for item selection change events.
/// </summary>
public class ItemSelectionChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the previously selected item.
    /// </summary>
    public CplItem? PreviousItem { get; }

    /// <summary>
    /// Gets the newly selected item.
    /// </summary>
    public CplItem? NewItem { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemSelectionChangedEventArgs"/> class.
    /// </summary>
    /// <param name="previousItem">The previously selected item.</param>
    /// <param name="newItem">The newly selected item.</param>
    public ItemSelectionChangedEventArgs(CplItem? previousItem, CplItem? newItem)
    {
        PreviousItem = previousItem;
        NewItem = newItem;
    }
}



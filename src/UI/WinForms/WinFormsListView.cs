using ClassicPanel.Core;
using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IListView interface.
/// </summary>
public class WinFormsListView : IListView
{
    private readonly System.Windows.Forms.ListView _listView;
    private readonly Dictionary<CplItem, System.Windows.Forms.ListViewItem> _itemMap = new();
    private readonly List<CplItem> _items = new();

    /// <summary>
    /// Gets the underlying WinForms ListView control.
    /// </summary>
    public System.Windows.Forms.ListView ListView => _listView;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsListView"/> class.
    /// </summary>
    public WinFormsListView()
    {
        _listView = new System.Windows.Forms.ListView();
        _listView.DoubleClick += (sender, e) =>
        {
            if (_listView.SelectedItems.Count > 0)
            {
                var selectedItem = _listView.SelectedItems[0];
                var cplItem = selectedItem.Tag as CplItem;
                if (cplItem != null)
                {
                    ItemDoubleClick?.Invoke(this, new ItemClickEventArgs(cplItem));
                }
            }
        };
        _listView.SelectedIndexChanged += (sender, e) =>
        {
            CplItem? previousItem = null;
            CplItem? newItem = null;

            if (_listView.SelectedItems.Count > 0)
            {
                var selectedItem = _listView.SelectedItems[0];
                newItem = selectedItem.Tag as CplItem;
            }

            SelectionChanged?.Invoke(this, new ItemSelectionChangedEventArgs(previousItem, newItem));
        };
    }

    /// <inheritdoc/>
    public ListViewMode ViewMode
    {
        get => _listView.View switch
        {
            System.Windows.Forms.View.LargeIcon => ListViewMode.LargeIcons,
            System.Windows.Forms.View.SmallIcon => ListViewMode.SmallIcons,
            System.Windows.Forms.View.List => ListViewMode.List,
            System.Windows.Forms.View.Details => ListViewMode.Details,
            System.Windows.Forms.View.Tile => ListViewMode.Tiles,
            _ => ListViewMode.LargeIcons
        };
        set => _listView.View = value switch
        {
            ListViewMode.LargeIcons => System.Windows.Forms.View.LargeIcon,
            ListViewMode.SmallIcons => System.Windows.Forms.View.SmallIcon,
            ListViewMode.List => System.Windows.Forms.View.List,
            ListViewMode.Details => System.Windows.Forms.View.Details,
            ListViewMode.Tiles => System.Windows.Forms.View.Tile,
            _ => System.Windows.Forms.View.LargeIcon
        };
    }

    /// <inheritdoc/>
    public CplItem? SelectedItem
    {
        get
        {
            if (_listView.SelectedItems.Count > 0)
            {
                return _listView.SelectedItems[0].Tag as CplItem;
            }
            return null;
        }
        set
        {
            if (value != null && _itemMap.TryGetValue(value, out var listViewItem))
            {
                listViewItem.Selected = true;
                listViewItem.EnsureVisible();
            }
            else
            {
                _listView.SelectedItems.Clear();
            }
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<CplItem> Items => _items.AsReadOnly();

    /// <inheritdoc/>
    public int ItemCount => _items.Count;

    /// <inheritdoc/>
    public void AddItem(CplItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (_itemMap.ContainsKey(item))
            return;

        var listViewItem = new System.Windows.Forms.ListViewItem(item.Name)
        {
            Tag = item
        };

        if (item.Icon != null)
        {
            if (_listView.LargeImageList == null)
            {
                _listView.LargeImageList = new System.Windows.Forms.ImageList();
            }
            if (_listView.SmallImageList == null)
            {
                _listView.SmallImageList = new System.Windows.Forms.ImageList();
            }

            _listView.LargeImageList.Images.Add(item.Icon.ToBitmap());
            _listView.SmallImageList.Images.Add(item.Icon.ToBitmap());
            listViewItem.ImageIndex = _listView.LargeImageList.Images.Count - 1;
        }

        _listView.Items.Add(listViewItem);
        _itemMap[item] = listViewItem;
        _items.Add(item);
    }

    /// <inheritdoc/>
    public void AddItems(IEnumerable<CplItem> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    /// <inheritdoc/>
    public bool RemoveItem(CplItem item)
    {
        if (item == null)
            return false;

        if (_itemMap.TryGetValue(item, out var listViewItem))
        {
            _listView.Items.Remove(listViewItem);
            _itemMap.Remove(item);
            _items.Remove(item);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public void ClearItems()
    {
        _listView.Items.Clear();
        _itemMap.Clear();
        _items.Clear();
    }

    /// <inheritdoc/>
    public CplItem? FindItem(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return _items.FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    public void SortItems(Comparison<CplItem> comparison)
    {
        if (comparison == null)
            throw new ArgumentNullException(nameof(comparison));

        _items.Sort(comparison);
        _listView.Items.Clear();
        _itemMap.Clear();

        foreach (var item in _items)
        {
            var listViewItem = new System.Windows.Forms.ListViewItem(item.Name)
            {
                Tag = item
            };
            _listView.Items.Add(listViewItem);
            _itemMap[item] = listViewItem;
        }
    }

    /// <inheritdoc/>
    public void Refresh()
    {
        _listView.Refresh();
    }

    /// <inheritdoc/>
    public event EventHandler<ItemClickEventArgs>? ItemDoubleClick;

    /// <inheritdoc/>
    public event EventHandler<ItemSelectionChangedEventArgs>? SelectionChanged;
}


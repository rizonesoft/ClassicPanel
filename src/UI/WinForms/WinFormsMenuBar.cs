using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IMenuBar interface.
/// </summary>
public class WinFormsMenuBar : IMenuBar
{
    private readonly System.Windows.Forms.MenuStrip _menuStrip;
    private readonly Dictionary<string, WinFormsMenuItem> _menus = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the underlying WinForms MenuStrip control.
    /// </summary>
    public System.Windows.Forms.MenuStrip MenuStrip => _menuStrip;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsMenuBar"/> class.
    /// </summary>
    public WinFormsMenuBar()
    {
        _menuStrip = new System.Windows.Forms.MenuStrip();
    }

    /// <inheritdoc/>
    public IMenuItem AddMenu(string name, IEnumerable<MenuItemData> items)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Menu name cannot be null or empty.", nameof(name));

        if (_menus.ContainsKey(name))
        {
            return _menus[name];
        }

        var menuItem = new System.Windows.Forms.ToolStripMenuItem(name);
        var wrapper = new WinFormsMenuItem(menuItem);

        if (items != null)
        {
            foreach (var itemData in items)
            {
                wrapper.AddItem(itemData);
            }
        }

        _menuStrip.Items.Add(menuItem);
        _menus[name] = wrapper;

        return wrapper;
    }

    /// <inheritdoc/>
    public IMenuItem AddMenu(string name)
    {
        return AddMenu(name, Enumerable.Empty<MenuItemData>());
    }

    /// <inheritdoc/>
    public bool RemoveMenu(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        if (_menus.TryGetValue(name, out var menuItem))
        {
            _menuStrip.Items.Remove(menuItem.MenuItem);
            _menus.Remove(name);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public IMenuItem? GetMenu(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        _menus.TryGetValue(name, out var menuItem);
        return menuItem;
    }

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItem> Menus => _menus.Values.Cast<IMenuItem>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public void Clear()
    {
        _menuStrip.Items.Clear();
        _menus.Clear();
    }
}

/// <summary>
/// WinForms implementation of the IMenuItem interface.
/// </summary>
internal class WinFormsMenuItem : IMenuItem
{
    private readonly System.Windows.Forms.ToolStripMenuItem _menuItem;
    private readonly List<WinFormsMenuItem> _childItems = new();

    /// <summary>
    /// Gets the underlying WinForms ToolStripMenuItem.
    /// </summary>
    public System.Windows.Forms.ToolStripMenuItem MenuItem => _menuItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsMenuItem"/> class.
    /// </summary>
    public WinFormsMenuItem(System.Windows.Forms.ToolStripMenuItem menuItem)
    {
        _menuItem = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
        _menuItem.Click += (sender, e) => Click?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc/>
    public string Text
    {
        get => _menuItem.Text;
        set => _menuItem.Text = value;
    }

    /// <inheritdoc/>
    public bool Enabled
    {
        get => _menuItem.Enabled;
        set => _menuItem.Enabled = value;
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _menuItem.Visible;
        set => _menuItem.Visible = value;
    }

    /// <inheritdoc/>
    public string? ShortcutKey
    {
        get => _menuItem.ShortcutKeys != System.Windows.Forms.Keys.None 
            ? _menuItem.ShortcutKeys.ToString() 
            : null;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _menuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
            }
            else if (Enum.TryParse<System.Windows.Forms.Keys>(value, out var keys))
            {
                _menuItem.ShortcutKeys = keys;
            }
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItem> Items => _childItems.Cast<IMenuItem>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public IMenuItem AddItem(string text, Action? action = null)
    {
        var childItem = new System.Windows.Forms.ToolStripMenuItem(text);
        if (action != null)
        {
            childItem.Click += (sender, e) => action();
        }

        var wrapper = new WinFormsMenuItem(childItem);
        _menuItem.DropDownItems.Add(childItem);
        _childItems.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public IMenuItem AddItem(MenuItemData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var childItem = new System.Windows.Forms.ToolStripMenuItem(data.Text)
        {
            Enabled = data.Enabled,
            Visible = data.Visible
        };

        if (!string.IsNullOrWhiteSpace(data.ShortcutKey) && 
            Enum.TryParse<System.Windows.Forms.Keys>(data.ShortcutKey, out var keys))
        {
            childItem.ShortcutKeys = keys;
        }

        if (data.Action != null)
        {
            childItem.Click += (sender, e) => data.Action();
        }

        var wrapper = new WinFormsMenuItem(childItem);
        _menuItem.DropDownItems.Add(childItem);
        _childItems.Add(wrapper);

        if (data.Children != null)
        {
            foreach (var childData in data.Children)
            {
                wrapper.AddItem(childData);
            }
        }

        return wrapper;
    }

    /// <inheritdoc/>
    public void AddSeparator()
    {
        _menuItem.DropDownItems.Add(new System.Windows.Forms.ToolStripSeparator());
    }

    /// <inheritdoc/>
    public bool RemoveItem(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        for (int i = 0; i < _menuItem.DropDownItems.Count; i++)
        {
            if (_menuItem.DropDownItems[i] is System.Windows.Forms.ToolStripMenuItem item && 
                item.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                _menuItem.DropDownItems.RemoveAt(i);
                _childItems.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _menuItem.DropDownItems.Clear();
        _childItems.Clear();
    }

    /// <inheritdoc/>
    public event EventHandler? Click;
}


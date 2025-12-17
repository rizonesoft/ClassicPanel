using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IContextMenu interface.
/// </summary>
public class WinFormsContextMenu : IContextMenu
{
    private readonly System.Windows.Forms.ContextMenuStrip _contextMenu;
    private readonly List<WinFormsContextMenuItem> _items = new();

    /// <summary>
    /// Gets the underlying WinForms ContextMenuStrip control.
    /// </summary>
    public System.Windows.Forms.ContextMenuStrip ContextMenuStrip => _contextMenu;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsContextMenu"/> class.
    /// </summary>
    public WinFormsContextMenu()
    {
        _contextMenu = new System.Windows.Forms.ContextMenuStrip();
    }

    /// <inheritdoc/>
    public IReadOnlyList<IContextMenuItem> Items => _items.Cast<IContextMenuItem>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public bool Visible
    {
        get => _contextMenu.Visible;
        set => _contextMenu.Visible = value;
    }

    /// <inheritdoc/>
    public IContextMenuItem AddItem(string text, Action? action = null, bool enabled = true)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Item text cannot be null or empty.", nameof(text));

        var menuItem = new System.Windows.Forms.ToolStripMenuItem(text)
        {
            Enabled = enabled
        };

        if (action != null)
        {
            menuItem.Click += (sender, e) => action();
        }

        var wrapper = new WinFormsContextMenuItem(menuItem);
        _contextMenu.Items.Add(menuItem);
        _items.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public IContextMenuItem AddItem(IContextMenuItemData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var menuItem = new System.Windows.Forms.ToolStripMenuItem(data.Text)
        {
            Enabled = data.Enabled,
            Visible = data.Visible,
            Checked = data.Checked,
            CheckOnClick = data.CheckOnClick
        };

        if (data.Action != null)
        {
            menuItem.Click += (sender, e) => data.Action();
        }

        var wrapper = new WinFormsContextMenuItem(menuItem);
        _contextMenu.Items.Add(menuItem);
        _items.Add(wrapper);

        if (data.Children != null)
        {
            foreach (var childData in data.Children)
            {
                wrapper.AddSubItem(childData.Text, childData.Action);
            }
        }

        return wrapper;
    }

    /// <inheritdoc/>
    public void AddSeparator()
    {
        _contextMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
    }

    /// <inheritdoc/>
    public bool RemoveItem(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        for (int i = 0; i < _contextMenu.Items.Count; i++)
        {
            if (_contextMenu.Items[i] is System.Windows.Forms.ToolStripMenuItem item && 
                item.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                _contextMenu.Items.RemoveAt(i);
                _items.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _contextMenu.Items.Clear();
        _items.Clear();
    }

    /// <inheritdoc/>
    public void Show(int x, int y)
    {
        _contextMenu.Show(x, y);
    }

    /// <inheritdoc/>
    public void Show(object control, int x, int y)
    {
        if (control is System.Windows.Forms.Control winFormsControl)
        {
            _contextMenu.Show(winFormsControl, x, y);
        }
        else
        {
            throw new ArgumentException("Control must be a WinForms Control instance.", nameof(control));
        }
    }
}

/// <summary>
/// WinForms implementation of the IContextMenuItem interface.
/// </summary>
internal class WinFormsContextMenuItem : IContextMenuItem
{
    private readonly System.Windows.Forms.ToolStripMenuItem _menuItem;
    private readonly List<WinFormsContextMenuItem> _childItems = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsContextMenuItem"/> class.
    /// </summary>
    public WinFormsContextMenuItem(System.Windows.Forms.ToolStripMenuItem menuItem)
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
    public bool Checked
    {
        get => _menuItem.Checked;
        set => _menuItem.Checked = value;
    }

    /// <inheritdoc/>
    public bool CheckOnClick
    {
        get => _menuItem.CheckOnClick;
        set => _menuItem.CheckOnClick = value;
    }

    /// <inheritdoc/>
    public IReadOnlyList<IContextMenuItem> Items => _childItems.Cast<IContextMenuItem>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public IContextMenuItem AddSubItem(string text, Action? action = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Item text cannot be null or empty.", nameof(text));

        var subItem = new System.Windows.Forms.ToolStripMenuItem(text);
        if (action != null)
        {
            subItem.Click += (sender, e) => action();
        }

        var wrapper = new WinFormsContextMenuItem(subItem);
        _menuItem.DropDownItems.Add(subItem);
        _childItems.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public bool RemoveSubItem(string text)
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
    public event EventHandler? Click;
}


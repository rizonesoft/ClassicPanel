using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IStatusBar interface.
/// </summary>
public class WinFormsStatusBar : IStatusBar
{
    private readonly System.Windows.Forms.StatusStrip _statusStrip;
    private readonly List<WinFormsStatusPanel> _panels = new();

    /// <summary>
    /// Gets the underlying WinForms StatusStrip control.
    /// </summary>
    public System.Windows.Forms.StatusStrip StatusStrip => _statusStrip;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsStatusBar"/> class.
    /// </summary>
    public WinFormsStatusBar()
    {
        _statusStrip = new System.Windows.Forms.StatusStrip();
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _statusStrip.Visible;
        set => _statusStrip.Visible = value;
    }

    /// <inheritdoc/>
    public string Text
    {
        get => _statusStrip.Items.Count > 0 && _statusStrip.Items[0] is System.Windows.Forms.ToolStripStatusLabel label
            ? label.Text
            : string.Empty;
        set
        {
            if (_statusStrip.Items.Count == 0)
            {
                AddPanel(value);
            }
            else if (_statusStrip.Items[0] is System.Windows.Forms.ToolStripStatusLabel label)
            {
                label.Text = value;
            }
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<IStatusPanel> Panels => _panels.Cast<IStatusPanel>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public IStatusPanel AddPanel(string text, int width = -1)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        var panel = new System.Windows.Forms.ToolStripStatusLabel(text);
        if (width >= 0)
        {
            panel.Width = width;
            panel.AutoSize = false;
        }

        var wrapper = new WinFormsStatusPanel(panel);
        _statusStrip.Items.Add(panel);
        _panels.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public bool RemovePanel(int index)
    {
        if (index < 0 || index >= _statusStrip.Items.Count)
            return false;

        _statusStrip.Items.RemoveAt(index);
        if (index < _panels.Count)
        {
            _panels.RemoveAt(index);
        }
        return true;
    }

    /// <inheritdoc/>
    public IStatusPanel? GetPanel(int index)
    {
        if (index < 0 || index >= _panels.Count)
            return null;

        return _panels[index];
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _statusStrip.Items.Clear();
        _panels.Clear();
    }
}

/// <summary>
/// WinForms implementation of the IStatusPanel interface.
/// </summary>
internal class WinFormsStatusPanel : IStatusPanel
{
    private readonly System.Windows.Forms.ToolStripStatusLabel _panel;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsStatusPanel"/> class.
    /// </summary>
    public WinFormsStatusPanel(System.Windows.Forms.ToolStripStatusLabel panel)
    {
        _panel = panel ?? throw new ArgumentNullException(nameof(panel));
    }

    /// <inheritdoc/>
    public string Text
    {
        get => _panel.Text;
        set => _panel.Text = value;
    }

    /// <inheritdoc/>
    public int Width
    {
        get => _panel.Width;
        set
        {
            _panel.Width = value;
            _panel.AutoSize = value < 0;
        }
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _panel.Visible;
        set => _panel.Visible = value;
    }
}


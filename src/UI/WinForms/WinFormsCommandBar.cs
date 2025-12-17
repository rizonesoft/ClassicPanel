using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the ICommandBar interface.
/// </summary>
public class WinFormsCommandBar : ICommandBar
{
    private readonly System.Windows.Forms.ToolStrip _toolStrip;
    private readonly List<WinFormsCommandButton> _buttons = new();

    /// <summary>
    /// Gets the underlying WinForms ToolStrip control.
    /// </summary>
    public System.Windows.Forms.ToolStrip ToolStrip => _toolStrip;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsCommandBar"/> class.
    /// </summary>
    public WinFormsCommandBar()
    {
        _toolStrip = new System.Windows.Forms.ToolStrip();
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _toolStrip.Visible;
        set => _toolStrip.Visible = value;
    }

    /// <inheritdoc/>
    public IReadOnlyList<ICommandButton> Buttons => _buttons.Cast<ICommandButton>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public ICommandButton AddButton(string text, Action? action = null, string? tooltip = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Button text cannot be null or empty.", nameof(text));

        var button = new System.Windows.Forms.ToolStripButton(text);
        if (!string.IsNullOrWhiteSpace(tooltip))
        {
            button.ToolTipText = tooltip;
        }

        if (action != null)
        {
            button.Click += (sender, e) => action();
        }

        var wrapper = new WinFormsCommandButton(button);
        _toolStrip.Items.Add(button);
        _buttons.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public ICommandButton AddButton(CommandButtonData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var button = new System.Windows.Forms.ToolStripButton(data.Text)
        {
            Enabled = data.Enabled,
            Visible = data.Visible
        };

        if (!string.IsNullOrWhiteSpace(data.Tooltip))
        {
            button.ToolTipText = data.Tooltip;
        }

        if (data.Action != null)
        {
            button.Click += (sender, e) => data.Action();
        }

        var wrapper = new WinFormsCommandButton(button);
        _toolStrip.Items.Add(button);
        _buttons.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public bool RemoveButton(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        for (int i = 0; i < _toolStrip.Items.Count; i++)
        {
            if (_toolStrip.Items[i] is System.Windows.Forms.ToolStripButton button && 
                button.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                _toolStrip.Items.RemoveAt(i);
                _buttons.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public ICommandButton? GetButton(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        for (int i = 0; i < _toolStrip.Items.Count; i++)
        {
            if (_toolStrip.Items[i] is System.Windows.Forms.ToolStripButton button && 
                button.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                return _buttons[i];
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public void AddSeparator()
    {
        _toolStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _toolStrip.Items.Clear();
        _buttons.Clear();
    }
}

/// <summary>
/// WinForms implementation of the ICommandButton interface.
/// </summary>
internal class WinFormsCommandButton : ICommandButton
{
    private readonly System.Windows.Forms.ToolStripButton _button;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsCommandButton"/> class.
    /// </summary>
    public WinFormsCommandButton(System.Windows.Forms.ToolStripButton button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
        _button.Click += (sender, e) => Click?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc/>
    public string Text
    {
        get => _button.Text;
        set => _button.Text = value;
    }

    /// <inheritdoc/>
    public string? Tooltip
    {
        get => _button.ToolTipText;
        set => _button.ToolTipText = value;
    }

    /// <inheritdoc/>
    public bool Enabled
    {
        get => _button.Enabled;
        set => _button.Enabled = value;
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _button.Visible;
        set => _button.Visible = value;
    }

    /// <inheritdoc/>
    public event EventHandler? Click;
}


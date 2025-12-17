using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IDialog interface.
/// </summary>
public class WinFormsDialog : IDialog
{
    private readonly WinFormsWindow _window;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsDialog"/> class.
    /// </summary>
    public WinFormsDialog()
    {
        _window = new WinFormsWindow();
        _window.Form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        _window.Form.MaximizeBox = false;
        _window.Form.MinimizeBox = false;
    }

    /// <inheritdoc/>
    public Abstractions.DialogResult DialogResult { get; set; } = Abstractions.DialogResult.None;

    /// <inheritdoc/>
    public Abstractions.DialogResult ShowDialog(IWindow? owner)
    {
        System.Windows.Forms.Form? ownerForm = null;
        if (owner is WinFormsWindow winFormsOwner)
        {
            ownerForm = winFormsOwner.Form;
        }

        var result = _window.Form.ShowDialog(ownerForm);
        return result switch
        {
            System.Windows.Forms.DialogResult.OK => Abstractions.DialogResult.OK,
            System.Windows.Forms.DialogResult.Cancel => Abstractions.DialogResult.Cancel,
            System.Windows.Forms.DialogResult.Abort => Abstractions.DialogResult.Abort,
            System.Windows.Forms.DialogResult.Retry => Abstractions.DialogResult.Retry,
            System.Windows.Forms.DialogResult.Ignore => Abstractions.DialogResult.Ignore,
            System.Windows.Forms.DialogResult.Yes => Abstractions.DialogResult.Yes,
            System.Windows.Forms.DialogResult.No => Abstractions.DialogResult.No,
            _ => Abstractions.DialogResult.None
        };
    }

    /// <inheritdoc/>
    public void Close(Abstractions.DialogResult result)
    {
        DialogResult = result;
        _window.Form.DialogResult = result switch
        {
            Abstractions.DialogResult.OK => System.Windows.Forms.DialogResult.OK,
            Abstractions.DialogResult.Cancel => System.Windows.Forms.DialogResult.Cancel,
            Abstractions.DialogResult.Abort => System.Windows.Forms.DialogResult.Abort,
            Abstractions.DialogResult.Retry => System.Windows.Forms.DialogResult.Retry,
            Abstractions.DialogResult.Ignore => System.Windows.Forms.DialogResult.Ignore,
            Abstractions.DialogResult.Yes => System.Windows.Forms.DialogResult.Yes,
            Abstractions.DialogResult.No => System.Windows.Forms.DialogResult.No,
            _ => System.Windows.Forms.DialogResult.None
        };
        _window.Close();
    }

    // IWindow implementation delegates to _window
    /// <inheritdoc/>
    public string Title
    {
        get => _window.Title;
        set => _window.Title = value;
    }

    /// <inheritdoc/>
    public int Width
    {
        get => _window.Width;
        set => _window.Width = value;
    }

    /// <inheritdoc/>
    public int Height
    {
        get => _window.Height;
        set => _window.Height = value;
    }

    /// <inheritdoc/>
    public int Left
    {
        get => _window.Left;
        set => _window.Left = value;
    }

    /// <inheritdoc/>
    public int Top
    {
        get => _window.Top;
        set => _window.Top = value;
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _window.Visible;
        set => _window.Visible = value;
    }

    /// <inheritdoc/>
    public bool Enabled
    {
        get => _window.Enabled;
        set => _window.Enabled = value;
    }

    /// <inheritdoc/>
    public WindowState State
    {
        get => _window.State;
        set => _window.State = value;
    }

    /// <inheritdoc/>
    public bool Resizable
    {
        get => _window.Resizable;
        set => _window.Resizable = value;
    }

    /// <inheritdoc/>
    public bool Minimizable
    {
        get => _window.Minimizable;
        set => _window.Minimizable = value;
    }

    /// <inheritdoc/>
    public bool Maximizable
    {
        get => _window.Maximizable;
        set => _window.Maximizable = value;
    }

    /// <inheritdoc/>
    public WindowStartPosition StartPosition
    {
        get => _window.StartPosition;
        set => _window.StartPosition = value;
    }

    /// <inheritdoc/>
    public int MinimumWidth
    {
        get => _window.MinimumWidth;
        set => _window.MinimumWidth = value;
    }

    /// <inheritdoc/>
    public int MinimumHeight
    {
        get => _window.MinimumHeight;
        set => _window.MinimumHeight = value;
    }

    /// <inheritdoc/>
    public int MaximumWidth
    {
        get => _window.MaximumWidth;
        set => _window.MaximumWidth = value;
    }

    /// <inheritdoc/>
    public int MaximumHeight
    {
        get => _window.MaximumHeight;
        set => _window.MaximumHeight = value;
    }

    /// <inheritdoc/>
    public void Show()
    {
        _window.Show();
    }

    /// <inheritdoc/>
    public void Hide()
    {
        _window.Hide();
    }

    /// <inheritdoc/>
    public void Close()
    {
        _window.Close();
    }

    /// <inheritdoc/>
    public void Center()
    {
        _window.Center();
    }

    /// <inheritdoc/>
    public void Activate()
    {
        _window.Activate();
    }

    /// <inheritdoc/>
    public event EventHandler? Closed
    {
        add => _window.Closed += value;
        remove => _window.Closed -= value;
    }

    /// <inheritdoc/>
    public event EventHandler? Shown
    {
        add => _window.Shown += value;
        remove => _window.Shown -= value;
    }

    /// <inheritdoc/>
    public event EventHandler<SizeChangedEventArgs>? SizeChanged
    {
        add => _window.SizeChanged += value;
        remove => _window.SizeChanged -= value;
    }

    /// <inheritdoc/>
    public event EventHandler<PositionChangedEventArgs>? PositionChanged
    {
        add => _window.PositionChanged += value;
        remove => _window.PositionChanged -= value;
    }

    /// <inheritdoc/>
    public event EventHandler<WindowClosingEventArgs>? Closing
    {
        add => _window.Closing += value;
        remove => _window.Closing -= value;
    }
}


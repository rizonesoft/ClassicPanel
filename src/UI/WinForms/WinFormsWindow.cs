using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the IWindow interface.
/// </summary>
public class WinFormsWindow : IWindow
{
    private readonly System.Windows.Forms.Form _form;

    /// <summary>
    /// Gets the underlying WinForms Form instance.
    /// </summary>
    public System.Windows.Forms.Form Form => _form;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsWindow"/> class.
    /// </summary>
    public WinFormsWindow()
    {
        _form = new System.Windows.Forms.Form();
        _form.FormClosed += (sender, e) => Closed?.Invoke(this, EventArgs.Empty);
        _form.Shown += (sender, e) => Shown?.Invoke(this, EventArgs.Empty);
        _form.Resize += (sender, e) => SizeChanged?.Invoke(this, new SizeChangedEventArgs(_form.Width, _form.Height));
        _form.Move += (sender, e) => PositionChanged?.Invoke(this, new PositionChangedEventArgs(_form.Left, _form.Top));
        _form.FormClosing += (sender, e) =>
        {
            var args = new WindowClosingEventArgs(WindowCloseReason.UserClosing);
            Closing?.Invoke(this, args);
            e.Cancel = args.Cancel;
        };
    }

    /// <inheritdoc/>
    public string Title
    {
        get => _form.Text;
        set => _form.Text = value;
    }

    /// <inheritdoc/>
    public int Width
    {
        get => _form.Width;
        set => _form.Width = value;
    }

    /// <inheritdoc/>
    public int Height
    {
        get => _form.Height;
        set => _form.Height = value;
    }

    /// <inheritdoc/>
    public int Left
    {
        get => _form.Left;
        set => _form.Left = value;
    }

    /// <inheritdoc/>
    public int Top
    {
        get => _form.Top;
        set => _form.Top = value;
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _form.Visible;
        set => _form.Visible = value;
    }

    /// <inheritdoc/>
    public bool Enabled
    {
        get => _form.Enabled;
        set => _form.Enabled = value;
    }

    /// <inheritdoc/>
    public WindowState State
    {
        get => _form.WindowState switch
        {
            System.Windows.Forms.FormWindowState.Normal => WindowState.Normal,
            System.Windows.Forms.FormWindowState.Minimized => WindowState.Minimized,
            System.Windows.Forms.FormWindowState.Maximized => WindowState.Maximized,
            _ => WindowState.Normal
        };
        set => _form.WindowState = value switch
        {
            WindowState.Normal => System.Windows.Forms.FormWindowState.Normal,
            WindowState.Minimized => System.Windows.Forms.FormWindowState.Minimized,
            WindowState.Maximized => System.Windows.Forms.FormWindowState.Maximized,
            _ => System.Windows.Forms.FormWindowState.Normal
        };
    }

    /// <inheritdoc/>
    public bool Resizable
    {
        get => _form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedDialog &&
               _form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedSingle &&
               _form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
               _form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.None;
        set
        {
            if (!value)
            {
                _form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            }
            else if (_form.FormBorderStyle == System.Windows.Forms.FormBorderStyle.FixedDialog)
            {
                _form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }
    }

    /// <inheritdoc/>
    public bool Minimizable
    {
        get => _form.MinimizeBox;
        set => _form.MinimizeBox = value;
    }

    /// <inheritdoc/>
    public bool Maximizable
    {
        get => _form.MaximizeBox;
        set => _form.MaximizeBox = value;
    }

    /// <inheritdoc/>
    public WindowStartPosition StartPosition
    {
        get => _form.StartPosition switch
        {
            System.Windows.Forms.FormStartPosition.Manual => WindowStartPosition.Manual,
            System.Windows.Forms.FormStartPosition.CenterScreen => WindowStartPosition.CenterScreen,
            System.Windows.Forms.FormStartPosition.CenterParent => WindowStartPosition.CenterParent,
            System.Windows.Forms.FormStartPosition.WindowsDefaultLocation => WindowStartPosition.WindowsDefaultLocation,
            System.Windows.Forms.FormStartPosition.WindowsDefaultBounds => WindowStartPosition.WindowsDefaultBounds,
            _ => WindowStartPosition.WindowsDefaultLocation
        };
        set => _form.StartPosition = value switch
        {
            WindowStartPosition.Manual => System.Windows.Forms.FormStartPosition.Manual,
            WindowStartPosition.CenterScreen => System.Windows.Forms.FormStartPosition.CenterScreen,
            WindowStartPosition.CenterParent => System.Windows.Forms.FormStartPosition.CenterParent,
            WindowStartPosition.WindowsDefaultLocation => System.Windows.Forms.FormStartPosition.WindowsDefaultLocation,
            WindowStartPosition.WindowsDefaultBounds => System.Windows.Forms.FormStartPosition.WindowsDefaultBounds,
            _ => System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
        };
    }

    /// <inheritdoc/>
    public int MinimumWidth
    {
        get => _form.MinimumSize.Width;
        set => _form.MinimumSize = new System.Drawing.Size(value, _form.MinimumSize.Height);
    }

    /// <inheritdoc/>
    public int MinimumHeight
    {
        get => _form.MinimumSize.Height;
        set => _form.MinimumSize = new System.Drawing.Size(_form.MinimumSize.Width, value);
    }

    /// <inheritdoc/>
    public int MaximumWidth
    {
        get => _form.MaximumSize.Width == 0 ? 0 : _form.MaximumSize.Width;
        set => _form.MaximumSize = new System.Drawing.Size(value == 0 ? 0 : value, _form.MaximumSize.Height);
    }

    /// <inheritdoc/>
    public int MaximumHeight
    {
        get => _form.MaximumSize.Height == 0 ? 0 : _form.MaximumSize.Height;
        set => _form.MaximumSize = new System.Drawing.Size(_form.MaximumSize.Width, value == 0 ? 0 : value);
    }

    /// <inheritdoc/>
    public void Show()
    {
        _form.Show();
    }

    /// <inheritdoc/>
    public void Hide()
    {
        _form.Hide();
    }

    /// <inheritdoc/>
    public void Close()
    {
        _form.Close();
    }

    /// <inheritdoc/>
    public void Center()
    {
        _form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
    }

    /// <inheritdoc/>
    public void Activate()
    {
        _form.Activate();
    }

    /// <inheritdoc/>
    public event EventHandler? Closed;

    /// <inheritdoc/>
    public event EventHandler? Shown;

    /// <inheritdoc/>
    public event EventHandler<SizeChangedEventArgs>? SizeChanged;

    /// <inheritdoc/>
    public event EventHandler<PositionChangedEventArgs>? PositionChanged;

    /// <inheritdoc/>
    public event EventHandler<WindowClosingEventArgs>? Closing;
}


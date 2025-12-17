using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the UI provider interface.
/// This is the default UI provider for ClassicPanel.
/// </summary>
public class WinFormsUIProvider : IUIProvider
{
    private static readonly string FrameworkNameValue = "WinForms";
    private static readonly string VersionValue = "0.1.0";

    /// <inheritdoc/>
    public string FrameworkName => FrameworkNameValue;

    /// <inheritdoc/>
    public string Version => VersionValue;

    /// <inheritdoc/>
    public IWindow CreateWindow()
    {
        return new WinFormsWindow();
    }

    /// <inheritdoc/>
    public IListView CreateListView()
    {
        return new WinFormsListView();
    }

    /// <inheritdoc/>
    public IMenuBar CreateMenuBar()
    {
        return new WinFormsMenuBar();
    }

    /// <inheritdoc/>
    public ICommandBar CreateCommandBar()
    {
        return new WinFormsCommandBar();
    }

    /// <inheritdoc/>
    public IStatusBar CreateStatusBar()
    {
        return new WinFormsStatusBar();
    }

    /// <inheritdoc/>
    public ITreeView CreateTreeView()
    {
        return new WinFormsTreeView();
    }

    /// <inheritdoc/>
    public IDialog CreateDialog()
    {
        return new WinFormsDialog();
    }

    /// <inheritdoc/>
    public IContextMenu CreateContextMenu()
    {
        return new WinFormsContextMenu();
    }

    /// <inheritdoc/>
    public void Run()
    {
        System.Windows.Forms.Application.Run();
    }

    /// <inheritdoc/>
    public void Run(IWindow mainWindow)
    {
        if (mainWindow is WinFormsWindow winFormsWindow)
        {
            System.Windows.Forms.Application.Run(winFormsWindow.Form);
        }
        else
        {
            throw new ArgumentException("Window must be a WinFormsWindow instance.", nameof(mainWindow));
        }
    }

    /// <inheritdoc/>
    public void Exit()
    {
        System.Windows.Forms.Application.Exit();
    }

    /// <inheritdoc/>
    public nint GetWindowHandle(IWindow window)
    {
        if (window is WinFormsWindow winFormsWindow)
        {
            return winFormsWindow.Form.Handle;
        }
        return nint.Zero;
    }
}


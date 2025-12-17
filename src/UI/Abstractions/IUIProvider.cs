namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents a factory interface for creating UI components in a framework-agnostic manner.
/// This is the main entry point for creating UI elements that work across different frameworks.
/// </summary>
public interface IUIProvider
{
    /// <summary>
    /// Gets the name of the UI framework (e.g., "WinForms", "WPF").
    /// </summary>
    string FrameworkName { get; }

    /// <summary>
    /// Gets the version of the UI framework implementation.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Creates a new window.
    /// </summary>
    /// <returns>A new window instance.</returns>
    IWindow CreateWindow();

    /// <summary>
    /// Creates a new list view.
    /// </summary>
    /// <returns>A new list view instance.</returns>
    IListView CreateListView();

    /// <summary>
    /// Creates a new menu bar.
    /// </summary>
    /// <returns>A new menu bar instance.</returns>
    IMenuBar CreateMenuBar();

    /// <summary>
    /// Creates a new command bar (toolbar).
    /// </summary>
    /// <returns>A new command bar instance.</returns>
    ICommandBar CreateCommandBar();

    /// <summary>
    /// Creates a new status bar.
    /// </summary>
    /// <returns>A new status bar instance.</returns>
    IStatusBar CreateStatusBar();

    /// <summary>
    /// Creates a new tree view.
    /// </summary>
    /// <returns>A new tree view instance.</returns>
    ITreeView CreateTreeView();

    /// <summary>
    /// Creates a new dialog window.
    /// </summary>
    /// <returns>A new dialog instance.</returns>
    IDialog CreateDialog();

    /// <summary>
    /// Creates a new context menu.
    /// </summary>
    /// <returns>A new context menu instance.</returns>
    IContextMenu CreateContextMenu();

    /// <summary>
    /// Runs the application message loop.
    /// This should be called from the main entry point.
    /// </summary>
    void Run();

    /// <summary>
    /// Runs the application message loop with a main window.
    /// </summary>
    /// <param name="mainWindow">The main window to display.</param>
    void Run(IWindow mainWindow);

    /// <summary>
    /// Exits the application message loop.
    /// </summary>
    void Exit();

    /// <summary>
    /// Gets the native window handle from an IWindow instance.
    /// This is useful for P/Invoke operations that require a window handle.
    /// </summary>
    /// <param name="window">The window to get the handle from.</param>
    /// <returns>The native window handle, or nint.Zero if not available.</returns>
    nint GetWindowHandle(IWindow window);
}


namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract window interface for framework-agnostic window operations.
/// </summary>
public interface IWindow
{
    /// <summary>
    /// Gets or sets the window title.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Gets or sets the window width in pixels.
    /// </summary>
    int Width { get; set; }

    /// <summary>
    /// Gets or sets the window height in pixels.
    /// </summary>
    int Height { get; set; }

    /// <summary>
    /// Gets or sets the window's left position in pixels.
    /// </summary>
    int Left { get; set; }

    /// <summary>
    /// Gets or sets the window's top position in pixels.
    /// </summary>
    int Top { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the window state (normal, minimized, maximized).
    /// </summary>
    WindowState State { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be resized.
    /// </summary>
    bool Resizable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be minimized.
    /// </summary>
    bool Minimizable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be maximized.
    /// </summary>
    bool Maximizable { get; set; }

    /// <summary>
    /// Gets or sets the starting position of the window.
    /// </summary>
    WindowStartPosition StartPosition { get; set; }

    /// <summary>
    /// Gets or sets the minimum width of the window in pixels.
    /// </summary>
    int MinimumWidth { get; set; }

    /// <summary>
    /// Gets or sets the minimum height of the window in pixels.
    /// </summary>
    int MinimumHeight { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of the window in pixels, or 0 for no limit.
    /// </summary>
    int MaximumWidth { get; set; }

    /// <summary>
    /// Gets or sets the maximum height of the window in pixels, or 0 for no limit.
    /// </summary>
    int MaximumHeight { get; set; }

    /// <summary>
    /// Shows the window.
    /// </summary>
    void Show();

    /// <summary>
    /// Hides the window.
    /// </summary>
    void Hide();

    /// <summary>
    /// Closes the window.
    /// </summary>
    void Close();

    /// <summary>
    /// Centers the window on the screen.
    /// </summary>
    void Center();

    /// <summary>
    /// Activates the window and brings it to the foreground.
    /// </summary>
    void Activate();

    /// <summary>
    /// Occurs when the window is closed.
    /// </summary>
    event EventHandler? Closed;

    /// <summary>
    /// Occurs when the window is shown.
    /// </summary>
    event EventHandler? Shown;

    /// <summary>
    /// Occurs when the window is resized.
    /// </summary>
    event EventHandler<SizeChangedEventArgs>? SizeChanged;

    /// <summary>
    /// Occurs when the window is moved.
    /// </summary>
    event EventHandler<PositionChangedEventArgs>? PositionChanged;

    /// <summary>
    /// Occurs when the window is about to close. This event can be canceled.
    /// </summary>
    event EventHandler<WindowClosingEventArgs>? Closing;
}

/// <summary>
/// Represents the starting position of a window.
/// </summary>
public enum WindowStartPosition
{
    /// <summary>
    /// The window position is determined by the Left and Top properties.
    /// </summary>
    Manual = 0,

    /// <summary>
    /// The window is centered on the current display.
    /// </summary>
    CenterScreen = 1,

    /// <summary>
    /// The window is centered on the parent window, or centered on the screen if no parent.
    /// </summary>
    CenterParent = 2,

    /// <summary>
    /// The window position is determined by Windows default location.
    /// </summary>
    WindowsDefaultLocation = 3,

    /// <summary>
    /// The window position and size are determined by Windows default bounds.
    /// </summary>
    WindowsDefaultBounds = 4
}

/// <summary>
/// Provides data for window closing events that can be canceled.
/// </summary>
public class WindowClosingEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether the window close should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Gets the reason for closing the window.
    /// </summary>
    public WindowCloseReason CloseReason { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowClosingEventArgs"/> class.
    /// </summary>
    /// <param name="closeReason">The reason for closing the window.</param>
    public WindowClosingEventArgs(WindowCloseReason closeReason)
    {
        CloseReason = closeReason;
    }
}

/// <summary>
/// Represents the reason a window is being closed.
/// </summary>
public enum WindowCloseReason
{
    /// <summary>
    /// The user is closing the window.
    /// </summary>
    UserClosing = 0,

    /// <summary>
    /// The application is exiting.
    /// </summary>
    ApplicationExit = 1,

    /// <summary>
    /// Windows is shutting down.
    /// </summary>
    WindowsShutDown = 2,

    /// <summary>
    /// The parent form is closing.
    /// </summary>
    ParentFormClosing = 3,

    /// <summary>
    /// The close is being triggered programmatically.
    /// </summary>
    Programmatic = 4
}

/// <summary>
/// Represents the state of a window.
/// </summary>
public enum WindowState
{
    /// <summary>
    /// Normal window state.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Minimized window state.
    /// </summary>
    Minimized = 1,

    /// <summary>
    /// Maximized window state.
    /// </summary>
    Maximized = 2
}

/// <summary>
/// Provides data for window size change events.
/// </summary>
public class SizeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the new width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the new height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SizeChangedEventArgs"/> class.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    public SizeChangedEventArgs(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

/// <summary>
/// Provides data for window position change events.
/// </summary>
public class PositionChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the new left position.
    /// </summary>
    public int Left { get; }

    /// <summary>
    /// Gets the new top position.
    /// </summary>
    public int Top { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionChangedEventArgs"/> class.
    /// </summary>
    /// <param name="left">The new left position.</param>
    /// <param name="top">The new top position.</param>
    public PositionChangedEventArgs(int left, int top)
    {
        Left = left;
        Top = top;
    }
}


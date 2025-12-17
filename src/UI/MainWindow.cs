using ClassicPanel.Core;
using ClassicPanel.Core.Performance;
using ClassicPanel.Core.Theme;
using ClassicPanel.Icons;
using static ClassicPanel.Core.Theme.WindowsThemeInterop;

namespace ClassicPanel.UI;

public partial class MainWindow : Form
{
    private ToolStripButton? _themeToggleButton;

    public MainWindow()
    {
        InitializeComponent();
        InitializeMenu();
        InitializeToolbar();
        ApplyTheme();
        
        // Subscribe to theme changes
        ThemeManager.OnThemeChanged += (effectiveTheme) => 
        {
            ApplyTheme();
            UpdateThemeToggleButton();
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
        // Apply title bar theme now that handle is created
        ApplyTitleBarTheme();
        
        // Mark startup complete when window is loaded
        PerformanceMonitor.MarkStartupComplete();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        
        // Apply title bar theme when handle is created
        ApplyTitleBarTheme();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        
        // Apply title bar theme again when window is shown (ensures it's applied)
        // Use BeginInvoke to ensure window is fully rendered first
        this.BeginInvoke(new Action(() =>
        {
            ApplyTitleBarTheme();
        }));
    }

    /// <summary>
    /// Applies the current theme to the window and controls.
    /// </summary>
    private void ApplyTheme()
    {
        var theme = ThemeManager.CurrentThemeData;
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        
        // Apply theme to form
        this.BackColor = theme.BackgroundColor;
        this.ForeColor = theme.ForegroundColor;
        
        // Apply title bar theme
        ApplyTitleBarTheme();
        
        // Apply theme to all controls
        ApplyThemeToControls(this, theme);
        
        // Update toolbar icons with new theme colors
        UpdateToolbarIcons(isDarkMode);
    }

    /// <summary>
    /// Updates toolbar icons to match the current theme.
    /// </summary>
    private void UpdateToolbarIcons(bool isDarkMode)
    {
        foreach (ToolStripItem item in toolStrip.Items)
        {
            if (item is ToolStripButton button && button.Tag is string svgPath)
            {
                // Re-render icon with new theme
                var newIcon = SvgIconRenderer.RenderSvgPathThemed(svgPath, 16, isDarkMode);
                button.Image = newIcon;
            }
        }
    }

    /// <summary>
    /// Applies dark mode to the title bar (non-client area) using DWM.
    /// </summary>
    private void ApplyTitleBarTheme()
    {
        if (!this.IsHandleCreated || this.Handle == nint.Zero)
            return;

        var effectiveTheme = ThemeManager.GetEffectiveTheme();
        bool isDarkMode = string.Equals(effectiveTheme, AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        
        // Apply dark mode to title bar
        bool success = SetWindowTitleBarTheme(this.Handle, isDarkMode);
        
        #if DEBUG
        if (!success)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to apply title bar theme. Dark mode: {isDarkMode}, Handle: {this.Handle}, EffectiveTheme: {effectiveTheme}");
        }
        #endif
        
        // Force window to refresh non-client area using SetWindowPos
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_FRAMECHANGED = 0x0020;
        SetWindowPos(this.Handle, nint.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    /// <summary>
    /// Recursively applies theme to all controls in the form.
    /// </summary>
    private void ApplyThemeToControls(Control parent, ThemeData theme)
    {
        foreach (Control control in parent.Controls)
        {
            // Apply theme based on control type
            if (control is Button button)
            {
                button.BackColor = theme.ControlBackgroundColor;
                button.ForeColor = theme.ControlForegroundColor;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = theme.BorderColor;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = theme.ControlBackgroundColor;
                textBox.ForeColor = theme.ControlForegroundColor;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is Label label)
            {
                label.BackColor = theme.BackgroundColor;
                label.ForeColor = theme.ForegroundColor;
            }
            else if (control is ListView listView)
            {
                listView.BackColor = theme.BackgroundColor;
                listView.ForeColor = theme.ForegroundColor;
            }
            else if (control is MenuStrip menuStrip)
            {
                menuStrip.BackColor = theme.ControlBackgroundColor;
                menuStrip.ForeColor = theme.ControlForegroundColor;
            }
            else if (control is ToolStrip toolStrip)
            {
                toolStrip.BackColor = theme.ControlBackgroundColor;
                toolStrip.ForeColor = theme.ControlForegroundColor;
            }
            else
            {
                // Apply default colors to other controls
                control.BackColor = theme.BackgroundColor;
                control.ForeColor = theme.ForegroundColor;
            }

            // Recursively apply to child controls
            if (control.HasChildren)
            {
                ApplyThemeToControls(control, theme);
            }
        }
    }

    /// <summary>
    /// Initializes the menu system.
    /// </summary>
    private void InitializeMenu()
    {
        // File menu
        var fileMenu = new ToolStripMenuItem("&File");
        fileMenu.DropDownItems.Add(new ToolStripMenuItem("&Refresh", null, (s, e) => RefreshItems(), Keys.F5));
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add(new ToolStripMenuItem("E&xit", null, (s, e) => Close(), Keys.Alt | Keys.F4));
        menuStrip.Items.Add(fileMenu);

        // View menu
        var viewMenu = new ToolStripMenuItem("&View");
        viewMenu.DropDownItems.Add(new ToolStripMenuItem("&Large Icons", null, (s, e) => SetViewMode("LargeIcons")));
        viewMenu.DropDownItems.Add(new ToolStripMenuItem("&Small Icons", null, (s, e) => SetViewMode("SmallIcons")));
        viewMenu.DropDownItems.Add(new ToolStripMenuItem("&List", null, (s, e) => SetViewMode("List")));
        viewMenu.DropDownItems.Add(new ToolStripMenuItem("&Details", null, (s, e) => SetViewMode("Details")));
        menuStrip.Items.Add(viewMenu);

        // Tools menu
        var toolsMenu = new ToolStripMenuItem("&Tools");
        toolsMenu.DropDownItems.Add(new ToolStripMenuItem("&Settings...", null, (s, e) => ShowSettingsDialog(), Keys.Control | Keys.Oemcomma));
        menuStrip.Items.Add(toolsMenu);

        // Help menu
        var helpMenu = new ToolStripMenuItem("&Help");
        helpMenu.DropDownItems.Add(new ToolStripMenuItem("&About ClassicPanel...", null, (s, e) => ShowAboutDialog()));
        menuStrip.Items.Add(helpMenu);
    }

    /// <summary>
    /// Initializes the toolbar with SVG icons.
    /// </summary>
    private void InitializeToolbar()
    {
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);

        // Refresh button
        var refreshIcon = SvgIconRenderer.RenderSvgPathThemed(SvgIcons.Refresh, 16, isDarkMode);
        var refreshButton = new ToolStripButton("Refresh", refreshIcon, (s, e) => RefreshItems())
        {
            ToolTipText = "Refresh (F5)",
            Tag = SvgIcons.Refresh // Store SVG path for theme updates
        };
        toolStrip.Items.Add(refreshButton);

        toolStrip.Items.Add(new ToolStripSeparator());

        // View mode buttons
        var largeIconsIcon = SvgIconRenderer.RenderSvgPathThemed(SvgIcons.LargeIcons, 16, isDarkMode);
        var largeIconsButton = new ToolStripButton("Large Icons", largeIconsIcon, (s, e) => SetViewMode("LargeIcons"))
        {
            ToolTipText = "Large Icons",
            Tag = SvgIcons.LargeIcons
        };
        toolStrip.Items.Add(largeIconsButton);

        var smallIconsIcon = SvgIconRenderer.RenderSvgPathThemed(SvgIcons.SmallIcons, 16, isDarkMode);
        var smallIconsButton = new ToolStripButton("Small Icons", smallIconsIcon, (s, e) => SetViewMode("SmallIcons"))
        {
            ToolTipText = "Small Icons",
            Tag = SvgIcons.SmallIcons
        };
        toolStrip.Items.Add(smallIconsButton);

        var listIcon = SvgIconRenderer.RenderSvgPathThemed(SvgIcons.List, 16, isDarkMode);
        var listButton = new ToolStripButton("List", listIcon, (s, e) => SetViewMode("List"))
        {
            ToolTipText = "List",
            Tag = SvgIcons.List
        };
        toolStrip.Items.Add(listButton);

        var detailsIcon = SvgIconRenderer.RenderSvgPathThemed(SvgIcons.Details, 16, isDarkMode);
        var detailsButton = new ToolStripButton("Details", detailsIcon, (s, e) => SetViewMode("Details"))
        {
            ToolTipText = "Details",
            Tag = SvgIcons.Details
        };
        toolStrip.Items.Add(detailsButton);

        toolStrip.Items.Add(new ToolStripSeparator());

        // Theme toggle button (only visible when system theme is disabled)
        UpdateThemeToggleButton();
    }

    /// <summary>
    /// Updates the theme toggle button visibility and icon based on current theme.
    /// </summary>
    private void UpdateThemeToggleButton()
    {
        var isSystemTheme = string.Equals(ThemeManager.CurrentTheme, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase);
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);

        if (_themeToggleButton == null)
        {
            // Create theme toggle button
            var themeIcon = SvgIconRenderer.RenderSvgPathThemed(
                isDarkMode ? SvgIcons.LightMode : SvgIcons.DarkMode, 
                16, 
                isDarkMode
            );
            _themeToggleButton = new ToolStripButton(
                isDarkMode ? "Light Mode" : "Dark Mode",
                themeIcon,
                (s, e) => ToggleTheme()
            )
            {
                ToolTipText = isDarkMode ? "Switch to Light Mode" : "Switch to Dark Mode",
                Tag = isDarkMode ? SvgIcons.LightMode : SvgIcons.DarkMode // Store SVG path for theme updates
            };
            toolStrip.Items.Add(_themeToggleButton);
        }

        // Only show when system theme is NOT enabled
        _themeToggleButton.Visible = !isSystemTheme;

        if (!isSystemTheme)
        {
            // Update icon and text
            var svgPath = isDarkMode ? SvgIcons.LightMode : SvgIcons.DarkMode;
            var newIcon = SvgIconRenderer.RenderSvgPathThemed(svgPath, 16, isDarkMode);
            _themeToggleButton.Image = newIcon;
            _themeToggleButton.Tag = svgPath;
            _themeToggleButton.Text = isDarkMode ? "Light Mode" : "Dark Mode";
            _themeToggleButton.ToolTipText = isDarkMode ? "Switch to Light Mode" : "Switch to Dark Mode";
        }
    }

    /// <summary>
    /// Toggles between light and dark theme.
    /// </summary>
    private void ToggleTheme()
    {
        var currentTheme = ThemeManager.CurrentTheme;
        if (string.Equals(currentTheme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
        {
            ThemeManager.CurrentTheme = AppConstants.DarkTheme;
        }
        else
        {
            ThemeManager.CurrentTheme = AppConstants.LightTheme;
        }
    }

    /// <summary>
    /// Refreshes the Control Panel items list.
    /// </summary>
    private void RefreshItems()
    {
        // TODO: Implement refresh logic
        System.Diagnostics.Debug.WriteLine("Refresh clicked");
    }

    /// <summary>
    /// Sets the view mode.
    /// </summary>
    private void SetViewMode(string mode)
    {
        // TODO: Implement view mode switching
        System.Diagnostics.Debug.WriteLine($"View mode: {mode}");
    }

    /// <summary>
    /// Shows the settings dialog.
    /// </summary>
    private void ShowSettingsDialog()
    {
        using var settingsDialog = new SettingsDialog();
        settingsDialog.ShowDialog(this);
    }

    /// <summary>
    /// Shows the about dialog.
    /// </summary>
    private void ShowAboutDialog()
    {
        // TODO: Implement about dialog
        MessageBox.Show(
            $"ClassicPanel\n\nVersion: {VersionInfo.Version}\n\nCopyright © 2025 Rizonetech (Pty) Ltd",
            "About ClassicPanel",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }
}


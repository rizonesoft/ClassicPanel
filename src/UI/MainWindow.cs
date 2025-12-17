using ClassicPanel.Core;
using ClassicPanel.Core.Performance;
using ClassicPanel.Core.Theme;
using static ClassicPanel.Core.Theme.WindowsThemeInterop;

namespace ClassicPanel.UI;

public partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
        ApplyTheme();
        
        // Subscribe to theme changes
        ThemeManager.OnThemeChanged += (effectiveTheme) => ApplyTheme();
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
        ApplyTitleBarTheme();
    }

    /// <summary>
    /// Applies the current theme to the window and controls.
    /// </summary>
    private void ApplyTheme()
    {
        var theme = ThemeManager.CurrentThemeData;
        
        // Apply theme to form
        this.BackColor = theme.BackgroundColor;
        this.ForeColor = theme.ForegroundColor;
        
        // Apply title bar theme
        ApplyTitleBarTheme();
        
        // Apply theme to all controls
        ApplyThemeToControls(this, theme);
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
        SetWindowTitleBarTheme(this.Handle, isDarkMode);
        
        // Force window to refresh non-client area
        if (this.Visible)
        {
            // Invalidate the non-client area to force redraw
            this.Invalidate(true);
        }
    }

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
}


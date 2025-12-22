using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// Custom ToolStrip control with improved styling for dark/light modes.
/// Fixes white borders, separator appearance, and removes drag handle.
/// </summary>
public class ModernToolStrip : ToolStrip
{
    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_FRAMECHANGED = 0x0020;
    private const int WS_EX_CLIENTEDGE = 0x200;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModernToolStrip"/> class.
    /// </summary>
    public ModernToolStrip()
    {
        // Remove drag handle (grip)
        this.GripStyle = ToolStripGripStyle.Hidden;
        
        // Set renderer
        this.Renderer = new ModernToolStripRenderer();
        
        // Configure appearance
        this.AutoSize = true;
        this.CanOverflow = false;
        this.ShowItemToolTips = true;
        
        // Add padding: left=8px, top=5px, right=0px, bottom=5px
        this.Padding = new Padding(8, 5, 0, 5);
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            // Remove client edge style to prevent border
            cp.ExStyle &= ~WS_EX_CLIENTEDGE;
            return cp;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (this.IsHandleCreated)
        {
            // Force window to recalculate non-client area
            SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Determine if we're in dark mode or light mode
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        // Toolbar background: #191919 for dark mode, #FFFFFF for light mode
        var toolbarBackground = isDarkMode
            ? Color.FromArgb(0x19, 0x19, 0x19)  // Dark mode: #191919
            : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: #FFFFFF

        e.Graphics.Clear(toolbarBackground);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Paint background first
        OnPaintBackground(e);
        
        // Call base to render items (buttons, separators, etc.)
        base.OnPaint(e);
        
        // Draw custom border (only bottom border, no right border) on top
        var theme = ClassicPanel.Core.Theme.ThemeManager.CurrentThemeData;
        using (var pen = new Pen(theme.BorderColor, 1))
        {
            // Only draw bottom border - no right border
            var bottomY = this.Height - 1;
            e.Graphics.DrawLine(pen, 0, bottomY, this.Width, bottomY);
        }
    }

    protected override void WndProc(ref Message m)
    {
        // Intercept WM_NCPAINT to prevent default non-client area border rendering
        const int WM_NCPAINT = 0x0085;
        
        if (m.Msg == WM_NCPAINT)
        {
            // Don't process non-client area paint (prevents right border)
            m.Result = IntPtr.Zero;
            return;
        }
        
        base.WndProc(ref m);
    }

    protected override void SetVisibleCore(bool value)
    {
        base.SetVisibleCore(value);
        if (value)
        {
            // Force repaint when made visible to ensure proper rendering
            this.Invalidate();
        }
    }
}


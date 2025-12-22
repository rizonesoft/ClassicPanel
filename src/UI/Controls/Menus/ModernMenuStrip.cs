using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// Custom MenuStrip control with dark/light mode support.
/// Provides consistent styling with the toolbar.
/// </summary>
public class ModernMenuStrip : MenuStrip
{
    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_FRAMECHANGED = 0x0020;
    private const int WS_EX_CLIENTEDGE = 0x200;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModernMenuStrip"/> class.
    /// </summary>
    public ModernMenuStrip()
    {
        // Set renderer for dark/light mode support
        this.Renderer = new ModernMenuStripRenderer();
        
        // Configure appearance
        this.AutoSize = true;
        this.ShowItemToolTips = false;
        
        // Add padding: left=10px to align with toolbar icons, top=3px, right=0px, bottom=3px
        this.Padding = new Padding(10, 3, 0, 3);
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

        // Menu background: #191919 for dark mode, #FFFFFF for light mode
        var menuBackground = isDarkMode
            ? Color.FromArgb(0x19, 0x19, 0x19)  // Dark mode: #191919
            : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: #FFFFFF

        e.Graphics.Clear(menuBackground);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Paint background first
        OnPaintBackground(e);
        
        // Call base to render menu items
        base.OnPaint(e);
        
        // Draw custom border (only bottom border)
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        var borderColor = isDarkMode
            ? Color.FromArgb(0x3C, 0x3C, 0x3C)  // Dark mode border
            : Color.FromArgb(0xE0, 0xE0, 0xE0); // Light mode border

        using (var pen = new Pen(borderColor, 1))
        {
            // Only draw bottom border
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
            // Don't process non-client area paint (prevents unwanted borders)
            m.Result = IntPtr.Zero;
            return;
        }
        
        base.WndProc(ref m);
    }
}


using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI;

/// <summary>
/// Modern toolstrip renderer with improved styling and hover effects.
/// </summary>
public class ModernToolStripRenderer : ToolStripProfessionalRenderer
{
    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        var button = e.Item as ToolStripButton;
        if (button == null || !button.Enabled)
        {
            base.OnRenderButtonBackground(e);
            return;
        }

        var rect = new Rectangle(Point.Empty, e.Item.Size);
        const int borderRadius = 2; // 2px border radius

        if (button.Selected || button.Pressed)
        {
            // Determine if we're in dark mode or light mode
            var isDarkMode = string.Equals(
                ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
                ClassicPanel.Core.AppConstants.DarkTheme,
                StringComparison.OrdinalIgnoreCase);

            // Button hover background: #282828 for dark mode, #F6F6F6 for light mode
            var hoverBackground = isDarkMode
                ? Color.FromArgb(0x28, 0x28, 0x28)  // Dark mode: #282828
                : Color.FromArgb(0xF6, 0xF6, 0xF6); // Light mode: #F6F6F6

            using (var brush = new SolidBrush(hoverBackground))
            {
                // Fill with rounded corners
                e.Graphics.FillRoundedRectangle(brush, rect, borderRadius);
            }
        }
    }

    protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
    {
        // Use default rendering - no color change on hover
        base.OnRenderItemImage(e);
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
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
        
        // Don't draw default borders - we handle this in the custom control
        // The background is already cleared, so no additional rendering needed
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Don't render the default border - we handle this in ModernToolStrip.OnPaint
        // This prevents the white border on the right and bottom
        // Completely override to prevent any default border rendering
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
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

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        var theme = ClassicPanel.Core.Theme.ThemeManager.CurrentThemeData;
        var rect = e.Item.ContentRectangle;
        
        // Use a more subtle separator color that works in both light and dark modes
        var separatorColor = theme.BorderColor;
        
        // Adjust opacity for better visibility in dark mode
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(), 
            ClassicPanel.Core.AppConstants.DarkTheme, 
            StringComparison.OrdinalIgnoreCase);
        
        if (isDarkMode)
        {
            // Use a lighter, more visible color for dark mode separators
            separatorColor = Color.FromArgb(60, 60, 60);
        }
        else
        {
            // Use a darker color for light mode separators
            separatorColor = Color.FromArgb(200, 200, 200);
        }
        
        using (var pen = new Pen(separatorColor, 1))
        {
            // Center the separator vertically with proper margins (spacing already handled by Margin property)
            var centerX = rect.Left + rect.Width / 2;
            var top = rect.Top + 8; // Top margin
            var bottom = rect.Bottom - 8; // Bottom margin
            e.Graphics.DrawLine(pen, centerX, top, centerX, bottom);
        }
    }

    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
        // Don't render the grip/drag handle at all
        // This is handled by setting GripStyle = Hidden, but we override here to be sure
    }
}

/// <summary>
/// Extension methods for drawing rounded rectangles.
/// </summary>
public static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle rect, int radius)
    {
        using (var path = CreateRoundedRectanglePath(rect, radius))
        {
            graphics.FillPath(brush, path);
        }
    }

    public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rect, int radius)
    {
        using (var path = CreateRoundedRectanglePath(rect, radius))
        {
            graphics.DrawPath(pen, path);
        }
    }

    private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        var diameter = radius * 2;

        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        return path;
    }
}


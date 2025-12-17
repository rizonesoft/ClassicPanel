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
        var theme = ClassicPanel.Core.Theme.ThemeManager.CurrentThemeData;

        if (button.Selected || button.Pressed)
        {
            using (var brush = new SolidBrush(theme.HoverBackgroundColor))
            {
                e.Graphics.FillRoundedRectangle(brush, rect, 4);
            }
        }

        // Draw border on hover
        if (button.Selected && !button.Pressed)
        {
            using (var pen = new Pen(theme.BorderColor, 1))
            {
                e.Graphics.DrawRoundedRectangle(pen, rect, 4);
            }
        }
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        var theme = ClassicPanel.Core.Theme.ThemeManager.CurrentThemeData;
        e.Graphics.Clear(theme.ControlBackgroundColor);
        
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
        // Override to prevent default panel background rendering
        var theme = ClassicPanel.Core.Theme.ThemeManager.CurrentThemeData;
        e.Graphics.Clear(theme.ControlBackgroundColor);
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
            // Center the separator vertically with proper margins
            var centerX = rect.Left + rect.Width / 2;
            var top = rect.Top + 6;
            var bottom = rect.Bottom - 6;
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


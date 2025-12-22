using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// Fully owner-drawn ToolStrip renderer with dark/light mode support.
/// Does NOT call any base methods to ensure complete control over appearance.
/// </summary>
public class ModernToolStripRenderer : ToolStripProfessionalRenderer
{
    private bool IsDarkMode => string.Equals(
        ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
        ClassicPanel.Core.AppConstants.DarkTheme,
        StringComparison.OrdinalIgnoreCase);

    // ===== Color Properties =====
    private Color ToolbarBackgroundColor => IsDarkMode
        ? Color.FromArgb(0x19, 0x19, 0x19)  // Dark mode: #191919
        : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: white

    private Color HoverBackgroundColor => IsDarkMode
        ? Color.FromArgb(0x28, 0x28, 0x28)  // Dark mode: #282828
        : Color.FromArgb(0xF6, 0xF6, 0xF6); // Light mode: #F6F6F6

    private Color SeparatorColor => IsDarkMode
        ? Color.FromArgb(0x3C, 0x3C, 0x3C)  // Dark mode: #3C3C3C
        : Color.FromArgb(0xC8, 0xC8, 0xC8); // Light mode: #C8C8C8

    private Color TextColor => IsDarkMode
        ? Color.FromArgb(0xFF, 0xFF, 0xFF)  // Dark mode: white
        : Color.FromArgb(0x00, 0x00, 0x00); // Light mode: black

    // ===== ToolStrip Background - FULLY OWNER DRAWN =====
    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        e.Graphics.Clear(ToolbarBackgroundColor);
        // No base call
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
        e.Graphics.Clear(ToolbarBackgroundColor);
        // No base call
    }

    protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
    {
        e.Graphics.Clear(ToolbarBackgroundColor);
        // No base call
    }

    // ===== Border - FULLY OWNER DRAWN =====
    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Don't render any border - handled by ModernToolStrip.OnPaint
        // No base call
    }

    // ===== Button Background - FULLY OWNER DRAWN =====
    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        var rect = new Rectangle(Point.Empty, e.Item.Size);
        const int borderRadius = 2;

        if (e.Item.Enabled && (e.Item.Selected || e.Item.Pressed))
        {
            // Hover state - draw rounded background
            using (var brush = new SolidBrush(HoverBackgroundColor))
            {
                e.Graphics.FillRoundedRectangle(brush, rect, borderRadius);
            }
        }
        // No background when not hovered - transparent
        // No base call
    }

    // ===== Dropdown Button Background - FULLY OWNER DRAWN =====
    protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
    {
        var rect = new Rectangle(Point.Empty, e.Item.Size);
        const int borderRadius = 2;

        if (e.Item.Enabled && (e.Item.Selected || e.Item.Pressed))
        {
            using (var brush = new SolidBrush(HoverBackgroundColor))
            {
                e.Graphics.FillRoundedRectangle(brush, rect, borderRadius);
            }
        }
        // No base call
    }

    // ===== Split Button Background - FULLY OWNER DRAWN =====
    protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
    {
        OnRenderDropDownButtonBackground(e);
        // No base call
    }

    // ===== Item Image - FULLY OWNER DRAWN =====
    protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
    {
        if (e.Image != null)
        {
            e.Graphics.DrawImage(e.Image, e.ImageRectangle);
        }
        // No base call
    }

    // ===== Item Text =====
    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = TextColor;
        base.OnRenderItemText(e); // Safe - just draws text
    }

    // ===== Separator - FULLY OWNER DRAWN =====
    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        // Fill background first
        var itemRect = new Rectangle(Point.Empty, e.Item.Size);
        using (var bgBrush = new SolidBrush(ToolbarBackgroundColor))
        {
            e.Graphics.FillRectangle(bgBrush, itemRect);
        }

        // Draw vertical separator line
        var rect = e.Item.ContentRectangle;
        using (var pen = new Pen(SeparatorColor, 1))
        {
            var centerX = rect.Left + rect.Width / 2;
            var top = rect.Top + 8;
            var bottom = rect.Bottom - 8;
            e.Graphics.DrawLine(pen, centerX, top, centerX, bottom);
        }
        // No base call
    }

    // ===== Grip - FULLY OWNER DRAWN =====
    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
        // Don't render grip
        // No base call
    }

    // ===== Overflow Button - FULLY OWNER DRAWN =====
    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
    {
        // Don't render overflow button background
        // No base call
    }

    // ===== Item Background - FULLY OWNER DRAWN =====
    protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
    {
        // No background - handled by specific item type methods
        // No base call
    }

    // ===== Item Check (for checked buttons) - FULLY OWNER DRAWN =====
    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
        var checkRect = e.ImageRectangle;
        using (var brush = new SolidBrush(HoverBackgroundColor))
        {
            e.Graphics.FillRectangle(brush, checkRect);
        }
        using (var font = new Font("Segoe UI", 9F))
        using (var textBrush = new SolidBrush(TextColor))
        {
            e.Graphics.DrawString("✓", font, textBrush, checkRect.X, checkRect.Y);
        }
        // No base call
    }

    // ===== Arrow - FULLY OWNER DRAWN =====
    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        e.ArrowColor = TextColor;
        base.OnRenderArrow(e); // Safe - just draws arrow
    }

    // ===== Label Background - FULLY OWNER DRAWN =====
    protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
    {
        // Transparent background
        // No base call
    }

    // ===== Status Strip Sizing Grip =====
    protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
    {
        // Don't render
        // No base call
    }

    // ===== Menu Item Background (for dropdown menus attached to toolbar) =====
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        // Delegate to MenuStripRenderer for dropdown menus
        // This shouldn't be called for toolbar items
        // No base call
    }

    // ===== Image Margin (for dropdown menus) =====
    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
        // Fill with toolbar background
        using (var brush = new SolidBrush(ToolbarBackgroundColor))
        {
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }
        // No base call
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

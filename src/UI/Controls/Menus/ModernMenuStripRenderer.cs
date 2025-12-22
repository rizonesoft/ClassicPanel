using System.Drawing;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// Fully owner-drawn MenuStrip renderer with dark/light mode support.
/// Does NOT call any base methods to ensure complete control over appearance.
/// </summary>
public class ModernMenuStripRenderer : ToolStripProfessionalRenderer
{
    private bool IsDarkMode => string.Equals(
        ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
        ClassicPanel.Core.AppConstants.DarkTheme,
        StringComparison.OrdinalIgnoreCase);

    // ===== Color Properties =====
    private Color MenuBackgroundColor => IsDarkMode
        ? Color.FromArgb(0x19, 0x19, 0x19)  // Dark mode: #191919
        : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: white

    private Color DropdownBackgroundColor => IsDarkMode
        ? Color.FromArgb(0x1E, 0x1E, 0x1E)  // Dark mode: #1E1E1E
        : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: white

    private Color MenuItemTextColor => IsDarkMode
        ? Color.FromArgb(0xFF, 0xFF, 0xFF)  // Dark mode: white text
        : Color.FromArgb(0x00, 0x00, 0x00); // Light mode: black text

    private Color HoverBackgroundColor => IsDarkMode
        ? Color.FromArgb(0x28, 0x28, 0x28)  // Dark mode: #282828
        : Color.FromArgb(0xF6, 0xF6, 0xF6); // Light mode: #F6F6F6

    private Color SeparatorColor => IsDarkMode
        ? Color.FromArgb(0x3C, 0x3C, 0x3C)  // Dark mode separator
        : Color.FromArgb(0xE0, 0xE0, 0xE0); // Light mode separator

    private Color BorderColor => IsDarkMode
        ? Color.FromArgb(0x2A, 0x2A, 0x2A)  // Dark mode: #2a2a2a
        : Color.FromArgb(0xE0, 0xE0, 0xE0); // Light mode: #E0E0E0

    // ===== ToolStrip Background =====
    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        // For dropdown menus, use dropdown background; for menu bar, use menu background
        var bgColor = e.ToolStrip is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        e.Graphics.Clear(bgColor);
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
        e.Graphics.Clear(MenuBackgroundColor);
    }

    // ===== Menu Item Background - FULLY OWNER DRAWN =====
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        // Determine background color based on state
        Color bgColor;
        
        if (e.Item.Selected || e.Item.Pressed)
        {
            bgColor = HoverBackgroundColor;
        }
        else if (e.Item.Owner is ToolStripDropDown)
        {
            bgColor = DropdownBackgroundColor;
        }
        else
        {
            bgColor = MenuBackgroundColor;
        }

        // Fill the entire item area - no base call
        var rect = new Rectangle(Point.Empty, e.Item.Size);
        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
    }

    // ===== Item Background (all items) - FULLY OWNER DRAWN =====
    protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
    {
        // Fill entire item with dropdown background
        var bgColor = e.Item.Owner is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        var rect = new Rectangle(Point.Empty, e.Item.Size);
        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
        // No base call
    }

    // ===== Image Margin - FULLY OWNER DRAWN =====
    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
        // Fill with same color as dropdown background
        using (var brush = new SolidBrush(DropdownBackgroundColor))
        {
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }
        // No base call
    }

    // ===== Text Rendering =====
    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = MenuItemTextColor;
        base.OnRenderItemText(e); // Safe to call - just draws text
    }

    // ===== Separator - FULLY OWNER DRAWN =====
    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        // Fill background first
        var bgColor = e.Item.Owner is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        var itemRect = new Rectangle(Point.Empty, e.Item.Size);
        using (var bgBrush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(bgBrush, itemRect);
        }

        // Draw horizontal separator line for dropdown menus
        if (e.Item.Owner is ToolStripDropDown)
        {
            var rect = e.Item.ContentRectangle;
            using (var pen = new Pen(SeparatorColor, 1))
            {
                var centerY = rect.Top + rect.Height / 2;
                e.Graphics.DrawLine(pen, rect.Left + 4, centerY, rect.Right - 4, centerY);
            }
        }
        // No base call
    }

    // ===== Arrow (submenu indicator) =====
    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        e.ArrowColor = MenuItemTextColor;
        base.OnRenderArrow(e); // Safe to call - just draws arrow
    }

    // ===== Dropdown Button Background - FULLY OWNER DRAWN =====
    protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
    {
        Color bgColor;
        if (e.Item.Selected || e.Item.Pressed)
        {
            bgColor = HoverBackgroundColor;
        }
        else
        {
            bgColor = e.Item.Owner is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        }

        var rect = new Rectangle(Point.Empty, e.Item.Size);
        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
        // No base call
    }

    // ===== Border - FULLY OWNER DRAWN =====
    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        if (e.ToolStrip is ToolStripDropDown)
        {
            var rect = e.AffectedBounds;
            using (var pen = new Pen(BorderColor, 1))
            {
                e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }
        }
        // No base call - no borders for menu bar
    }

    // ===== Item Check (checkmark) - FULLY OWNER DRAWN =====
    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
        // Draw checkmark background
        var checkRect = e.ImageRectangle;
        using (var brush = new SolidBrush(HoverBackgroundColor))
        {
            e.Graphics.FillRectangle(brush, checkRect);
        }
        
        // Draw checkmark using text
        using (var font = new Font("Segoe UI", 9F))
        using (var brush = new SolidBrush(MenuItemTextColor))
        {
            e.Graphics.DrawString("✓", font, brush, checkRect.X, checkRect.Y);
        }
        // No base call
    }

    // ===== Item Image =====
    protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
    {
        // Just draw the image, no background changes
        if (e.Image != null)
        {
            e.Graphics.DrawImage(e.Image, e.ImageRectangle);
        }
        // No base call
    }

    // ===== Grip (not used but override for completeness) =====
    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
        // Don't render grip
    }

    // ===== Overflow Button (not used) =====
    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
    {
        // Don't render overflow
    }

    // ===== Split Button (not used) =====
    protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
    {
        // Treat like regular button
        OnRenderDropDownButtonBackground(e);
    }

    // ===== Button Background =====
    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        Color bgColor;
        if (e.Item.Selected || e.Item.Pressed)
        {
            bgColor = HoverBackgroundColor;
        }
        else
        {
            bgColor = e.Item.Owner is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        }

        var rect = new Rectangle(Point.Empty, e.Item.Size);
        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
        // No base call
    }

    // ===== ToolStrip Content Panel Background =====
    protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
    {
        e.Graphics.Clear(MenuBackgroundColor);
    }

    // ===== Status Strip Background =====
    protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
    {
        // Don't render sizing grip
    }

    // ===== Label Background =====
    protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
    {
        var bgColor = e.Item.Owner is ToolStripDropDown ? DropdownBackgroundColor : MenuBackgroundColor;
        var rect = new Rectangle(Point.Empty, e.Item.Size);
        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, rect);
        }
    }
}

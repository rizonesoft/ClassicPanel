using System.Drawing;
using System.Windows.Forms;

namespace ClassicPanel.UI;

/// <summary>
/// Custom MenuStrip renderer with dark/light mode support.
/// Provides consistent styling with the toolbar.
/// </summary>
public class ModernMenuStripRenderer : ToolStripProfessionalRenderer
{
    /// <summary>
    /// Gets the toolbar background color based on current theme.
    /// </summary>
    private Color GetMenuBackgroundColor()
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        return isDarkMode
            ? Color.FromArgb(0x19, 0x19, 0x19)  // Dark mode: #191919
            : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: #FFFFFF
    }

    /// <summary>
    /// Gets the menu item text color based on current theme.
    /// </summary>
    private Color GetMenuItemTextColor()
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        return isDarkMode
            ? Color.FromArgb(0xFF, 0xFF, 0xFF)  // Dark mode: white text
            : Color.FromArgb(0x00, 0x00, 0x00); // Light mode: black text
    }

    /// <summary>
    /// Gets the menu item hover background color based on current theme.
    /// </summary>
    private Color GetMenuItemHoverBackgroundColor()
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        return isDarkMode
            ? Color.FromArgb(0x28, 0x28, 0x28)  // Dark mode: #282828
            : Color.FromArgb(0xF6, 0xF6, 0xF6); // Light mode: #F6F6F6
    }

    /// <summary>
    /// Gets the dropdown menu background color based on current theme.
    /// </summary>
    private Color GetDropdownBackgroundColor()
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        return isDarkMode
            ? Color.FromArgb(0x1E, 0x1E, 0x1E)  // Dark mode: slightly lighter than menu bar
            : Color.FromArgb(0xFF, 0xFF, 0xFF);  // Light mode: white
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        // Menu background based on theme
        var menuBackground = GetMenuBackgroundColor();
        e.Graphics.Clear(menuBackground);
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
        // Menu background based on theme
        var menuBackground = GetMenuBackgroundColor();
        e.Graphics.Clear(menuBackground);
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        var menuItem = e.Item as ToolStripMenuItem;
        if (menuItem == null || !menuItem.Enabled)
        {
            base.OnRenderMenuItemBackground(e);
            return;
        }

        var rect = new Rectangle(Point.Empty, e.Item.Size);

        // Render hover background for both top-level and dropdown items
        if (menuItem.Selected || menuItem.Pressed)
        {
            var hoverBackground = GetMenuItemHoverBackgroundColor();
            using (var brush = new SolidBrush(hoverBackground))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }
        else
        {
            // For non-hovered dropdown items, use dropdown background
            if (menuItem.Owner is ToolStripDropDown)
            {
                var dropdownBackground = GetDropdownBackgroundColor();
                using (var brush = new SolidBrush(dropdownBackground))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
        }
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        // Set text color based on theme
        var textColor = GetMenuItemTextColor();
        e.TextColor = textColor;
        
        base.OnRenderItemText(e);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        var separatorColor = isDarkMode
            ? Color.FromArgb(0x3C, 0x3C, 0x3C)  // Dark mode separator
            : Color.FromArgb(0xE0, 0xE0, 0xE0); // Light mode separator

        var rect = e.Item.ContentRectangle;
        using (var pen = new Pen(separatorColor, 1))
        {
            var centerX = rect.Left + rect.Width / 2;
            e.Graphics.DrawLine(pen, centerX, rect.Top, centerX, rect.Bottom);
        }
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        // Set arrow color based on theme
        var arrowColor = GetMenuItemTextColor();
        e.ArrowColor = arrowColor;
        
        base.OnRenderArrow(e);
    }

    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
        // Use dropdown background color for image margin
        var marginBackground = GetDropdownBackgroundColor();
        using (var brush = new SolidBrush(marginBackground))
        {
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }
    }

    protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
    {
        // Use the same hover background for dropdown buttons
        var menuItem = e.Item as ToolStripMenuItem;
        if (menuItem != null && (menuItem.Selected || menuItem.Pressed))
        {
            var hoverBackground = GetMenuItemHoverBackgroundColor();
            var rect = new Rectangle(Point.Empty, e.Item.Size);
            using (var brush = new SolidBrush(hoverBackground))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }
        else
        {
            base.OnRenderDropDownButtonBackground(e);
        }
    }
}


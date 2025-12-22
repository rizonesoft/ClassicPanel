using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// A modern TabControl with owner-drawn tabs and accent color underline for selected tab.
/// </summary>
public class ModernTabControl : TabControl
{
    private bool _isDarkMode;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                Invalidate();
            }
        }
    }

    public ModernTabControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        DrawMode = TabDrawMode.OwnerDrawFixed;
        ItemSize = new Size(0, 32);
        SizeMode = TabSizeMode.Normal;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Draw background
        var bgColor = _isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White;
        g.Clear(bgColor);

        // Draw tab strip background
        var stripColor = _isDarkMode ? Color.FromArgb(28, 28, 28) : Color.FromArgb(248, 248, 248);
        var stripRect = new Rectangle(0, 0, Width, ItemSize.Height + 2);
        using (var brush = new SolidBrush(stripColor))
        {
            g.FillRectangle(brush, stripRect);
        }

        // Draw bottom border of tab strip
        var borderColor = _isDarkMode ? Color.FromArgb(48, 48, 48) : Color.FromArgb(225, 225, 225);
        using (var pen = new Pen(borderColor, 1))
        {
            g.DrawLine(pen, 0, stripRect.Height - 1, Width, stripRect.Height - 1);
        }

        // Draw tabs
        for (int i = 0; i < TabCount; i++)
        {
            DrawTab(g, i);
        }
    }

    private void DrawTab(Graphics g, int index)
    {
        var tabRect = GetTabRect(index);
        var isSelected = SelectedIndex == index;

        // Tab text color
        var textColor = isSelected
            ? (_isDarkMode ? Color.White : Color.Black)
            : (_isDarkMode ? Color.FromArgb(160, 160, 160) : Color.FromArgb(100, 100, 100));

        // Draw selection background
        if (isSelected)
        {
            var selBgColor = _isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White;
            using (var brush = new SolidBrush(selBgColor))
            {
                var bgRect = new Rectangle(tabRect.X, tabRect.Y + 2, tabRect.Width, tabRect.Height - 2);
                g.FillRectangle(brush, bgRect);
            }

            // Draw accent underline
            var accentColor = ClassicPanel.Core.Theme.ThemeManager.AccentColor;
            using (var brush = new SolidBrush(accentColor))
            {
                var underlineRect = new Rectangle(tabRect.X + 8, tabRect.Bottom - 3, tabRect.Width - 16, 3);
                g.FillRectangle(brush, underlineRect);
            }
        }

        // Draw tab text
        var tabText = TabPages[index].Text;
        TextRenderer.DrawText(g, tabText, Font, tabRect, textColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        // Handled in OnPaint
    }
}

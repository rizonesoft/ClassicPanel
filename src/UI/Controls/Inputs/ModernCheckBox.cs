using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// A modern CheckBox with Windows 11-style appearance.
/// </summary>
public class ModernCheckBox : CheckBox
{
    private bool _isDarkMode;
    private bool _isHovering;

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

    public ModernCheckBox()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.OptimizedDoubleBuffer, true);
        Font = new Font("Segoe UI", 9F);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _isHovering = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _isHovering = false;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // Clear background
        var bgColor = Parent?.BackColor ?? (_isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White);
        g.Clear(bgColor);

        // Checkbox box dimensions
        var boxSize = 16;
        var boxY = (Height - boxSize) / 2;
        var boxRect = new Rectangle(0, boxY, boxSize, boxSize);

        // Box colors
        Color boxBg, boxBorder;
        if (Checked)
        {
            var accent = Core.Theme.ThemeManager.AccentColor;
            boxBg = accent;
            boxBorder = accent;
        }
        else if (_isHovering)
        {
            boxBg = _isDarkMode ? Color.FromArgb(55, 55, 55) : Color.FromArgb(245, 245, 245);
            boxBorder = _isDarkMode ? Color.FromArgb(100, 100, 100) : Color.FromArgb(160, 160, 160);
        }
        else
        {
            boxBg = _isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White;
            boxBorder = _isDarkMode ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
        }

        // Draw rounded box
        using (var path = CreateRoundedPath(boxRect, 3))
        using (var brush = new SolidBrush(boxBg))
        using (var pen = new Pen(boxBorder, 1))
        {
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
        }

        // Draw checkmark if checked
        if (Checked)
        {
            using var pen = new Pen(Color.White, 2f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            var cx = boxRect.X + boxSize / 2;
            var cy = boxRect.Y + boxSize / 2;
            g.DrawLine(pen, cx - 4, cy, cx - 1, cy + 3);
            g.DrawLine(pen, cx - 1, cy + 3, cx + 4, cy - 3);
        }

        // Draw text
        var textColor = _isDarkMode ? Color.FromArgb(240, 240, 240) : Color.FromArgb(25, 25, 25);
        var textRect = new Rectangle(boxSize + 6, 0, Width - boxSize - 6, Height);
        TextRenderer.DrawText(g, Text, Font, textRect, textColor, 
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }

    private static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
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

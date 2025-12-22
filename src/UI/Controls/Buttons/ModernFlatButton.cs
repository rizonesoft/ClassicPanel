using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// A modern flat button styled like Windows 11 buttons.
/// Simple, clean design with subtle hover and pressed states.
/// </summary>
public class ModernFlatButton : Button
{
    private bool _isHovering;
    private bool _isPressed;
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

    public ModernFlatButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = Color.Transparent;
        FlatAppearance.MouseDownBackColor = Color.Transparent;
        Cursor = Cursors.Hand;
        Font = new Font("Segoe UI", 9F);

        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
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
        _isPressed = false;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
        {
            _isPressed = true;
            Invalidate();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _isPressed = false;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // Clear with parent background
        var parentBg = Parent?.BackColor ?? (_isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White);
        g.Clear(parentBg);

        // Button rectangle
        var rect = new Rectangle(1, 1, Width - 3, Height - 3);

        // Colors based on state
        Color bgColor, borderColor, textColor;

        if (!Enabled)
        {
            bgColor = _isDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(243, 243, 243);
            borderColor = _isDarkMode ? Color.FromArgb(55, 55, 55) : Color.FromArgb(220, 220, 220);
            textColor = _isDarkMode ? Color.FromArgb(90, 90, 90) : Color.FromArgb(160, 160, 160);
        }
        else if (_isPressed)
        {
            bgColor = _isDarkMode ? Color.FromArgb(35, 35, 35) : Color.FromArgb(238, 238, 238);
            borderColor = _isDarkMode ? Color.FromArgb(70, 70, 70) : Color.FromArgb(190, 190, 190);
            textColor = _isDarkMode ? Color.FromArgb(200, 200, 200) : Color.FromArgb(30, 30, 30);
        }
        else if (_isHovering)
        {
            bgColor = _isDarkMode ? Color.FromArgb(55, 55, 55) : Color.FromArgb(250, 250, 250);
            borderColor = _isDarkMode ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            textColor = _isDarkMode ? Color.White : Color.Black;
        }
        else
        {
            bgColor = _isDarkMode ? Color.FromArgb(45, 45, 45) : Color.FromArgb(251, 251, 251);
            borderColor = _isDarkMode ? Color.FromArgb(65, 65, 65) : Color.FromArgb(200, 200, 200);
            textColor = _isDarkMode ? Color.FromArgb(245, 245, 245) : Color.FromArgb(25, 25, 25);
        }

        // Draw rounded rectangle
        using (var path = CreateRoundedPath(rect, 4))
        using (var bgBrush = new SolidBrush(bgColor))
        using (var borderPen = new Pen(borderColor, 1))
        {
            g.FillPath(bgBrush, path);
            g.DrawPath(borderPen, path);
        }

        // Draw text centered
        TextRenderer.DrawText(g, Text, Font, new Rectangle(0, 0, Width, Height), textColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        var diameter = radius * 2;

        if (diameter > rect.Width) diameter = rect.Width;
        if (diameter > rect.Height) diameter = rect.Height;

        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        return path;
    }
}

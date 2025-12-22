using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// A modern card control for displaying metrics with a title and content area.
/// </summary>
public class MetricsCard : Panel
{
    private bool _isDarkMode;
    private string _title = "";
    private int _cornerRadius = 6;

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

    [Category("Appearance")]
    [Description("The title displayed at the top of the card.")]
    [DefaultValue("")]
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                Invalidate();
            }
        }
    }

    [Category("Appearance")]
    [Description("Corner radius of the card.")]
    [DefaultValue(6)]
    public int CornerRadius
    {
        get => _cornerRadius;
        set
        {
            if (_cornerRadius != value && value >= 0)
            {
                _cornerRadius = value;
                Invalidate();
            }
        }
    }

    public MetricsCard()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Padding = new Padding(16, 40, 16, 16); // Leave room for title
        Font = new Font("Segoe UI", 9.5F);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // Clear with parent background
        var parentBg = Parent?.BackColor ?? (_isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White);
        g.Clear(parentBg);

        // Card colors
        var cardBg = _isDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(250, 250, 250);
        var cardBorder = _isDarkMode ? Color.FromArgb(55, 55, 55) : Color.FromArgb(225, 225, 225);
        var titleColor = _isDarkMode ? Color.FromArgb(240, 240, 240) : Color.FromArgb(30, 30, 30);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);

        // Draw card background with rounded corners
        using (var path = CreateRoundedPath(rect, _cornerRadius))
        using (var bgBrush = new SolidBrush(cardBg))
        using (var borderPen = new Pen(cardBorder, 1))
        {
            g.FillPath(bgBrush, path);
            g.DrawPath(borderPen, path);
        }

        // Draw title
        if (!string.IsNullOrEmpty(_title))
        {
            using var titleFont = new Font("Segoe UI Semibold", 10F);
            var titleRect = new Rectangle(16, 12, Width - 32, 24);
            TextRenderer.DrawText(g, _title, titleFont, titleRect, titleColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        base.OnPaint(e);
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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// A modern ComboBox with Windows 11-style appearance.
/// </summary>
public class ModernComboBox : ComboBox
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
                UpdateColors();
            }
        }
    }

    public ModernComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        FlatStyle = FlatStyle.Flat;
        Font = new Font("Segoe UI", 9F);
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 24;
    }

    private void UpdateColors()
    {
        BackColor = _isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White;
        ForeColor = _isDarkMode ? Color.FromArgb(240, 240, 240) : Color.FromArgb(25, 25, 25);
        Invalidate();
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Determine colors
        Color bgColor, textColor;
        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            bgColor = _isDarkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(230, 230, 230);
            textColor = _isDarkMode ? Color.White : Color.Black;
        }
        else
        {
            bgColor = _isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White;
            textColor = _isDarkMode ? Color.FromArgb(240, 240, 240) : Color.FromArgb(25, 25, 25);
        }

        // Fill background
        using (var brush = new SolidBrush(bgColor))
        {
            g.FillRectangle(brush, e.Bounds);
        }

        // Draw text
        var text = Items[e.Index]?.ToString() ?? "";
        var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
        TextRenderer.DrawText(g, text, Font, textRect, textColor, 
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }
}

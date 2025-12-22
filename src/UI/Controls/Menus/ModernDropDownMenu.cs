using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClassicPanel.UI.Controls;

/// <summary>
/// Custom ToolStripDropDownMenu with proper container padding support.
/// Provides consistent spacing around menu items through DisplayRectangle adjustment.
/// </summary>
public class ModernDropDownMenu : ToolStripDropDownMenu
{
    private int _containerPadding = 12; // Default 12px padding
    
    /// <summary>
    /// Gets or sets the container padding in pixels.
    /// This adds spacing around all menu items inside the dropdown.
    /// </summary>
    [DefaultValue(12)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int ContainerPadding
    {
        get => _containerPadding;
        set
        {
            _containerPadding = value;
            this.Padding = new Padding(_containerPadding);
            ApplyPaddingToSubmenus();
            PerformLayout();
        }
    }

    /// <summary>
    /// Override DefaultPadding to ensure consistent base padding for all instances.
    /// </summary>
    protected override Padding DefaultPadding => new Padding(_containerPadding);

    public ModernDropDownMenu() : base()
    {
        // Apply custom renderer
        this.Renderer = new ModernMenuStripRenderer();
        
        // Set appearance
        this.ShowImageMargin = true;
        this.ShowCheckMargin = false;
        
        // Use Padding instead of DisplayRectangle override to avoid triggering scroll buttons
        this.Padding = new Padding(_containerPadding);
        
        // Subscribe to ItemAdded to apply padding to submenus
        this.ItemAdded += OnItemAdded;
    }

    /// <summary>
    /// Handles when items are added to apply padding to any submenu dropdowns.
    /// </summary>
    private void OnItemAdded(object? sender, ToolStripItemEventArgs e)
    {
        if (e.Item is ToolStripMenuItem menuItem)
        {
            // Listen for when the dropdown is opened to apply padding
            menuItem.DropDownOpening += OnSubmenuOpening;
        }
    }

    /// <summary>
    /// Called when a submenu is about to open, applies padding to the dropdown.
    /// </summary>
    private void OnSubmenuOpening(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem && menuItem.DropDown != null)
        {
            // Apply the same container padding to the submenu
            menuItem.DropDown.Padding = new Padding(_containerPadding);
            
            // Apply our renderer and theme colors if it's not already a ModernDropDownMenu
            if (menuItem.DropDown is not ModernDropDownMenu)
            {
                menuItem.DropDown.Renderer = new ModernMenuStripRenderer();
                
                // Apply theme background color
                var isDarkMode = string.Equals(
                    ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
                    ClassicPanel.Core.AppConstants.DarkTheme,
                    StringComparison.OrdinalIgnoreCase);

                menuItem.DropDown.BackColor = isDarkMode
                    ? Color.FromArgb(0x1E, 0x1E, 0x1E)
                    : Color.FromArgb(0xFF, 0xFF, 0xFF);
            }
        }
    }

    /// <summary>
    /// Applies padding to all existing submenus (called when ContainerPadding changes).
    /// </summary>
    private void ApplyPaddingToSubmenus()
    {
        foreach (ToolStripItem item in this.Items)
        {
            if (item is ToolStripMenuItem menuItem && menuItem.DropDown != null)
            {
                menuItem.DropDown.Padding = new Padding(_containerPadding);
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Fill background with theme color first (including padding area)
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        var bgColor = isDarkMode
            ? Color.FromArgb(0x1E, 0x1E, 0x1E)  // Dark mode: #1E1E1E
            : Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: white

        using (var brush = new SolidBrush(bgColor))
        {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        // Draw border
        var borderColor = isDarkMode
            ? Color.FromArgb(0x2A, 0x2A, 0x2A)  // Dark mode: #2a2a2a
            : Color.FromArgb(0xE0, 0xE0, 0xE0); // Light mode: #E0E0E0

        using (var pen = new Pen(borderColor, 1))
        {
            e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
        }

        base.OnPaint(e);
    }
    
    /// <summary>
    /// Sets the BackColor for this dropdown based on current theme.
    /// </summary>
    public void ApplyThemeColors()
    {
        var isDarkMode = string.Equals(
            ClassicPanel.Core.Theme.ThemeManager.GetEffectiveTheme(),
            ClassicPanel.Core.AppConstants.DarkTheme,
            StringComparison.OrdinalIgnoreCase);

        this.BackColor = isDarkMode
            ? Color.FromArgb(0x1E, 0x1E, 0x1E)
            : Color.FromArgb(0xFF, 0xFF, 0xFF);
    }
}

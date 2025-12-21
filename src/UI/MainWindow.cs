using ClassicPanel.Core;
using ClassicPanel.Core.Performance;
using ClassicPanel.Core.Theme;
using ClassicPanel.Icons;
using static ClassicPanel.Core.Theme.WindowsThemeInterop;

namespace ClassicPanel.UI;

public partial class MainWindow : Form
{
    private ToolStripButton? _themeToggleButton;
    private ToolStripDropDownButton? _viewDropDownButton;
    private string _currentViewMode = "LargeIcons";

    public MainWindow()
    {
        InitializeComponent();
        InitializeMenu();
        InitializeToolbar();
        ApplyTheme();
        
        // Subscribe to theme changes
        ThemeManager.OnThemeChanged += (effectiveTheme) => 
        {
            ApplyTheme();
            UpdateThemeToggleButton();
        };
    }
    
    /// <summary>
    /// Initializes the toolbar structure. Icons are loaded after window is shown to prevent startup delay.
    /// </summary>
    private void InitializeToolbar()
    {
        const int buttonSize = 52; // 52x52px buttons
        const int iconSize = 32;   // 32x32px icons
        const int menuIconSize = 20; // 20x20px icons for dropdown menus
        toolStrip.ImageScalingSize = new System.Drawing.Size(iconSize, iconSize);
        
        // Create Refresh button
        var refreshButton = new ToolStripButton("Refresh", null, (s, e) => RefreshItems())
        {
            ToolTipText = "Refresh (F5)",
            Tag = "refresh.svg",
            DisplayStyle = ToolStripItemDisplayStyle.Image,
            ImageScaling = ToolStripItemImageScaling.None,
            AutoSize = false,
            Size = new System.Drawing.Size(buttonSize, buttonSize),
            Margin = new Padding(2, 0, 2, 0)
        };
        toolStrip.Items.Add(refreshButton);
        
        var separator1 = new ToolStripSeparator();
        separator1.Margin = new Padding(8, 0, 8, 0);
        toolStrip.Items.Add(separator1);
        
        // Create View dropdown button (styled like regular buttons)
        _viewDropDownButton = new ToolStripDropDownButton("View", null)
        {
            ToolTipText = "Change View",
            Tag = "view-large.svg",
            DisplayStyle = ToolStripItemDisplayStyle.Image,
            ImageScaling = ToolStripItemImageScaling.None,
            AutoSize = false,
            Size = new System.Drawing.Size(buttonSize, buttonSize), // Same size as other buttons
            Margin = new Padding(2, 0, 2, 0),
            ShowDropDownArrow = false // Hide arrow for cleaner look
        };
        
        // Create custom dropdown menu with proper container padding
        var viewDropDownMenu = new ModernDropDownMenu();
        viewDropDownMenu.ContainerPadding = 12;
        viewDropDownMenu.ApplyThemeColors();
        viewDropDownMenu.ShowCheckMargin = true;
        var dropdownBgColor = GetDropdownBackgroundColor();
        
        // Assign the custom dropdown menu to the button
        _viewDropDownButton.DropDown = viewDropDownMenu;
        
        // Add view options to dropdown
        var largeIconsItem = new ToolStripMenuItem("Large Icons", null, (s, e) => SetViewMode("LargeIcons"))
        {
            Tag = "view-large.svg",
            Checked = true,
            Margin = new Padding(4, 4, 4, 4),
            BackColor = dropdownBgColor
        };
        _viewDropDownButton.DropDownItems.Add(largeIconsItem);
        
        var smallIconsItem = new ToolStripMenuItem("Small Icons", null, (s, e) => SetViewMode("SmallIcons"))
        {
            Tag = "view-small.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = dropdownBgColor
        };
        _viewDropDownButton.DropDownItems.Add(smallIconsItem);
        
        var listItem = new ToolStripMenuItem("List", null, (s, e) => SetViewMode("List"))
        {
            Tag = "view-list.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = dropdownBgColor
        };
        _viewDropDownButton.DropDownItems.Add(listItem);
        
        var detailsItem = new ToolStripMenuItem("Details", null, (s, e) => SetViewMode("Details"))
        {
            Tag = "view-details.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = dropdownBgColor
        };
        _viewDropDownButton.DropDownItems.Add(detailsItem);
        
        toolStrip.Items.Add(_viewDropDownButton);
        
        var separator2 = new ToolStripSeparator();
        separator2.Margin = new Padding(8, 0, 8, 0);
        toolStrip.Items.Add(separator2);
        
        // Create 3-state theme toggle button
        CreateThemeToggleButton(buttonSize);
        
        // Load icons after window is shown to prevent blocking startup
        this.Shown += (s, e) =>
        {
            BeginInvoke(new Action(LoadToolbarIcons));
        };
    }
    
    /// <summary>
    /// Creates the 3-state theme toggle button.
    /// </summary>
    private void CreateThemeToggleButton(int buttonSize)
    {
        var currentTheme = ThemeManager.CurrentTheme;
        var iconFileName = GetThemeIconFileName(currentTheme);
        var tooltipText = GetThemeTooltipText(currentTheme);
        
        _themeToggleButton = new ToolStripButton("Theme", null, (s, e) => CycleTheme())
        {
            ToolTipText = tooltipText,
            Tag = iconFileName,
            DisplayStyle = ToolStripItemDisplayStyle.Image,
            ImageScaling = ToolStripItemImageScaling.None,
            AutoSize = false,
            Size = new System.Drawing.Size(buttonSize, buttonSize),
            Margin = new Padding(2, 0, 2, 0)
        };
        toolStrip.Items.Add(_themeToggleButton);
    }
    
    /// <summary>
    /// Gets the icon filename for the given theme mode.
    /// </summary>
    private string GetThemeIconFileName(string theme)
    {
        if (string.Equals(theme, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase))
            return "theme-system.svg";
        if (string.Equals(theme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
            return "theme-light.svg";
        return "theme-dark.svg";
    }
    
    /// <summary>
    /// Gets the tooltip text for the given theme mode.
    /// </summary>
    private string GetThemeTooltipText(string theme)
    {
        if (string.Equals(theme, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase))
            return "Theme: System (click for Light)";
        if (string.Equals(theme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
            return "Theme: Light (click for Dark)";
        return "Theme: Dark (click for System)";
    }
    
    /// <summary>
    /// Loads icons for toolbar buttons and dropdown items. Called after window is shown.
    /// </summary>
    private void LoadToolbarIcons()
    {
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        const int iconSize = 32;     // 32x32px for toolbar buttons
        const int menuIconSize = 20; // 20x20px for dropdown menu items

        // Load icons for toolbar buttons
        foreach (ToolStripItem item in toolStrip.Items)
        {
            if (item is ToolStripButton button && button.Tag is string fileName)
            {
                var icon = SvgFileLoader.RenderSvgFileThemed(fileName, iconSize, isDarkMode);
                button.Image = icon;
            }
            else if (item is ToolStripDropDownButton dropDownButton && dropDownButton.Tag is string ddFileName)
            {
                var icon = SvgFileLoader.RenderSvgFileThemed(ddFileName, iconSize, isDarkMode);
                dropDownButton.Image = icon;
                
                // Load icons for dropdown items
                foreach (ToolStripItem dropDownItem in dropDownButton.DropDownItems)
                {
                    if (dropDownItem is ToolStripMenuItem menuItem && menuItem.Tag is string menuFileName)
                    {
                        var menuIcon = SvgFileLoader.RenderSvgFileThemed(menuFileName, menuIconSize, isDarkMode);
                        menuItem.Image = menuIcon;
                    }
                }
            }
        }
        
        // Load icons for menu items
        LoadMenuIcons(isDarkMode, menuIconSize);
    }
    
    /// <summary>
    /// Loads icons for menu items.
    /// </summary>
    private void LoadMenuIcons(bool isDarkMode, int iconSize)
    {
        foreach (ToolStripItem item in menuStrip.Items)
        {
            if (item is ToolStripMenuItem menuItem)
            {
                LoadMenuItemIcons(menuItem, isDarkMode, iconSize);
            }
        }
    }
    
    /// <summary>
    /// Recursively loads icons for menu items.
    /// </summary>
    private void LoadMenuItemIcons(ToolStripMenuItem menuItem, bool isDarkMode, int iconSize)
    {
        if (menuItem.Tag is string fileName)
        {
            var icon = SvgFileLoader.RenderSvgFileThemed(fileName, iconSize, isDarkMode);
            menuItem.Image = icon;
        }
        
        foreach (ToolStripItem subItem in menuItem.DropDownItems)
        {
            if (subItem is ToolStripMenuItem subMenuItem)
            {
                LoadMenuItemIcons(subMenuItem, isDarkMode, iconSize);
            }
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
        // Apply title bar theme now that handle is created
        ApplyTitleBarTheme();
        
        // Mark startup complete when window is loaded
        PerformanceMonitor.MarkStartupComplete();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        
        // Apply title bar theme when handle is created
        ApplyTitleBarTheme();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        
        // Apply title bar theme again when window is shown (ensures it's applied)
        this.BeginInvoke(new Action(() =>
        {
            ApplyTitleBarTheme();
        }));
    }

    /// <summary>
    /// Applies the current theme to the window and controls.
    /// </summary>
    private void ApplyTheme()
    {
        var theme = ThemeManager.CurrentThemeData;
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        
        // Apply theme to form
        this.BackColor = theme.BackgroundColor;
        this.ForeColor = theme.ForegroundColor;
        
        // Apply title bar theme
        ApplyTitleBarTheme();
        
        // Apply theme to all controls
        ApplyThemeToControls(this, theme);
        
        // Update toolbar icons with new theme colors
        UpdateToolbarIcons(isDarkMode);
    }

    /// <summary>
    /// Updates toolbar icons to match the current theme.
    /// </summary>
    private void UpdateToolbarIcons(bool isDarkMode)
    {
        const int iconSize = 32;     // 32x32px for toolbar buttons
        const int menuIconSize = 20; // 20x20px for dropdown menu items

        foreach (ToolStripItem item in toolStrip.Items)
        {
            if (item is ToolStripButton button && button.Tag is string fileName)
            {
                var newIcon = SvgFileLoader.RenderSvgFileThemed(fileName, iconSize, isDarkMode);
                button.Image = newIcon;
            }
            else if (item is ToolStripDropDownButton dropDownButton && dropDownButton.Tag is string ddFileName)
            {
                var newIcon = SvgFileLoader.RenderSvgFileThemed(ddFileName, iconSize, isDarkMode);
                dropDownButton.Image = newIcon;
                
                // Update dropdown item icons
                foreach (ToolStripItem dropDownItem in dropDownButton.DropDownItems)
                {
                    if (dropDownItem is ToolStripMenuItem menuItem && menuItem.Tag is string menuFileName)
                    {
                        var menuIcon = SvgFileLoader.RenderSvgFileThemed(menuFileName, menuIconSize, isDarkMode);
                        menuItem.Image = menuIcon;
                    }
                }
            }
        }
        
        // Update menu icons
        LoadMenuIcons(isDarkMode, menuIconSize);
    }

    /// <summary>
    /// Applies dark mode to the title bar (non-client area) using DWM.
    /// </summary>
    private void ApplyTitleBarTheme()
    {
        if (!this.IsHandleCreated || this.Handle == nint.Zero)
            return;

        var effectiveTheme = ThemeManager.GetEffectiveTheme();
        bool isDarkMode = string.Equals(effectiveTheme, AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        
        // Apply dark mode to title bar
        bool success = SetWindowTitleBarTheme(this.Handle, isDarkMode);
        
        #if DEBUG
        if (!success)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to apply title bar theme. Dark mode: {isDarkMode}, Handle: {this.Handle}, EffectiveTheme: {effectiveTheme}");
        }
        #endif
        
        // Force window to refresh non-client area using SetWindowPos
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_FRAMECHANGED = 0x0020;
        SetWindowPos(this.Handle, nint.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    /// <summary>
    /// Recursively applies theme to all controls in the form.
    /// </summary>
    private void ApplyThemeToControls(Control parent, ThemeData theme)
    {
        foreach (Control control in parent.Controls)
        {
            // Apply theme based on control type
            if (control is Button button)
            {
                button.BackColor = theme.ControlBackgroundColor;
                button.ForeColor = theme.ControlForegroundColor;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = theme.BorderColor;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = theme.ControlBackgroundColor;
                textBox.ForeColor = theme.ControlForegroundColor;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is Label label)
            {
                label.BackColor = theme.BackgroundColor;
                label.ForeColor = theme.ForegroundColor;
            }
            else if (control is ListView listView)
            {
                listView.BackColor = theme.BackgroundColor;
                listView.ForeColor = theme.ForegroundColor;
            }
            else if (control is MenuStrip menuStrip)
            {
                menuStrip.BackColor = theme.ControlBackgroundColor;
                menuStrip.ForeColor = theme.ControlForegroundColor;
            }
            else if (control is ToolStrip toolStrip)
            {
                toolStrip.BackColor = theme.ControlBackgroundColor;
                toolStrip.ForeColor = theme.ControlForegroundColor;
            }
            else
            {
                // Apply default colors to other controls
                control.BackColor = theme.BackgroundColor;
                control.ForeColor = theme.ForegroundColor;
            }

            // Recursively apply to child controls
            if (control.HasChildren)
            {
                ApplyThemeToControls(control, theme);
            }
        }
    }

    /// <summary>
    /// Initializes the menu system with icons.
    /// </summary>
    private void InitializeMenu()
    {
        var bgColor = GetDropdownBackgroundColor();
        
        // ===== File menu =====
        var fileMenu = new ToolStripMenuItem("&File");
        var fileDropDown = CreateStyledDropDownMenu(bgColor);
        fileMenu.DropDown = fileDropDown;
        
        var refreshMenuItem = new ToolStripMenuItem("&Refresh", null, (s, e) => RefreshItems(), Keys.F5)
        {
            Tag = "refresh.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        fileMenu.DropDownItems.Add(refreshMenuItem);
        
        var separator1 = new ToolStripSeparator();
        separator1.Margin = new Padding(4, 4, 4, 4);
        separator1.BackColor = bgColor;
        fileMenu.DropDownItems.Add(separator1);
        
        var exitMenuItem = new ToolStripMenuItem("E&xit", null, (s, e) => Close(), Keys.Alt | Keys.F4)
        {
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        fileMenu.DropDownItems.Add(exitMenuItem);
        
        menuStrip.Items.Add(fileMenu);

        // ===== View menu =====
        var viewMenu = new ToolStripMenuItem("&View");
        var viewDropDown = CreateStyledDropDownMenu(bgColor);
        viewMenu.DropDown = viewDropDown;
        
        var largeIconsMenuItem = new ToolStripMenuItem("&Large Icons", null, (s, e) => SetViewMode("LargeIcons"))
        {
            Tag = "view-large.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        viewMenu.DropDownItems.Add(largeIconsMenuItem);
        
        var smallIconsMenuItem = new ToolStripMenuItem("&Small Icons", null, (s, e) => SetViewMode("SmallIcons"))
        {
            Tag = "view-small.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        viewMenu.DropDownItems.Add(smallIconsMenuItem);
        
        var listMenuItem = new ToolStripMenuItem("&List", null, (s, e) => SetViewMode("List"))
        {
            Tag = "view-list.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        viewMenu.DropDownItems.Add(listMenuItem);
        
        var detailsMenuItem = new ToolStripMenuItem("&Details", null, (s, e) => SetViewMode("Details"))
        {
            Tag = "view-details.svg",
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        viewMenu.DropDownItems.Add(detailsMenuItem);
        
        menuStrip.Items.Add(viewMenu);

        // ===== Tools menu =====
        var toolsMenu = new ToolStripMenuItem("&Tools");
        var toolsDropDown = CreateStyledDropDownMenu(bgColor);
        toolsMenu.DropDown = toolsDropDown;
        
        var settingsMenuItem = new ToolStripMenuItem("&Settings...", null, (s, e) => ShowSettingsDialog(), Keys.Control | Keys.Oemcomma)
        {
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        toolsMenu.DropDownItems.Add(settingsMenuItem);
        menuStrip.Items.Add(toolsMenu);

        // ===== Help menu =====
        var helpMenu = new ToolStripMenuItem("&Help");
        var helpDropDown = CreateStyledDropDownMenu(bgColor);
        helpMenu.DropDown = helpDropDown;
        
        var aboutMenuItem = new ToolStripMenuItem("&About ClassicPanel...", null, (s, e) => ShowAboutDialog())
        {
            Margin = new Padding(4, 4, 4, 4),
            BackColor = bgColor
        };
        helpMenu.DropDownItems.Add(aboutMenuItem);
        menuStrip.Items.Add(helpMenu);
    }
    
    /// <summary>
    /// Creates a styled ModernDropDownMenu with custom renderer and container padding.
    /// </summary>
    private ModernDropDownMenu CreateStyledDropDownMenu(System.Drawing.Color bgColor)
    {
        var dropDown = new ModernDropDownMenu();
        dropDown.ContainerPadding = 12;
        dropDown.ApplyThemeColors();
        return dropDown;
    }
    
    /// <summary>
    /// Applies custom renderer and styling to a ToolStripMenuItem's dropdown.
    /// </summary>
    private void ApplyMenuDropdownStyling(ToolStripMenuItem menuItem)
    {
        menuItem.DropDownOpening += (s, e) =>
        {
            if (menuItem.DropDown.Renderer is not ModernMenuStripRenderer)
            {
                var bgColor = GetDropdownBackgroundColor();
                menuItem.DropDown.Renderer = new ModernMenuStripRenderer();
                menuItem.DropDown.Padding = new Padding(8, 8, 8, 8); // 8px container spacing
                menuItem.DropDown.BackColor = bgColor;
                
                // Set BackColor on all dropdown items to match container
                foreach (ToolStripItem item in menuItem.DropDownItems)
                {
                    item.BackColor = bgColor;
                }
            }
        };
    }


    /// <summary>
    /// Updates the theme toggle button icon and tooltip.
    /// </summary>
    private void UpdateThemeToggleButton()
    {
        if (_themeToggleButton == null) return;
        
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        var currentTheme = ThemeManager.CurrentTheme;
        var iconFileName = GetThemeIconFileName(currentTheme);
        var tooltipText = GetThemeTooltipText(currentTheme);
        
        const int iconSize = 32; // 32x32px to match toolbar buttons
        var newIcon = SvgFileLoader.RenderSvgFileThemed(iconFileName, iconSize, isDarkMode);
        _themeToggleButton.Image = newIcon;
        _themeToggleButton.Tag = iconFileName;
        _themeToggleButton.ToolTipText = tooltipText;
    }

    /// <summary>
    /// Cycles through theme modes: System → Light → Dark → System
    /// </summary>
    private void CycleTheme()
    {
        var currentTheme = ThemeManager.CurrentTheme;
        
        if (string.Equals(currentTheme, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase))
        {
            ThemeManager.CurrentTheme = AppConstants.LightTheme;
        }
        else if (string.Equals(currentTheme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
        {
            ThemeManager.CurrentTheme = AppConstants.DarkTheme;
        }
        else
        {
            ThemeManager.CurrentTheme = AppConstants.SystemTheme;
        }
    }

    /// <summary>
    /// Toggles between light and dark theme (legacy, kept for compatibility).
    /// </summary>
    private void ToggleTheme()
    {
        CycleTheme();
    }

    /// <summary>
    /// Checks if a bitmap is blank (all transparent or same color).
    /// </summary>
    private static bool IsBitmapBlank(Bitmap bitmap)
    {
        if (bitmap == null) return true;
        
        try
        {
            // Sample a few pixels to check if bitmap is blank
            var pixel1 = bitmap.GetPixel(0, 0);
            var pixel2 = bitmap.GetPixel(bitmap.Width / 2, bitmap.Height / 2);
            var pixel3 = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
            
            // If all pixels are transparent or the same, it's likely blank
            return pixel1.A == 0 && pixel2.A == 0 && pixel3.A == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Refreshes the Control Panel items list.
    /// </summary>
    private void RefreshItems()
    {
        // TODO: Implement refresh logic
        System.Diagnostics.Debug.WriteLine("Refresh clicked");
    }

    /// <summary>
    /// Sets the view mode and updates the UI.
    /// </summary>
    private void SetViewMode(string mode)
    {
        _currentViewMode = mode;
        
        // Update checkmarks in View dropdown
        if (_viewDropDownButton != null)
        {
            foreach (ToolStripItem item in _viewDropDownButton.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    // Check if this item corresponds to the selected mode
                    bool isSelected = menuItem.Text.Replace("&", "").Replace(" ", "") == mode ||
                                     (mode == "LargeIcons" && menuItem.Text.Contains("Large")) ||
                                     (mode == "SmallIcons" && menuItem.Text.Contains("Small")) ||
                                     (mode == "List" && menuItem.Text == "List") ||
                                     (mode == "Details" && menuItem.Text.Contains("Details"));
                    menuItem.Checked = isSelected;
                }
            }
            
            // Update dropdown button icon to match current view
            var iconFileName = mode switch
            {
                "LargeIcons" => "view-large.svg",
                "SmallIcons" => "view-small.svg",
                "List" => "view-list.svg",
                "Details" => "view-details.svg",
                _ => "view-large.svg"
            };
            _viewDropDownButton.Tag = iconFileName;
            
            var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
            var icon = SvgFileLoader.RenderSvgFileThemed(iconFileName, 32, isDarkMode);
            _viewDropDownButton.Image = icon;
        }
        
        System.Diagnostics.Debug.WriteLine($"View mode: {mode}");
    }

    /// <summary>
    /// Shows the settings dialog.
    /// </summary>
    private void ShowSettingsDialog()
    {
        using var settingsDialog = new SettingsDialog();
        settingsDialog.ShowDialog(this);
    }

    /// <summary>
    /// Shows the about dialog.
    /// </summary>
    private void ShowAboutDialog()
    {
        // TODO: Implement about dialog
        MessageBox.Show(
            $"ClassicPanel\n\nVersion: {VersionInfo.Version}\n\nCopyright © 2025 Rizonetech (Pty) Ltd",
            "About ClassicPanel",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }
    
    /// <summary>
    /// Gets the dropdown background color based on current theme.
    /// </summary>
    private System.Drawing.Color GetDropdownBackgroundColor()
    {
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        return isDarkMode
            ? System.Drawing.Color.FromArgb(0x1E, 0x1E, 0x1E)  // Dark mode: #1E1E1E
            : System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF); // Light mode: white
    }
}

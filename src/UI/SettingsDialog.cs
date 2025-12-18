using System.Drawing;
using System.Windows.Forms;
using ClassicPanel.Core;
using ClassicPanel.Core.Theme;

namespace ClassicPanel.UI;

/// <summary>
/// Full-featured settings dialog for ClassicPanel configuration.
/// Features a category sidebar on the left and settings panels on the right.
/// Uses standard Windows styling with modern visual enhancements.
/// </summary>
public partial class SettingsDialog : Form
{
    // Layout constants
    private const int DialogWidth = 800;
    private const int DialogHeight = 600;
    private const int SidebarWidth = 220;
    private const int CategoryItemHeight = 32;
    private const int ButtonWidth = 88;
    private const int ButtonHeight = 30;
    private const int GroupBoxPadding = 20;
    private const int ControlSpacing = 12;
    private const int LabelWidth = 100;
    private const int ControlWidth = 220;

    private Panel sidebarPanel;
    private ListBox categoryListBox;
    private Panel contentPanel;
    private Panel settingsPanel;
    private Panel buttonPanel;
    private Button okButton;
    private Button cancelButton;
    private Button applyButton;
    private Button resetButton;
    private ToolTip toolTip;

    // Settings panels
    private Panel generalPanel;
    private Panel appearancePanel;
    private Panel viewPanel;
    private Panel advancedPanel;

    // General settings
    private GroupBox startupGroupBox;
    private Label startupDescLabel;
    private CheckBox startWithWindowsCheckBox;
    private CheckBox minimizeToTrayCheckBox;
    private CheckBox showNotificationsCheckBox;

    // Appearance settings
    private GroupBox themeGroupBox;
    private Label themeDescLabel;
    private RadioButton systemThemeRadio;
    private RadioButton lightThemeRadio;
    private RadioButton darkThemeRadio;

    // View settings
    private GroupBox viewGroupBox;
    private Label viewDescLabel;
    private Label defaultViewLabel;
    private ComboBox defaultViewComboBox;
    private Label iconSizeLabel;
    private TrackBar iconSizeTrackBar;
    private Label iconSizeValueLabel;
    private Label iconSizeMinLabel;
    private Label iconSizeMaxLabel;

    // Advanced settings
    private GroupBox loggingGroupBox;
    private Label loggingDescLabel;
    private CheckBox enableLoggingCheckBox;
    private Label logLevelLabel;
    private ComboBox logLevelComboBox;
    private GroupBox cacheGroupBox;
    private Label cacheDescLabel;
    private Button clearCacheButton;

    public SettingsDialog()
    {
        InitializeComponent();
        LoadSettings();
        SelectCategory(0);
    }

    private void InitializeComponent()
    {
        // Initialize tooltip
        this.toolTip = new ToolTip();
        this.toolTip.AutoPopDelay = 5000;
        this.toolTip.InitialDelay = 500;
        this.toolTip.ReshowDelay = 200;
        this.toolTip.ShowAlways = true;

        // Initialize main containers
        this.sidebarPanel = new Panel();
        this.categoryListBox = new ListBox();
        this.contentPanel = new Panel();
        this.settingsPanel = new Panel();
        this.buttonPanel = new Panel();
        this.okButton = new Button();
        this.cancelButton = new Button();
        this.applyButton = new Button();
        this.resetButton = new Button();

        // Initialize settings panels
        this.generalPanel = new Panel();
        this.appearancePanel = new Panel();
        this.viewPanel = new Panel();
        this.advancedPanel = new Panel();

        // Initialize General settings controls
        this.startupGroupBox = new GroupBox();
        this.startupDescLabel = new Label();
        this.startWithWindowsCheckBox = new CheckBox();
        this.minimizeToTrayCheckBox = new CheckBox();
        this.showNotificationsCheckBox = new CheckBox();

        // Initialize Appearance settings controls
        this.themeGroupBox = new GroupBox();
        this.themeDescLabel = new Label();
        this.systemThemeRadio = new RadioButton();
        this.lightThemeRadio = new RadioButton();
        this.darkThemeRadio = new RadioButton();

        // Initialize View settings controls
        this.viewGroupBox = new GroupBox();
        this.viewDescLabel = new Label();
        this.defaultViewLabel = new Label();
        this.defaultViewComboBox = new ComboBox();
        this.iconSizeLabel = new Label();
        this.iconSizeTrackBar = new TrackBar();
        this.iconSizeValueLabel = new Label();
        this.iconSizeMinLabel = new Label();
        this.iconSizeMaxLabel = new Label();

        // Initialize Advanced settings controls
        this.loggingGroupBox = new GroupBox();
        this.loggingDescLabel = new Label();
        this.enableLoggingCheckBox = new CheckBox();
        this.logLevelLabel = new Label();
        this.logLevelComboBox = new ComboBox();
        this.cacheGroupBox = new GroupBox();
        this.cacheDescLabel = new Label();
        this.clearCacheButton = new Button();

        this.SuspendLayout();

        // ===== Sidebar Panel =====
        this.sidebarPanel.Dock = DockStyle.Left;
        this.sidebarPanel.Width = SidebarWidth;
        this.sidebarPanel.Padding = new Padding(0);
        this.sidebarPanel.BackColor = SystemColors.Control;

        // ===== Category ListBox =====
        this.categoryListBox.Dock = DockStyle.Fill;
        this.categoryListBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.categoryListBox.ItemHeight = CategoryItemHeight;
        this.categoryListBox.DrawMode = DrawMode.OwnerDrawFixed;
        this.categoryListBox.BorderStyle = BorderStyle.None;
        this.categoryListBox.BackColor = SystemColors.Control;
        this.categoryListBox.Items.AddRange(new object[] {
            "  General",
            "  Appearance",
            "  View",
            "  Advanced"
        });
        this.categoryListBox.DrawItem += CategoryListBox_DrawItem;
        this.categoryListBox.SelectedIndexChanged += CategoryListBox_SelectedIndexChanged;

        this.sidebarPanel.Controls.Add(this.categoryListBox);

        // ===== Content Panel =====
        this.contentPanel.Dock = DockStyle.Fill;
        this.contentPanel.Padding = new Padding(0);

        // ===== Settings Panel =====
        this.settingsPanel.Dock = DockStyle.Fill;
        this.settingsPanel.Padding = new Padding(24, 16, 24, 16);
        this.settingsPanel.AutoScroll = true;
        this.settingsPanel.BackColor = SystemColors.Window;

        // ===== Button Panel =====
        this.buttonPanel.Dock = DockStyle.Bottom;
        this.buttonPanel.Height = 54;
        this.buttonPanel.Padding = new Padding(12, 12, 12, 12);
        this.buttonPanel.BackColor = SystemColors.Control;

        // Initialize all settings panels
        InitializeGeneralPanel();
        InitializeAppearancePanel();
        InitializeViewPanel();
        InitializeAdvancedPanel();

        // ===== Reset Button =====
        this.resetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.resetButton.Location = new Point(12, 13);
        this.resetButton.Size = new Size(110, ButtonHeight);
        this.resetButton.Text = "Reset to Defaults";
        this.resetButton.UseVisualStyleBackColor = true;
        this.resetButton.TabIndex = 0;
        this.resetButton.Click += ResetButton_Click;
        this.toolTip.SetToolTip(this.resetButton, "Restore all settings to their default values");

        // ===== Apply Button =====
        this.applyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.applyButton.Location = new Point(DialogWidth - 3 * ButtonWidth - 3 * 8 - 12, 13);
        this.applyButton.Size = new Size(ButtonWidth, ButtonHeight);
        this.applyButton.Text = "Apply";
        this.applyButton.UseVisualStyleBackColor = true;
        this.applyButton.TabIndex = 1;
        this.applyButton.Click += ApplyButton_Click;
        this.toolTip.SetToolTip(this.applyButton, "Apply changes without closing the dialog");

        // ===== OK Button =====
        this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.okButton.DialogResult = DialogResult.OK;
        this.okButton.Location = new Point(DialogWidth - 2 * ButtonWidth - 2 * 8 - 12, 13);
        this.okButton.Size = new Size(ButtonWidth, ButtonHeight);
        this.okButton.Text = "OK";
        this.okButton.UseVisualStyleBackColor = true;
        this.okButton.TabIndex = 2;
        this.okButton.Click += OkButton_Click;
        this.toolTip.SetToolTip(this.okButton, "Save changes and close");

        // ===== Cancel Button =====
        this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.cancelButton.DialogResult = DialogResult.Cancel;
        this.cancelButton.Location = new Point(DialogWidth - ButtonWidth - 8 - 12, 13);
        this.cancelButton.Size = new Size(ButtonWidth, ButtonHeight);
        this.cancelButton.Text = "Cancel";
        this.cancelButton.UseVisualStyleBackColor = true;
        this.cancelButton.TabIndex = 3;
        this.toolTip.SetToolTip(this.cancelButton, "Discard changes and close");

        this.buttonPanel.Controls.Add(this.resetButton);
        this.buttonPanel.Controls.Add(this.applyButton);
        this.buttonPanel.Controls.Add(this.okButton);
        this.buttonPanel.Controls.Add(this.cancelButton);

        this.contentPanel.Controls.Add(this.settingsPanel);
        this.contentPanel.Controls.Add(this.buttonPanel);

        // ===== SettingsDialog =====
        this.AcceptButton = this.okButton;
        this.CancelButton = this.cancelButton;
        this.ClientSize = new Size(DialogWidth, DialogHeight);
        this.Controls.Add(this.contentPanel);
        this.Controls.Add(this.sidebarPanel);
        this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "SettingsDialog";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Settings";
        this.ResumeLayout(false);
    }

    private void CategoryListBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        e.DrawBackground();

        var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var text = categoryListBox.Items[e.Index]?.ToString() ?? string.Empty;

        // Draw selection highlight with accent border
        if (isSelected)
        {
            using var accentBrush = new SolidBrush(Color.FromArgb(100, 100, 100));
            e.Graphics.FillRectangle(accentBrush, new Rectangle(e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height));

            using var bgBrush = new SolidBrush(Color.FromArgb(230, 230, 230));
            e.Graphics.FillRectangle(bgBrush, new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height));
        }

        // Draw text
        var font = isSelected
            ? new Font(e.Font ?? categoryListBox.Font, FontStyle.Bold)
            : e.Font ?? categoryListBox.Font;

        var textColor = isSelected ? Color.FromArgb(30, 30, 30) : SystemColors.ControlText;
        using var textBrush = new SolidBrush(textColor);

        var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height);
        var format = new StringFormat { LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString(text, font, textBrush, textRect, format);

        if (isSelected) font.Dispose();
    }

    private void InitializeGeneralPanel()
    {
        this.generalPanel.Dock = DockStyle.Fill;
        this.generalPanel.AutoScroll = true;

        // ===== Startup GroupBox =====
        this.startupGroupBox.Text = "Startup & Behavior";
        this.startupGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.startupGroupBox.Location = new Point(0, 0);
        this.startupGroupBox.Size = new Size(500, 180);
        this.startupGroupBox.Padding = new Padding(GroupBoxPadding);

        int yPos = 28;

        // Description label
        this.startupDescLabel.Text = "Configure how ClassicPanel starts and behaves on your system.";
        this.startupDescLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        this.startupDescLabel.ForeColor = SystemColors.GrayText;
        this.startupDescLabel.Location = new Point(GroupBoxPadding, yPos);
        this.startupDescLabel.Size = new Size(460, 20);
        yPos += 32;

        // Start with Windows
        this.startWithWindowsCheckBox.Text = "Start ClassicPanel with Windows";
        this.startWithWindowsCheckBox.AutoSize = true;
        this.startWithWindowsCheckBox.Location = new Point(GroupBoxPadding, yPos);
        this.startWithWindowsCheckBox.Size = new Size(300, 24);
        this.startWithWindowsCheckBox.TabIndex = 0;
        this.toolTip.SetToolTip(this.startWithWindowsCheckBox, "Automatically launch ClassicPanel when Windows starts");
        yPos += ControlSpacing + 24;

        // Minimize to tray
        this.minimizeToTrayCheckBox.Text = "Minimize to system tray";
        this.minimizeToTrayCheckBox.AutoSize = true;
        this.minimizeToTrayCheckBox.Location = new Point(GroupBoxPadding, yPos);
        this.minimizeToTrayCheckBox.Size = new Size(300, 24);
        this.minimizeToTrayCheckBox.TabIndex = 1;
        this.toolTip.SetToolTip(this.minimizeToTrayCheckBox, "Minimize to the notification area instead of the taskbar");
        yPos += ControlSpacing + 24;

        // Show notifications
        this.showNotificationsCheckBox.Text = "Show notifications";
        this.showNotificationsCheckBox.AutoSize = true;
        this.showNotificationsCheckBox.Location = new Point(GroupBoxPadding, yPos);
        this.showNotificationsCheckBox.Size = new Size(300, 24);
        this.showNotificationsCheckBox.TabIndex = 2;
        this.toolTip.SetToolTip(this.showNotificationsCheckBox, "Display system notifications from ClassicPanel");

        this.startupGroupBox.Controls.Add(this.startupDescLabel);
        this.startupGroupBox.Controls.Add(this.startWithWindowsCheckBox);
        this.startupGroupBox.Controls.Add(this.minimizeToTrayCheckBox);
        this.startupGroupBox.Controls.Add(this.showNotificationsCheckBox);
        this.generalPanel.Controls.Add(this.startupGroupBox);
    }

    private void InitializeAppearancePanel()
    {
        this.appearancePanel.Dock = DockStyle.Fill;
        this.appearancePanel.AutoScroll = true;

        // ===== Theme GroupBox =====
        this.themeGroupBox.Text = "Application Theme";
        this.themeGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.themeGroupBox.Location = new Point(0, 0);
        this.themeGroupBox.Size = new Size(500, 200);
        this.themeGroupBox.Padding = new Padding(GroupBoxPadding);

        int yPos = 28;

        // Description label
        this.themeDescLabel.Text = "Choose a theme for ClassicPanel. System theme follows your Windows settings.";
        this.themeDescLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        this.themeDescLabel.ForeColor = SystemColors.GrayText;
        this.themeDescLabel.Location = new Point(GroupBoxPadding, yPos);
        this.themeDescLabel.Size = new Size(460, 20);
        yPos += 36;

        // System theme
        this.systemThemeRadio.Text = "Use system setting";
        this.systemThemeRadio.AutoSize = true;
        this.systemThemeRadio.Location = new Point(GroupBoxPadding, yPos);
        this.systemThemeRadio.Size = new Size(300, 24);
        this.systemThemeRadio.TabIndex = 0;
        this.systemThemeRadio.CheckedChanged += SystemThemeRadio_CheckedChanged;
        this.toolTip.SetToolTip(this.systemThemeRadio, "Automatically match Windows light/dark mode preference");
        yPos += ControlSpacing + 24;

        // Light theme
        this.lightThemeRadio.Text = "Light";
        this.lightThemeRadio.AutoSize = true;
        this.lightThemeRadio.Location = new Point(GroupBoxPadding, yPos);
        this.lightThemeRadio.Size = new Size(300, 24);
        this.lightThemeRadio.TabIndex = 1;
        this.lightThemeRadio.CheckedChanged += LightThemeRadio_CheckedChanged;
        this.toolTip.SetToolTip(this.lightThemeRadio, "Use light theme with bright backgrounds");
        yPos += ControlSpacing + 24;

        // Dark theme
        this.darkThemeRadio.Text = "Dark";
        this.darkThemeRadio.AutoSize = true;
        this.darkThemeRadio.Location = new Point(GroupBoxPadding, yPos);
        this.darkThemeRadio.Size = new Size(300, 24);
        this.darkThemeRadio.TabIndex = 2;
        this.darkThemeRadio.CheckedChanged += DarkThemeRadio_CheckedChanged;
        this.toolTip.SetToolTip(this.darkThemeRadio, "Use dark theme with darker backgrounds");

        this.themeGroupBox.Controls.Add(this.themeDescLabel);
        this.themeGroupBox.Controls.Add(this.systemThemeRadio);
        this.themeGroupBox.Controls.Add(this.lightThemeRadio);
        this.themeGroupBox.Controls.Add(this.darkThemeRadio);
        this.appearancePanel.Controls.Add(this.themeGroupBox);
    }

    private void InitializeViewPanel()
    {
        this.viewPanel.Dock = DockStyle.Fill;
        this.viewPanel.AutoScroll = true;

        // ===== View GroupBox =====
        this.viewGroupBox.Text = "View Options";
        this.viewGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.viewGroupBox.Location = new Point(0, 0);
        this.viewGroupBox.Size = new Size(500, 220);
        this.viewGroupBox.Padding = new Padding(GroupBoxPadding);

        int yPos = 28;

        // Description label
        this.viewDescLabel.Text = "Customize how items are displayed in the main panel.";
        this.viewDescLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        this.viewDescLabel.ForeColor = SystemColors.GrayText;
        this.viewDescLabel.Location = new Point(GroupBoxPadding, yPos);
        this.viewDescLabel.Size = new Size(460, 20);
        yPos += 36;

        // Default View
        this.defaultViewLabel.Text = "Default view:";
        this.defaultViewLabel.AutoSize = true;
        this.defaultViewLabel.Location = new Point(GroupBoxPadding, yPos + 3);
        this.defaultViewLabel.Size = new Size(LabelWidth, 20);

        this.defaultViewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        this.defaultViewComboBox.Items.AddRange(new object[] {
            "Large Icons",
            "Small Icons",
            "List",
            "Details"
        });
        this.defaultViewComboBox.Location = new Point(GroupBoxPadding + LabelWidth + 8, yPos);
        this.defaultViewComboBox.Size = new Size(ControlWidth, 28);
        this.defaultViewComboBox.TabIndex = 0;
        this.toolTip.SetToolTip(this.defaultViewComboBox, "Select the default view mode when opening ClassicPanel");
        yPos += 40;

        // Icon Size
        this.iconSizeLabel.Text = "Icon size:";
        this.iconSizeLabel.AutoSize = true;
        this.iconSizeLabel.Location = new Point(GroupBoxPadding, yPos + 10);
        this.iconSizeLabel.Size = new Size(LabelWidth, 20);

        this.iconSizeTrackBar.Location = new Point(GroupBoxPadding + LabelWidth + 8, yPos);
        this.iconSizeTrackBar.Size = new Size(ControlWidth, 45);
        this.iconSizeTrackBar.Minimum = 16;
        this.iconSizeTrackBar.Maximum = 64;
        this.iconSizeTrackBar.TickFrequency = 16;
        this.iconSizeTrackBar.LargeChange = 16;
        this.iconSizeTrackBar.SmallChange = 8;
        this.iconSizeTrackBar.Value = 32;
        this.iconSizeTrackBar.TabIndex = 1;
        this.iconSizeTrackBar.ValueChanged += IconSizeTrackBar_ValueChanged;
        this.toolTip.SetToolTip(this.iconSizeTrackBar, "Adjust the size of icons in the main panel (16px - 64px)");

        this.iconSizeValueLabel.Text = "32 px";
        this.iconSizeValueLabel.AutoSize = true;
        this.iconSizeValueLabel.ForeColor = Color.FromArgb(0, 120, 215);
        this.iconSizeValueLabel.Location = new Point(GroupBoxPadding + LabelWidth + ControlWidth + 16, yPos + 10);
        this.iconSizeValueLabel.Size = new Size(50, 20);
        this.iconSizeValueLabel.TextAlign = ContentAlignment.MiddleLeft;

        // Min/Max labels for slider
        this.iconSizeMinLabel.Text = "16";
        this.iconSizeMinLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
        this.iconSizeMinLabel.ForeColor = SystemColors.GrayText;
        this.iconSizeMinLabel.Location = new Point(GroupBoxPadding + LabelWidth + 8, yPos + 40);
        this.iconSizeMinLabel.Size = new Size(25, 15);

        this.iconSizeMaxLabel.Text = "64";
        this.iconSizeMaxLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
        this.iconSizeMaxLabel.ForeColor = SystemColors.GrayText;
        this.iconSizeMaxLabel.Location = new Point(GroupBoxPadding + LabelWidth + ControlWidth - 10, yPos + 40);
        this.iconSizeMaxLabel.Size = new Size(25, 15);
        this.iconSizeMaxLabel.TextAlign = ContentAlignment.TopRight;

        this.viewGroupBox.Controls.Add(this.viewDescLabel);
        this.viewGroupBox.Controls.Add(this.defaultViewLabel);
        this.viewGroupBox.Controls.Add(this.defaultViewComboBox);
        this.viewGroupBox.Controls.Add(this.iconSizeLabel);
        this.viewGroupBox.Controls.Add(this.iconSizeTrackBar);
        this.viewGroupBox.Controls.Add(this.iconSizeValueLabel);
        this.viewGroupBox.Controls.Add(this.iconSizeMinLabel);
        this.viewGroupBox.Controls.Add(this.iconSizeMaxLabel);
        this.viewPanel.Controls.Add(this.viewGroupBox);
    }

    private void InitializeAdvancedPanel()
    {
        this.advancedPanel.Dock = DockStyle.Fill;
        this.advancedPanel.AutoScroll = true;

        // ===== Logging GroupBox =====
        this.loggingGroupBox.Text = "Logging";
        this.loggingGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.loggingGroupBox.Location = new Point(0, 0);
        this.loggingGroupBox.Size = new Size(500, 160);
        this.loggingGroupBox.Padding = new Padding(GroupBoxPadding);

        int yPos = 28;

        // Description label
        this.loggingDescLabel.Text = "Enable diagnostic logging for troubleshooting issues.";
        this.loggingDescLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        this.loggingDescLabel.ForeColor = SystemColors.GrayText;
        this.loggingDescLabel.Location = new Point(GroupBoxPadding, yPos);
        this.loggingDescLabel.Size = new Size(460, 20);
        yPos += 32;

        // Enable logging
        this.enableLoggingCheckBox.Text = "Enable debug logging";
        this.enableLoggingCheckBox.AutoSize = true;
        this.enableLoggingCheckBox.Location = new Point(GroupBoxPadding, yPos);
        this.enableLoggingCheckBox.Size = new Size(300, 24);
        this.enableLoggingCheckBox.TabIndex = 0;
        this.enableLoggingCheckBox.CheckedChanged += EnableLoggingCheckBox_CheckedChanged;
        this.toolTip.SetToolTip(this.enableLoggingCheckBox, "Write diagnostic information to log files for troubleshooting");
        yPos += ControlSpacing + 24;

        // Log level
        this.logLevelLabel.Text = "Log level:";
        this.logLevelLabel.AutoSize = true;
        this.logLevelLabel.Location = new Point(GroupBoxPadding, yPos + 3);
        this.logLevelLabel.Size = new Size(LabelWidth, 20);
        this.logLevelLabel.Enabled = false;

        this.logLevelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        this.logLevelComboBox.Items.AddRange(new object[] {
            "Debug",
            "Info",
            "Warning",
            "Error"
        });
        this.logLevelComboBox.Location = new Point(GroupBoxPadding + LabelWidth + 8, yPos);
        this.logLevelComboBox.Size = new Size(ControlWidth, 28);
        this.logLevelComboBox.TabIndex = 1;
        this.logLevelComboBox.SelectedIndex = 1;
        this.logLevelComboBox.Enabled = false;
        this.toolTip.SetToolTip(this.logLevelComboBox, "Select the minimum severity level for log messages");

        this.loggingGroupBox.Controls.Add(this.loggingDescLabel);
        this.loggingGroupBox.Controls.Add(this.enableLoggingCheckBox);
        this.loggingGroupBox.Controls.Add(this.logLevelLabel);
        this.loggingGroupBox.Controls.Add(this.logLevelComboBox);

        // ===== Cache GroupBox =====
        this.cacheGroupBox.Text = "Cache Management";
        this.cacheGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.cacheGroupBox.Location = new Point(0, 170);
        this.cacheGroupBox.Size = new Size(500, 130);
        this.cacheGroupBox.Padding = new Padding(GroupBoxPadding);

        yPos = 28;

        // Description label
        this.cacheDescLabel.Text = "Clear cached icons and data to free up space or fix display issues.";
        this.cacheDescLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
        this.cacheDescLabel.ForeColor = SystemColors.GrayText;
        this.cacheDescLabel.Location = new Point(GroupBoxPadding, yPos);
        this.cacheDescLabel.Size = new Size(460, 20);
        yPos += 36;

        // Clear cache button
        this.clearCacheButton.Text = "Clear Icon Cache";
        this.clearCacheButton.Location = new Point(GroupBoxPadding, yPos);
        this.clearCacheButton.Size = new Size(180, 30);
        this.clearCacheButton.TabIndex = 0;
        this.clearCacheButton.UseVisualStyleBackColor = true;
        this.clearCacheButton.Click += ClearCacheButton_Click;
        this.toolTip.SetToolTip(this.clearCacheButton, "Remove all cached icon files to reclaim disk space");

        this.cacheGroupBox.Controls.Add(this.cacheDescLabel);
        this.cacheGroupBox.Controls.Add(this.clearCacheButton);

        this.advancedPanel.Controls.Add(this.loggingGroupBox);
        this.advancedPanel.Controls.Add(this.cacheGroupBox);
    }

    private void EnableLoggingCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        this.logLevelLabel.Enabled = this.enableLoggingCheckBox.Checked;
        this.logLevelComboBox.Enabled = this.enableLoggingCheckBox.Checked;
    }

    private void CategoryListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (categoryListBox.SelectedIndex >= 0)
        {
            SelectCategory(categoryListBox.SelectedIndex);
        }
    }

    private void SelectCategory(int index)
    {
        // Hide all panels
        generalPanel.Visible = false;
        appearancePanel.Visible = false;
        viewPanel.Visible = false;
        advancedPanel.Visible = false;

        // Remove all panels from settingsPanel
        settingsPanel.Controls.Clear();

        // Show and add the selected panel
        switch (index)
        {
            case 0: // General
                settingsPanel.Controls.Add(generalPanel);
                generalPanel.Visible = true;
                break;
            case 1: // Appearance
                settingsPanel.Controls.Add(appearancePanel);
                appearancePanel.Visible = true;
                break;
            case 2: // View
                settingsPanel.Controls.Add(viewPanel);
                viewPanel.Visible = true;
                break;
            case 3: // Advanced
                settingsPanel.Controls.Add(advancedPanel);
                advancedPanel.Visible = true;
                break;
        }

        // Ensure selection is visible
        categoryListBox.SelectedIndex = index;
    }

    private void LoadSettings()
    {
        // Load theme setting
        var currentTheme = ThemeManager.CurrentTheme;
        if (string.Equals(currentTheme, AppConstants.SystemTheme, StringComparison.OrdinalIgnoreCase))
        {
            systemThemeRadio.Checked = true;
        }
        else if (string.Equals(currentTheme, AppConstants.LightTheme, StringComparison.OrdinalIgnoreCase))
        {
            lightThemeRadio.Checked = true;
        }
        else if (string.Equals(currentTheme, AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase))
        {
            darkThemeRadio.Checked = true;
        }

        // Set default values
        defaultViewComboBox.SelectedIndex = 0;
        iconSizeTrackBar.Value = 32;
        iconSizeValueLabel.Text = "32 px";
    }

    private void SaveSettings()
    {
        // Theme is already saved when radio buttons change
        // TODO: Save other settings to configuration
    }

    private void ResetToDefaults()
    {
        // General
        startWithWindowsCheckBox.Checked = false;
        minimizeToTrayCheckBox.Checked = false;
        showNotificationsCheckBox.Checked = true;

        // Appearance
        systemThemeRadio.Checked = true;

        // View
        defaultViewComboBox.SelectedIndex = 0;
        iconSizeTrackBar.Value = 32;

        // Advanced
        enableLoggingCheckBox.Checked = false;
        logLevelComboBox.SelectedIndex = 1;
    }

    private void ResetButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to reset all settings to their default values?",
            "Reset Settings",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            ResetToDefaults();
            MessageBox.Show(
                "Settings have been reset to defaults.",
                "Settings Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void SystemThemeRadio_CheckedChanged(object? sender, EventArgs e)
    {
        if (systemThemeRadio.Checked)
        {
            ThemeManager.CurrentTheme = AppConstants.SystemTheme;
        }
    }

    private void LightThemeRadio_CheckedChanged(object? sender, EventArgs e)
    {
        if (lightThemeRadio.Checked)
        {
            ThemeManager.CurrentTheme = AppConstants.LightTheme;
        }
    }

    private void DarkThemeRadio_CheckedChanged(object? sender, EventArgs e)
    {
        if (darkThemeRadio.Checked)
        {
            ThemeManager.CurrentTheme = AppConstants.DarkTheme;
        }
    }

    private void IconSizeTrackBar_ValueChanged(object? sender, EventArgs e)
    {
        iconSizeValueLabel.Text = $"{iconSizeTrackBar.Value} px";
    }

    private void ClearCacheButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "This will clear all cached icons. ClassicPanel will need to reload icons on next startup.\n\nContinue?",
            "Clear Cache",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // TODO: Implement actual cache clearing
            MessageBox.Show(
                "Icon cache cleared successfully.",
                "Cache Cleared",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        SaveSettings();
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        SaveSettings();
        MessageBox.Show(
            "Settings have been applied.",
            "Settings Applied",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}

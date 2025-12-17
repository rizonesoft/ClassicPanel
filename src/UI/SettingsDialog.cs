using ClassicPanel.Core;
using ClassicPanel.Core.Theme;
using ClassicPanel.Icons;

namespace ClassicPanel.UI;

/// <summary>
/// Settings dialog for ClassicPanel configuration.
/// </summary>
public partial class SettingsDialog : Form
{
    private GroupBox themeGroupBox;
    private RadioButton systemThemeRadio;
    private RadioButton lightThemeRadio;
    private RadioButton darkThemeRadio;
    private Button okButton;
    private Button cancelButton;

    public SettingsDialog()
    {
        InitializeComponent();
        LoadSettings();
        ApplyTheme();
        
        // Subscribe to theme changes
        ThemeManager.OnThemeChanged += (effectiveTheme) => ApplyTheme();
    }

    private void InitializeComponent()
    {
        this.themeGroupBox = new GroupBox();
        this.systemThemeRadio = new RadioButton();
        this.lightThemeRadio = new RadioButton();
        this.darkThemeRadio = new RadioButton();
        this.okButton = new Button();
        this.cancelButton = new Button();
        this.themeGroupBox.SuspendLayout();
        this.SuspendLayout();
        
        // 
        // themeGroupBox
        // 
        this.themeGroupBox.Controls.Add(this.systemThemeRadio);
        this.themeGroupBox.Controls.Add(this.lightThemeRadio);
        this.themeGroupBox.Controls.Add(this.darkThemeRadio);
        this.themeGroupBox.Location = new System.Drawing.Point(12, 12);
        this.themeGroupBox.Name = "themeGroupBox";
        this.themeGroupBox.Size = new System.Drawing.Size(360, 120);
        this.themeGroupBox.TabIndex = 0;
        this.themeGroupBox.TabStop = false;
        this.themeGroupBox.Text = "Theme";
        
        // 
        // systemThemeRadio
        // 
        this.systemThemeRadio.AutoSize = true;
        this.systemThemeRadio.Location = new System.Drawing.Point(15, 25);
        this.systemThemeRadio.Name = "systemThemeRadio";
        this.systemThemeRadio.Size = new System.Drawing.Size(60, 19);
        this.systemThemeRadio.TabIndex = 0;
        this.systemThemeRadio.TabStop = true;
        this.systemThemeRadio.Text = "System";
        this.systemThemeRadio.UseVisualStyleBackColor = true;
        this.systemThemeRadio.CheckedChanged += SystemThemeRadio_CheckedChanged;
        
        // 
        // lightThemeRadio
        // 
        this.lightThemeRadio.AutoSize = true;
        this.lightThemeRadio.Location = new System.Drawing.Point(15, 50);
        this.lightThemeRadio.Name = "lightThemeRadio";
        this.lightThemeRadio.Size = new System.Drawing.Size(52, 19);
        this.lightThemeRadio.TabIndex = 1;
        this.lightThemeRadio.Text = "Light";
        this.lightThemeRadio.UseVisualStyleBackColor = true;
        this.lightThemeRadio.CheckedChanged += LightThemeRadio_CheckedChanged;
        
        // 
        // darkThemeRadio
        // 
        this.darkThemeRadio.AutoSize = true;
        this.darkThemeRadio.Location = new System.Drawing.Point(15, 75);
        this.darkThemeRadio.Name = "darkThemeRadio";
        this.darkThemeRadio.Size = new System.Drawing.Size(51, 19);
        this.darkThemeRadio.TabIndex = 2;
        this.darkThemeRadio.Text = "Dark";
        this.darkThemeRadio.UseVisualStyleBackColor = true;
        this.darkThemeRadio.CheckedChanged += DarkThemeRadio_CheckedChanged;
        
        // 
        // okButton
        // 
        this.okButton.DialogResult = DialogResult.OK;
        this.okButton.Location = new System.Drawing.Point(216, 150);
        this.okButton.Name = "okButton";
        this.okButton.Size = new System.Drawing.Size(75, 23);
        this.okButton.TabIndex = 1;
        this.okButton.Text = "OK";
        this.okButton.UseVisualStyleBackColor = true;
        this.okButton.Click += OkButton_Click;
        
        // 
        // cancelButton
        // 
        this.cancelButton.DialogResult = DialogResult.Cancel;
        this.cancelButton.Location = new System.Drawing.Point(297, 150);
        this.cancelButton.Name = "cancelButton";
        this.cancelButton.Size = new System.Drawing.Size(75, 23);
        this.cancelButton.TabIndex = 2;
        this.cancelButton.Text = "Cancel";
        this.cancelButton.UseVisualStyleBackColor = true;
        
        // 
        // SettingsDialog
        // 
        this.AcceptButton = this.okButton;
        this.CancelButton = this.cancelButton;
        this.ClientSize = new System.Drawing.Size(384, 185);
        this.Controls.Add(this.cancelButton);
        this.Controls.Add(this.okButton);
        this.Controls.Add(this.themeGroupBox);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "SettingsDialog";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Settings";
        this.themeGroupBox.ResumeLayout(false);
        this.themeGroupBox.PerformLayout();
        this.ResumeLayout(false);
    }

    /// <summary>
    /// Loads current settings into the dialog.
    /// </summary>
    private void LoadSettings()
    {
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
    }

    /// <summary>
    /// Applies the current theme to the dialog.
    /// </summary>
    private void ApplyTheme()
    {
        var theme = ThemeManager.CurrentThemeData;
        
        this.BackColor = theme.BackgroundColor;
        this.ForeColor = theme.ForegroundColor;
        
        themeGroupBox.BackColor = theme.BackgroundColor;
        themeGroupBox.ForeColor = theme.ForegroundColor;
        
        systemThemeRadio.BackColor = theme.BackgroundColor;
        systemThemeRadio.ForeColor = theme.ForegroundColor;
        
        lightThemeRadio.BackColor = theme.BackgroundColor;
        lightThemeRadio.ForeColor = theme.ForegroundColor;
        
        darkThemeRadio.BackColor = theme.BackgroundColor;
        darkThemeRadio.ForeColor = theme.ForegroundColor;
        
        okButton.BackColor = theme.ControlBackgroundColor;
        okButton.ForeColor = theme.ControlForegroundColor;
        okButton.FlatStyle = FlatStyle.Flat;
        
        cancelButton.BackColor = theme.ControlBackgroundColor;
        cancelButton.ForeColor = theme.ControlForegroundColor;
        cancelButton.FlatStyle = FlatStyle.Flat;
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

    private void OkButton_Click(object? sender, EventArgs e)
    {
        // Settings are already applied when radio buttons change
        // TODO: Save other settings here when implemented
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}


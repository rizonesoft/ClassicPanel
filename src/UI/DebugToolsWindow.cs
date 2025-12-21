using ClassicPanel.Core;
using ClassicPanel.Core.Performance;
using ClassicPanel.Core.Theme;
using ClassicPanel.Icons;
using System.Diagnostics;
using System.Text;

namespace ClassicPanel.UI;

/// <summary>
/// Debug tools window providing console output, log viewer, and performance metrics.
/// Similar to Chrome DevTools console interface.
/// </summary>
public partial class DebugToolsWindow : Form
{
    private TabControl _tabControl;
    private RichTextBox _consoleTextBox;
    private RichTextBox _logViewerTextBox;
    private Panel _metricsPanel;
    private System.Windows.Forms.Timer _refreshTimer;
    private bool _isAutoScroll = true;
    private LogLevel _currentLogFilter = LogLevel.Info;
    private CheckBox _autoScrollCheckBox;
    private ComboBox _logLevelComboBox;
    private Button _clearConsoleButton;
    private Button _clearLogButton;
    private Button _exportLogButton;

    public DebugToolsWindow()
    {
        InitializeComponent();
        SetupTabs();
        SetupConsole();
        SetupLogViewer();
        SetupMetrics();
        SetupRefreshTimer();
        ApplyTheme();

        // Start log capture if not already started
        if (!DebugLogCapture.IsCapturing)
        {
            DebugLogCapture.StartCapture();
        }

        // Subscribe to new log entries
        DebugLogCapture.OnLogEntry += OnLogEntryReceived;
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // Window properties
        this.Text = "Debug Tools - ClassicPanel";
        this.Size = new Size(1000, 700);
        this.MinimumSize = new Size(600, 400);
        this.StartPosition = FormStartPosition.CenterParent;
        this.WindowState = FormWindowState.Normal;

        // Tab control
        _tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F)
        };

        this.Controls.Add(_tabControl);
        this.ResumeLayout(false);
    }

    private void SetupTabs()
    {
        // Console tab
        var consoleTab = new TabPage("Console");
        _tabControl.TabPages.Add(consoleTab);

        // Log Viewer tab
        var logViewerTab = new TabPage("Log Viewer");
        _tabControl.TabPages.Add(logViewerTab);

        // Metrics tab
        var metricsTab = new TabPage("Metrics");
        _tabControl.TabPages.Add(metricsTab);
    }

    private void SetupConsole()
    {
        var consoleTab = _tabControl.TabPages[0];
        var consolePanel = new Panel { Dock = DockStyle.Fill };
        consoleTab.Controls.Add(consolePanel);

        // Toolbar for console
        var consoleToolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 35,
            Padding = new Padding(5)
        };
        consolePanel.Controls.Add(consoleToolbar);

        _autoScrollCheckBox = new CheckBox
        {
            Text = "Auto-scroll",
            Checked = true,
            AutoSize = true,
            Location = new Point(5, 8)
        };
        _autoScrollCheckBox.CheckedChanged += (s, e) => _isAutoScroll = _autoScrollCheckBox.Checked;
        consoleToolbar.Controls.Add(_autoScrollCheckBox);

        _clearConsoleButton = new Button
        {
            Text = "Clear",
            Size = new Size(75, 23),
            Location = new Point(120, 5)
        };
        _clearConsoleButton.Click += (s, e) => _consoleTextBox.Clear();
        consoleToolbar.Controls.Add(_clearConsoleButton);

        // Console text box
        _consoleTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9F),
            ReadOnly = true,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.FromArgb(212, 212, 212),
            DetectUrls = false
        };
        consolePanel.Controls.Add(_consoleTextBox);

        // Load existing log entries
        RefreshConsole();
    }

    private void SetupLogViewer()
    {
        var logViewerTab = _tabControl.TabPages[1];
        var logViewerPanel = new Panel { Dock = DockStyle.Fill };
        logViewerTab.Controls.Add(logViewerPanel);

        // Toolbar for log viewer
        var logToolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 35,
            Padding = new Padding(5)
        };
        logViewerPanel.Controls.Add(logToolbar);

        var logLevelLabel = new Label
        {
            Text = "Filter:",
            AutoSize = true,
            Location = new Point(5, 10)
        };
        logToolbar.Controls.Add(logLevelLabel);

        _logLevelComboBox = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Size = new Size(100, 23),
            Location = new Point(50, 7)
        };
        _logLevelComboBox.Items.AddRange(new[] { "All", "Info", "Warning", "Error", "Critical" });
        _logLevelComboBox.SelectedIndex = 0;
        _logLevelComboBox.SelectedIndexChanged += (s, e) =>
        {
            _currentLogFilter = _logLevelComboBox.SelectedIndex switch
            {
                0 => LogLevel.Info, // All - show all
                1 => LogLevel.Info,
                2 => LogLevel.Warning,
                3 => LogLevel.Error,
                4 => LogLevel.Critical,
                _ => LogLevel.Info
            };
            RefreshLogViewer();
        };
        logToolbar.Controls.Add(_logLevelComboBox);

        _clearLogButton = new Button
        {
            Text = "Clear",
            Size = new Size(75, 23),
            Location = new Point(160, 5)
        };
        _clearLogButton.Click += (s, e) =>
        {
            DebugLogCapture.Clear();
            RefreshLogViewer();
        };
        logToolbar.Controls.Add(_clearLogButton);

        _exportLogButton = new Button
        {
            Text = "Export...",
            Size = new Size(75, 23),
            Location = new Point(245, 5)
        };
        _exportLogButton.Click += (s, e) => ExportLog();
        logToolbar.Controls.Add(_exportLogButton);

        // Log viewer text box
        _logViewerTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9F),
            ReadOnly = true,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.FromArgb(212, 212, 212),
            DetectUrls = false
        };
        logViewerPanel.Controls.Add(_logViewerTextBox);

        RefreshLogViewer();
    }

    private void SetupMetrics()
    {
        var metricsTab = _tabControl.TabPages[2];
        _metricsPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(10)
        };
        metricsTab.Controls.Add(_metricsPanel);

        RefreshMetrics();
    }

    private void SetupRefreshTimer()
    {
        _refreshTimer = new System.Windows.Forms.Timer
        {
            Interval = 1000, // Refresh every second
            Enabled = true
        };
        _refreshTimer.Tick += (s, e) =>
        {
            RefreshMetrics();
            if (_isAutoScroll)
            {
                RefreshConsole();
            }
        };
    }

    private void RefreshConsole()
    {
        if (_consoleTextBox == null)
            return;

        var entries = DebugLogCapture.LogEntries;
        var sb = new StringBuilder();
        
        // Show last 1000 entries
        var entriesToShow = entries.Count > 1000 ? entries.Skip(entries.Count - 1000) : entries;
        foreach (var entry in entriesToShow)
        {
            AppendLogEntry(sb, entry);
        }

        var newText = sb.ToString();
        if (newText != _consoleTextBox.Text)
        {
            var scrollPos = _consoleTextBox.SelectionStart;
            _consoleTextBox.Text = newText;
            
            if (_isAutoScroll)
            {
                _consoleTextBox.SelectionStart = _consoleTextBox.Text.Length;
                _consoleTextBox.ScrollToCaret();
            }
            else
            {
                _consoleTextBox.SelectionStart = Math.Min(scrollPos, _consoleTextBox.Text.Length);
            }
        }
    }

    private void RefreshLogViewer()
    {
        if (_logViewerTextBox == null)
            return;

        var entries = _logLevelComboBox.SelectedIndex == 0
            ? DebugLogCapture.LogEntries
            : DebugLogCapture.GetEntriesByLevel(_currentLogFilter);

        var sb = new StringBuilder();
        foreach (var entry in entries)
        {
            AppendLogEntry(sb, entry);
        }

        _logViewerTextBox.Text = sb.ToString();
        _logViewerTextBox.SelectionStart = _logViewerTextBox.Text.Length;
        _logViewerTextBox.ScrollToCaret();
    }

    private void RefreshMetrics()
    {
        if (_metricsPanel == null)
            return;

        _metricsPanel.Controls.Clear();

        var metrics = PerformanceMonitor.GetMetrics();
        var y = 10;

        // Performance Metrics
        AddMetricLabel(_metricsPanel, "Performance Metrics", y, true);
        y += 25;

        AddMetricLabel(_metricsPanel, $"Startup Time: {metrics.StartupTimeMs} ms", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Memory Usage: {metrics.MemoryUsageMB:F2} MB", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Peak Memory: {metrics.PeakMemoryUsageMB:F2} MB", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"CPU Usage: {metrics.CpuUsagePercent:F1}%", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Operations Tracked: {metrics.OperationCount}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Average Operation Time: {metrics.AverageOperationTimeMs:F2} ms", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Slowest Operation: {metrics.SlowestOperationTimeMs} ms", y);
        y += 30;

        // Log Statistics
        AddMetricLabel(_metricsPanel, "Log Statistics", y, true);
        y += 25;

        var logEntries = DebugLogCapture.LogEntries;
        var infoCount = logEntries.Count(e => e.Level == LogLevel.Info);
        var warningCount = logEntries.Count(e => e.Level == LogLevel.Warning);
        var errorCount = logEntries.Count(e => e.Level == LogLevel.Error);
        var criticalCount = logEntries.Count(e => e.Level == LogLevel.Critical);

        AddMetricLabel(_metricsPanel, $"Total Log Entries: {DebugLogCapture.EntryCount}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Info: {infoCount}", y, false, Color.LightBlue);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Warnings: {warningCount}", y, false, Color.Orange);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Errors: {errorCount}", y, false, Color.Red);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Critical: {criticalCount}", y, false, Color.DarkRed);
        y += 30;

        // System Information
        AddMetricLabel(_metricsPanel, "System Information", y, true);
        y += 25;

        AddMetricLabel(_metricsPanel, $"OS Version: {Environment.OSVersion}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $".NET Version: {Environment.Version}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Machine Name: {Environment.MachineName}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"User Name: {Environment.UserName}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Processor Count: {Environment.ProcessorCount}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"Working Set: {Environment.WorkingSet / 1024 / 1024:F2} MB", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"64-bit Process: {Environment.Is64BitProcess}", y);
        y += 20;

        AddMetricLabel(_metricsPanel, $"64-bit OS: {Environment.Is64BitOperatingSystem}", y);
    }

    private void AddMetricLabel(Panel panel, string text, int y, bool isHeader = false, Color? color = null)
    {
        var label = new Label
        {
            Text = text,
            AutoSize = true,
            Location = new Point(10, y),
            Font = isHeader ? new Font("Segoe UI", 10F, FontStyle.Bold) : new Font("Segoe UI", 9F),
            ForeColor = color ?? (isHeader ? Color.White : Color.FromArgb(212, 212, 212))
        };
        panel.Controls.Add(label);
    }

    private void AppendLogEntry(StringBuilder sb, LogEntry entry)
    {
        var color = entry.Level switch
        {
            LogLevel.Info => Color.FromArgb(212, 212, 212),
            LogLevel.Warning => Color.Orange,
            LogLevel.Error => Color.Red,
            LogLevel.Critical => Color.DarkRed,
            _ => Color.FromArgb(212, 212, 212)
        };

        sb.AppendLine(entry.FormattedMessage);
    }

    private void OnLogEntryReceived(LogEntry entry)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => OnLogEntryReceived(entry)));
            return;
        }

        if (_isAutoScroll && _consoleTextBox != null)
        {
            var color = entry.Level switch
            {
                LogLevel.Info => Color.FromArgb(212, 212, 212),
                LogLevel.Warning => Color.Orange,
                LogLevel.Error => Color.Red,
                LogLevel.Critical => Color.DarkRed,
                _ => Color.FromArgb(212, 212, 212)
            };

            _consoleTextBox.SelectionStart = _consoleTextBox.Text.Length;
            _consoleTextBox.SelectionColor = color;
            _consoleTextBox.AppendText(entry.FormattedMessage + Environment.NewLine);
            _consoleTextBox.SelectionStart = _consoleTextBox.Text.Length;
            _consoleTextBox.ScrollToCaret();
        }
    }

    private void ExportLog()
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            FileName = $"ClassicPanel_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
            DefaultExt = "txt"
        };

        if (saveDialog.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var entry in DebugLogCapture.LogEntries)
                {
                    sb.AppendLine(entry.FormattedMessage);
                }

                File.WriteAllText(saveDialog.FileName, sb.ToString());
                MessageBox.Show(this, "Log exported successfully.", "Export Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowError("Failed to export log file", ex, this);
            }
        }
    }

    private void ApplyTheme()
    {
        var isDarkMode = string.Equals(ThemeManager.GetEffectiveTheme(), AppConstants.DarkTheme, StringComparison.OrdinalIgnoreCase);
        var theme = ThemeManager.CurrentThemeData;

        this.BackColor = theme.BackgroundColor;
        this.ForeColor = theme.ForegroundColor;

        if (_consoleTextBox != null)
        {
            _consoleTextBox.BackColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            _consoleTextBox.ForeColor = isDarkMode ? Color.FromArgb(212, 212, 212) : Color.Black;
        }

        if (_logViewerTextBox != null)
        {
            _logViewerTextBox.BackColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            _logViewerTextBox.ForeColor = isDarkMode ? Color.FromArgb(212, 212, 212) : Color.Black;
        }

        if (_metricsPanel != null)
        {
            _metricsPanel.BackColor = theme.BackgroundColor;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Don't dispose, just hide
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
        }
        else
        {
            base.OnFormClosing(e);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            DebugLogCapture.OnLogEntry -= OnLogEntryReceived;
        }
        base.Dispose(disposing);
    }
}


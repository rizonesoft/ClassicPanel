using ClassicPanel.Core;
using ClassicPanel.Core.Localization;
using ClassicPanel.Core.Performance;
using ClassicPanel.Core.Theme;
using ClassicPanel.UI;
using System.Reflection;

namespace ClassicPanel;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        try
        {
            // Initialize performance monitoring first
            PerformanceMonitor.Initialize();

            // Start debug log capture
            DebugLogCapture.StartCapture();

            // Validate platform requirements (Windows 10/11, 64-bit)
            // This will show an error dialog and exit if requirements aren't met
            if (!PlatformValidator.ValidatePlatform())
            {
                return; // Platform validation failed and showed error dialog
            }

            // Initialize localization
            try
            {
                LocalizationManager.Initialize();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("Failed to initialize localization", ex, "Program.Main");
                // Continue - localization failure is not critical
            }

            // Initialize theme system (defaults to System mode to follow Windows theme)
            try
            {
                ThemeManager.Initialize();
                ThemeManager.CurrentTheme = AppConstants.SystemTheme;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("Failed to initialize theme system", ex, "Program.Main");
                // Continue - theme failure is not critical
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Set high DPI mode (PerMonitorV2 for best high-DPI support)
            // This replaces the DPI settings in the manifest
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            
            Application.Run(new MainWindow());
        }
        catch (Exception ex)
        {
            // Critical error during application startup
            ErrorLogger.ShowError("A critical error occurred during application startup", ex, null, "Program.Main");
            Environment.Exit(1);
        }
    }
}


using ClassicPanel.Core;
using ClassicPanel.Core.Localization;
using ClassicPanel.UI;

namespace ClassicPanel;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Validate platform requirements (Windows 10/11, 64-bit)
        // This will show an error dialog and exit if requirements aren't met
        PlatformValidator.ValidatePlatform();

        // Initialize localization
        LocalizationManager.Initialize();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        // Set high DPI mode (PerMonitorV2 for best high-DPI support)
        // This replaces the DPI settings in the manifest
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        
        Application.Run(new MainWindow());
    }
}


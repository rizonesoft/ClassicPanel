using ClassicPanel.Core;
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

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainWindow());
    }
}


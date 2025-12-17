namespace ClassicPanel.Interop;

/// <summary>
/// Provides functionality for detecting available UI frameworks at runtime.
/// </summary>
public static class FrameworkDetector
{
    /// <summary>
    /// Detects all available UI frameworks.
    /// </summary>
    /// <returns>A list of available framework names.</returns>
    public static IReadOnlyList<string> DetectAvailableFrameworks()
    {
        var frameworks = new List<string>();

        // WinForms is always available on Windows
        frameworks.Add("WinForms");

        // Check if WPF is available
        try
        {
            // Try to load WPF types to see if it's available
            var wpfWindowType = Type.GetType("System.Windows.Window, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            if (wpfWindowType != null)
            {
                frameworks.Add("WPF");
            }
        }
        catch
        {
            // WPF not available
        }

        return frameworks.AsReadOnly();
    }

    /// <summary>
    /// Checks if a specific framework is available.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to check.</param>
    /// <returns>True if the framework is available; otherwise, false.</returns>
    public static bool IsFrameworkAvailable(string frameworkName)
    {
        if (string.IsNullOrWhiteSpace(frameworkName))
            return false;

        var availableFrameworks = DetectAvailableFrameworks();
        return availableFrameworks.Contains(frameworkName, StringComparer.OrdinalIgnoreCase);
    }
}


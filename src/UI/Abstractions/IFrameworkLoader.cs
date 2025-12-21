namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an interface for loading UI framework implementations as plugins.
/// </summary>
public interface IFrameworkLoader
{
    /// <summary>
    /// Gets all available UI framework providers.
    /// </summary>
    /// <returns>A list of available framework names.</returns>
    IReadOnlyList<string> GetAvailableFrameworks();

    /// <summary>
    /// Loads a UI framework provider by name.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to load (e.g., "WinForms", "WPF").</param>
    /// <returns>The loaded UI provider, or null if the framework could not be loaded.</returns>
    IUIProvider? LoadFramework(string frameworkName);

    /// <summary>
    /// Loads the default UI framework provider.
    /// </summary>
    /// <returns>The default UI provider, or null if no default is available.</returns>
    IUIProvider? LoadDefaultFramework();

    /// <summary>
    /// Registers a UI framework provider.
    /// </summary>
    /// <param name="provider">The UI provider to register.</param>
    void RegisterFramework(IUIProvider provider);

    /// <summary>
    /// Unregisters a UI framework provider.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to unregister.</param>
    /// <returns>True if the framework was unregistered; otherwise, false.</returns>
    bool UnregisterFramework(string frameworkName);
}









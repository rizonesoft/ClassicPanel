namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Provides a default implementation of <see cref="IFrameworkLoader"/> for loading UI framework providers.
/// </summary>
public class FrameworkLoader : IFrameworkLoader
{
    private readonly Dictionary<string, IUIProvider> _providers = new(StringComparer.OrdinalIgnoreCase);
    private string? _defaultFrameworkName;

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameworkLoader"/> class.
    /// </summary>
    public FrameworkLoader()
    {
    }

    /// <summary>
    /// Gets all available UI framework providers.
    /// </summary>
    /// <returns>A list of available framework names.</returns>
    public IReadOnlyList<string> GetAvailableFrameworks()
    {
        lock (_providers)
        {
            return _providers.Keys.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Loads a UI framework provider by name.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to load (e.g., "WinForms", "WPF").</param>
    /// <returns>The loaded UI provider, or null if the framework could not be loaded.</returns>
    public IUIProvider? LoadFramework(string frameworkName)
    {
        if (string.IsNullOrWhiteSpace(frameworkName))
            return null;

        lock (_providers)
        {
            _providers.TryGetValue(frameworkName, out var provider);
            return provider;
        }
    }

    /// <summary>
    /// Loads the default UI framework provider.
    /// </summary>
    /// <returns>The default UI provider, or null if no default is available.</returns>
    public IUIProvider? LoadDefaultFramework()
    {
        if (_defaultFrameworkName == null)
            return null;

        return LoadFramework(_defaultFrameworkName);
    }

    /// <summary>
    /// Registers a UI framework provider.
    /// </summary>
    /// <param name="provider">The UI provider to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when provider is null.</exception>
    public void RegisterFramework(IUIProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        if (string.IsNullOrWhiteSpace(provider.FrameworkName))
            throw new ArgumentException("Provider must have a valid FrameworkName.", nameof(provider));

        lock (_providers)
        {
            _providers[provider.FrameworkName] = provider;

            // Set as default if it's the first one registered
            if (_defaultFrameworkName == null)
            {
                _defaultFrameworkName = provider.FrameworkName;
            }
        }
    }

    /// <summary>
    /// Sets the default framework name.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to set as default.</param>
    /// <exception cref="ArgumentException">Thrown when the framework is not registered.</exception>
    public void SetDefaultFramework(string frameworkName)
    {
        if (string.IsNullOrWhiteSpace(frameworkName))
            throw new ArgumentException("Framework name cannot be null or empty.", nameof(frameworkName));

        lock (_providers)
        {
            if (!_providers.ContainsKey(frameworkName))
                throw new ArgumentException($"Framework '{frameworkName}' is not registered.", nameof(frameworkName));

            _defaultFrameworkName = frameworkName;
        }
    }

    /// <summary>
    /// Unregisters a UI framework provider.
    /// </summary>
    /// <param name="frameworkName">The name of the framework to unregister.</param>
    /// <returns>True if the framework was unregistered; otherwise, false.</returns>
    public bool UnregisterFramework(string frameworkName)
    {
        if (string.IsNullOrWhiteSpace(frameworkName))
            return false;

        lock (_providers)
        {
            bool removed = _providers.Remove(frameworkName);

            // If we removed the default framework, set a new default
            if (removed && _defaultFrameworkName == frameworkName)
            {
                _defaultFrameworkName = _providers.Keys.FirstOrDefault();
            }

            return removed;
        }
    }
}









# Localization Infrastructure

This folder contains the localization and internationalization infrastructure for ClassicPanel.

## Current Status

The localization infrastructure is set up and ready for use. Resource files (`.resx`) will be added as needed for different languages.

## Structure

```
Localization/
├── LocalizationManager.cs    # Main localization manager
└── README.md                 # This file
```

## Usage

```csharp
// Get a localized string
string title = LocalizationManager.GetString("MainWindow.Title", "ClassicPanel");

// Get a formatted localized string
string message = LocalizationManager.GetString("Message.ItemsFound", "{0} items found", itemCount);

// Change culture
LocalizationManager.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

// Or try to set from string
LocalizationManager.TrySetCulture("en-GB");
```

## Future Implementation

When resource files are added:
1. Create `Resources/Strings.resx` (default/fallback)
2. Create `Resources/Strings.en-US.resx` (English - US)
3. Create `Resources/Strings.fr-FR.resx` (French - France)
4. Add more languages as needed

The `LocalizationManager` will automatically load the appropriate resource file based on the current culture.


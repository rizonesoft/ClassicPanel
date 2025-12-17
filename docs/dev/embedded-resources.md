# Embedded Resources

## Overview

ClassicPanel embeds SVG icon files directly into the main assembly to avoid needing separate resource files in the output directory. This ensures a clean single-file deployment.

## Resource Naming Format

**Format**: `ClassicPanel.Resources.Actions.{filename}.svg`

**Examples**:
- `ClassicPanel.Resources.Actions.refresh.svg`
- `ClassicPanel.Resources.Actions.settings.svg`
- `ClassicPanel.Resources.Actions.view-large-icons.svg`

## Configuration

Resources are configured in `src/ClassicPanel.csproj`:

```xml
<EmbeddedResource Include="..\resources\actions\*.svg">
  <LogicalName>ClassicPanel.Resources.Actions.%(Filename)%(Extension)</LogicalName>
</EmbeddedResource>
```

The `LogicalName` attribute explicitly defines the embedded resource name format, ensuring consistency regardless of build configuration.

## Loading Resources

Resources are loaded using the `SvgFileLoader` class in `ClassicPanel.Icons`:

```csharp
// Get the resource name
var resourceName = SvgFileLoader.GetEmbeddedResourceName("refresh");
// Returns: "ClassicPanel.Resources.Actions.refresh.svg"

// Load the resource
var assembly = Assembly.GetEntryAssembly();
using var stream = assembly.GetManifestResourceStream(resourceName);
```

## Implementation Details

- **No Runtime Detection**: The resource name format is hardcoded for consistency and performance
- **SkiaSharp Rendering**: SVG files are rendered using SkiaSharp for accurate rendering with theme support
- **Theme Support**: Icons automatically adapt to light/dark themes via `SvgFileLoader.RenderSvgFileThemed()`
- **Caching**: Rendered bitmaps are cached for performance

## Adding New SVG Icons

1. Place the SVG file in `resources/actions/` directory
2. The file will be automatically embedded during build
3. Use `SvgFileLoader.RenderSvgFileThemed(fileName, size, isDarkMode)` to render the icon

## Troubleshooting

If icons are missing:
1. Verify the SVG file exists in `resources/actions/`
2. Check that the resource name matches the format: `ClassicPanel.Resources.Actions.{filename}.svg`
3. Use `Assembly.GetEntryAssembly().GetManifestResourceNames()` to list all embedded resources
4. Check debug output for resource loading errors


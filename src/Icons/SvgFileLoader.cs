using System.Drawing;
using System.IO;
using System.Reflection;
using SkiaSharp;
using Svg.Skia;

namespace ClassicPanel.Icons;

/// <summary>
/// Loads and renders SVG files to bitmaps using SkiaSharp for accurate SVG rendering.
/// Supports full SVG 1.1/2.0 features including arcs, gradients, and complex paths.
/// </summary>
public static class SvgFileLoader
{
    private static readonly Dictionary<string, Bitmap> _cache = new();
    private static readonly object _cacheLock = new();

    /// <summary>
    /// Gets the embedded resource name for an SVG file.
    /// Resources are embedded with the format: ClassicPanel.Resources.Actions.{filename}.svg
    /// </summary>
    /// <param name="fileName">The SVG file name (e.g., "refresh.svg").</param>
    /// <returns>The embedded resource name.</returns>
    private static string GetEmbeddedResourceName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

        if (!fileName.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            fileName += ".svg";

        return $"ClassicPanel.Resources.Actions.{fileName}";
    }

    /// <summary>
    /// Reads an embedded SVG resource as a string.
    /// </summary>
    /// <param name="resourceName">The embedded resource name (format: ClassicPanel.Resources.Actions.{filename}.svg).</param>
    /// <returns>The SVG content as a string.</returns>
    private static string ReadEmbeddedSvg(string resourceName)
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
        {
            throw new InvalidOperationException("Cannot get entry assembly");
        }
        
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        
        // If not found, provide helpful error message with available resources
        var availableResources = assembly.GetManifestResourceNames();
        var availableList = availableResources.Length > 0 
            ? string.Join("\n  - ", availableResources) 
            : "(no resources found)";
        
        throw new FileNotFoundException(
            $"Embedded resource not found: {resourceName}\n" +
            $"Available resources:\n  - {availableList}");
    }

    /// <summary>
    /// Renders an SVG file to a bitmap with the specified size and color.
    /// Supports both embedded resources and file paths (for backward compatibility).
    /// </summary>
    /// <param name="svgFilePath">Path to the SVG file or embedded resource name.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering (replaces currentColor in SVG).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgFile(string svgFilePath, int size, Color color)
    {
        if (string.IsNullOrWhiteSpace(svgFilePath))
            throw new ArgumentException("SVG file path cannot be null or empty.", nameof(svgFilePath));

        if (size <= 0)
            throw new ArgumentException("Size must be greater than zero.", nameof(size));

        // Create cache key
        var cacheKey = $"{svgFilePath}|{size}|{color.ToArgb()}";

        lock (_cacheLock)
        {
            if (_cache.TryGetValue(cacheKey, out var cached))
            {
                return new Bitmap(cached); // Return a copy
            }
        }

        // Load and render SVG file using SkiaSharp
        Bitmap bitmap;
        if (File.Exists(svgFilePath))
        {
            // File-based (for development/testing)
            bitmap = RenderSvgWithSkiaSharp(svgFilePath, size, color);
        }
        else
        {
            // Try as embedded resource
            try
            {
                var resourceName = GetEmbeddedResourceName(Path.GetFileName(svgFilePath));
                var svgContent = ReadEmbeddedSvg(resourceName);
                bitmap = RenderSvgFromString(svgContent, size, color);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"SVG file not found: {svgFilePath}", svgFilePath);
            }
        }

        // Cache the result
        lock (_cacheLock)
        {
            if (!_cache.ContainsKey(cacheKey))
            {
                _cache[cacheKey] = new Bitmap(bitmap);
            }
        }

        return bitmap;
    }

    /// <summary>
    /// Renders an embedded SVG file with theme-aware colors.
    /// </summary>
    /// <param name="fileName">The SVG file name (e.g., "refresh.svg").</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="isDarkMode">True for dark mode, false for light mode.</param>
    /// <returns>A bitmap containing the rendered icon, or a blank bitmap if file not found.</returns>
    public static Bitmap RenderSvgFileThemed(string fileName, int size, bool isDarkMode)
    {
        try
        {
            var resourceName = GetEmbeddedResourceName(fileName);
            var svgContent = ReadEmbeddedSvg(resourceName);
            var color = isDarkMode ? Color.White : Color.Black;
            return RenderSvgFromString(svgContent, size, color);
        }
        catch (FileNotFoundException ex)
        {
            // Log error but don't crash - return blank bitmap
            System.Diagnostics.Debug.WriteLine($"[SvgFileLoader] Embedded resource not found: {fileName}. {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SvgFileLoader] Error loading embedded resource {fileName}: {ex.Message}");
        }
        
        // Return a blank bitmap if SVG file not found (prevents crash)
        var blankBitmap = new Bitmap(size, size);
        using (var g = Graphics.FromImage(blankBitmap))
        {
            g.Clear(Color.Transparent);
        }
        return blankBitmap;
    }


    /// <summary>
    /// Renders an SVG string using SkiaSharp library.
    /// </summary>
    /// <param name="svgContent">The SVG content as a string.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering (replaces currentColor in SVG).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    private static Bitmap RenderSvgFromString(string svgContent, int size, Color color)
    {
        // Replace currentColor with specified color
        var colorHex = ColorToHex(color);
        svgContent = svgContent.Replace("currentColor", colorHex);
        
        // Create a temporary file with modified content (SKSvg needs a file or stream)
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, svgContent);
            
            // Load SVG using SkiaSharp
            using var svg = new SKSvg();
            svg.Load(tempFile);
            
            if (svg.Picture == null)
            {
                throw new InvalidOperationException("Failed to load SVG content");
            }
            
            // Get SVG dimensions
            var svgBounds = svg.Picture.CullRect;
            var svgWidth = svgBounds.Width;
            var svgHeight = svgBounds.Height;
            
            if (svgWidth <= 0 || svgHeight <= 0)
            {
                svgWidth = size;
                svgHeight = size;
            }
            
            // Calculate scale to fit the bitmap while maintaining aspect ratio
            var scaleX = size / svgWidth;
            var scaleY = size / svgHeight;
            var scale = Math.Min(scaleX, scaleY);
            
            // Calculate centered position
            var scaledWidth = svgWidth * scale;
            var scaledHeight = svgHeight * scale;
            var offsetX = (size - scaledWidth) / 2;
            var offsetY = (size - scaledHeight) / 2;
            
            // Create SkiaSharp surface and render
            var info = new SKImageInfo(size, size, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            
            // Clear with transparent background
            canvas.Clear(SKColors.Transparent);
            
            // Translate and scale to center and fit the SVG
            canvas.Translate(offsetX, offsetY);
            canvas.Scale((float)scale);
            canvas.Translate((float)-svgBounds.Left, (float)-svgBounds.Top);
            
            // Render the SVG
            canvas.DrawPicture(svg.Picture);
            
            // Convert SkiaSharp image to System.Drawing.Bitmap
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = data.AsStream();
            
            return new Bitmap(stream);
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                try { File.Delete(tempFile); } catch { }
            }
        }
    }

    /// <summary>
    /// Renders an SVG file using SkiaSharp library (for file-based loading, backward compatibility).
    /// </summary>
    /// <param name="svgFilePath">Path to the SVG file.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering (replaces currentColor in SVG).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    private static Bitmap RenderSvgWithSkiaSharp(string svgFilePath, int size, Color color)
    {
        // Read SVG content from file
        var svgContent = File.ReadAllText(svgFilePath);
        return RenderSvgFromString(svgContent, size, color);
    }

    /// <summary>
    /// Converts a Color to hex string.
    /// </summary>
    private static string ColorToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public static void ClearCache()
    {
        lock (_cacheLock)
        {
            foreach (var bitmap in _cache.Values)
            {
                bitmap.Dispose();
            }
            _cache.Clear();
        }
    }
}


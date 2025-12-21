using System.Diagnostics;
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
        try
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
        catch (FileNotFoundException)
        {
            // Re-throw FileNotFoundException as-is
            throw;
        }
        catch (Exception ex)
        {
            // Log error and re-throw
            Debug.WriteLine($"[SvgFileLoader] Failed to read embedded SVG resource: {resourceName}. {ex.Message}");
            throw;
        }
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

        // Load and render SVG file using SkiaSharp with fallback strategies
        Bitmap bitmap;
        if (File.Exists(svgFilePath))
        {
            // File-based (for development/testing) with retry and fallback
            bitmap = RetryWithFallback(
                () => RenderSvgWithSkiaSharp(svgFilePath, size, color),
                () => CreateFallbackIcon(size, color), // Fallback to blank icon
                maxRetries: 2,
                context: $"SvgFileLoader.RenderSvgFile({Path.GetFileName(svgFilePath)})"
            );
        }
        else
        {
            // Try as embedded resource with fallback
            bitmap = TryWithFallback(
                () =>
                {
                    var resourceName = GetEmbeddedResourceName(Path.GetFileName(svgFilePath));
                    var svgContent = ReadEmbeddedSvg(resourceName);
                    return RenderSvgFromString(svgContent, size, color);
                },
                () => CreateFallbackIcon(size, color), // Fallback to blank icon
                context: $"SvgFileLoader.RenderSvgFile({Path.GetFileName(svgFilePath)})"
            );
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
        var color = isDarkMode ? Color.White : Color.Black;
        
        // Use fallback strategy - return blank icon if loading fails
        return TryWithFallback(
            () =>
            {
                var resourceName = GetEmbeddedResourceName(fileName);
                var svgContent = ReadEmbeddedSvg(resourceName);
                return RenderSvgFromString(svgContent, size, color);
            },
            () => CreateFallbackIcon(size, color), // Fallback to blank icon
            context: $"SvgFileLoader.RenderSvgFileThemed({fileName})"
        );
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
            // Retry file write operation for transient errors
            RetryOperation<object?>(
                () =>
                {
                    File.WriteAllText(tempFile, svgContent);
                    return null;
                },
                maxRetries: 2,
                context: "SvgFileLoader.RenderSvgFromString.WriteTempFile"
            );
            
            // Load SVG using SkiaSharp
            using var svg = new SKSvg();
            try
            {
                svg.Load(tempFile);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load SVG from temporary file: {tempFile}", ex);
            }
            
            if (svg.Picture == null)
            {
                throw new InvalidOperationException("Failed to load SVG content - Picture is null");
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
            if (surface == null)
            {
                throw new InvalidOperationException("Failed to create SkiaSharp surface");
            }
            
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
            if (data == null)
            {
                throw new InvalidOperationException("Failed to encode SVG image to PNG");
            }
            
            using var stream = data.AsStream();
            
            return new Bitmap(stream);
        }
        catch (Exception ex)
        {
            // Log error and re-throw
            Debug.WriteLine($"[SvgFileLoader] Failed to render SVG from string: {ex.Message}");
            throw;
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception ex)
                {
                    // Log but don't throw - cleanup failure is not critical
                    Debug.WriteLine($"[SvgFileLoader] Failed to delete temporary file: {tempFile}. {ex.Message}");
                }
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
        // Retry file read operation for transient errors
        var svgContent = RetryOperation(
            () => File.ReadAllText(svgFilePath),
            maxRetries: 2,
            context: $"SvgFileLoader.RenderSvgWithSkiaSharp({Path.GetFileName(svgFilePath)})"
        );
        
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

    #region Error Recovery Helpers

    /// <summary>
    /// Executes an operation with retry logic for transient file I/O errors.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation.</returns>
    private static T RetryOperation<T>(Func<T> operation, int maxRetries = 2, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Exception? lastException = null;
        int delay = 50; // Initial delay in milliseconds

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                return operation();
            }
            catch (IOException ex)
            {
                lastException = ex;

                // Check if this is a transient file error (sharing violation, lock violation, etc.)
                var errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex) & 0xFFFF;
                bool isTransient = errorCode == 32 || errorCode == 33 || errorCode == 5; // ERROR_SHARING_VIOLATION, ERROR_LOCK_VIOLATION, ERROR_ACCESS_DENIED

                if (!isTransient || attempt >= maxRetries)
                {
                    // Not transient or last attempt - throw
                    if (!string.IsNullOrEmpty(context))
                    {
                        Debug.WriteLine($"[SvgFileLoader] {context}: Operation failed after {attempt + 1} attempts: {ex.Message}");
                    }
                    throw;
                }

                // Wait before retrying (exponential backoff)
                Thread.Sleep(delay);
                delay = Math.Min(delay * 2, 500); // Max 500ms delay
            }
            catch (Exception ex)
            {
                // Non-IO exceptions are not retried
                if (!string.IsNullOrEmpty(context))
                {
                    Debug.WriteLine($"[SvgFileLoader] {context}: Operation failed: {ex.Message}");
                }
                throw;
            }
        }

        throw lastException ?? new InvalidOperationException("Retry operation failed with unknown error");
    }

    /// <summary>
    /// Executes an operation with retry logic and a fallback if all retries fail.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute with retry.</param>
    /// <param name="fallbackOperation">The fallback operation to execute if all retries fail.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the operation if successful, otherwise the result of the fallback operation.</returns>
    private static T RetryWithFallback<T>(Func<T> operation, Func<T> fallbackOperation, int maxRetries = 2, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(fallbackOperation);

        try
        {
            return RetryOperation(operation, maxRetries, context);
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(context))
            {
                Debug.WriteLine($"[SvgFileLoader] {context}: All retry attempts failed, using fallback: {ex.Message}");
            }

            try
            {
                return fallbackOperation();
            }
            catch (Exception fallbackEx)
            {
                if (!string.IsNullOrEmpty(context))
                {
                    Debug.WriteLine($"[SvgFileLoader] {context}: Fallback operation also failed: {fallbackEx.Message}");
                }
                throw;
            }
        }
    }

    /// <summary>
    /// Executes an operation with a fallback if it fails.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="fallbackOperation">The fallback operation to execute if primary fails.</param>
    /// <param name="context">Optional context for error logging.</param>
    /// <returns>The result of the primary operation if successful, otherwise the result of the fallback operation.</returns>
    private static T TryWithFallback<T>(Func<T> operation, Func<T> fallbackOperation, string? context = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(fallbackOperation);

        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrEmpty(context))
            {
                Debug.WriteLine($"[SvgFileLoader] {context}: Operation failed, using fallback: {ex.Message}");
            }

            try
            {
                return fallbackOperation();
            }
            catch (Exception fallbackEx)
            {
                if (!string.IsNullOrEmpty(context))
                {
                    Debug.WriteLine($"[SvgFileLoader] {context}: Fallback operation also failed: {fallbackEx.Message}");
                }
                throw;
            }
        }
    }

    /// <summary>
    /// Creates a fallback blank icon when SVG loading fails.
    /// </summary>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color for the icon (not used for blank icon, but kept for consistency).</param>
    /// <returns>A blank transparent bitmap.</returns>
    private static Bitmap CreateFallbackIcon(int size, Color color)
    {
        var blankBitmap = new Bitmap(size, size);
        using (var g = Graphics.FromImage(blankBitmap))
        {
            g.Clear(Color.Transparent);
        }
        return blankBitmap;
    }

    #endregion
}


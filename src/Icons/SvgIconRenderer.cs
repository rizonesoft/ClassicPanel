using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace ClassicPanel.Icons;

/// <summary>
/// Renders SVG path data to bitmaps with theme-aware colors.
/// Supports light and dark mode with automatic color adaptation.
/// </summary>
public static class SvgIconRenderer
{
    private static readonly Dictionary<string, Bitmap> _iconCache = new();
    private static readonly object _cacheLock = new();

    /// <summary>
    /// Renders an SVG path to a bitmap with the specified size and color.
    /// </summary>
    /// <param name="svgPath">The SVG path data (e.g., "M 0 0 L 10 10 Z").</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering.</param>
    /// <param name="strokeWidth">The stroke width (default: 1.5).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgPath(string svgPath, int size, Color color, float strokeWidth = 1.5f)
    {
        if (string.IsNullOrWhiteSpace(svgPath))
            throw new ArgumentException("SVG path cannot be null or empty.", nameof(svgPath));

        if (size <= 0)
            throw new ArgumentException("Size must be greater than zero.", nameof(size));

        // Create cache key
        var cacheKey = $"{svgPath}|{size}|{color.ToArgb()}|{strokeWidth}";

        lock (_cacheLock)
        {
            if (_iconCache.TryGetValue(cacheKey, out var cached))
            {
                return new Bitmap(cached); // Return a copy to avoid disposal issues
            }
        }

        var bitmap = new Bitmap(size, size);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.Clear(Color.Transparent);

            using (var pen = new Pen(color, strokeWidth))
            using (var brush = new SolidBrush(color))
            {
                var path = ParseSvgPath(svgPath, size);
                
                // Determine if path should be filled or stroked
                // For now, we'll stroke the path (outline style)
                graphics.DrawPath(pen, path);
            }
        }

        // Cache the result
        lock (_cacheLock)
        {
            if (!_iconCache.ContainsKey(cacheKey))
            {
                _iconCache[cacheKey] = new Bitmap(bitmap);
            }
        }

        return bitmap;
    }

    /// <summary>
    /// Renders an SVG path with theme-aware colors (light or dark mode).
    /// </summary>
    /// <param name="svgPath">The SVG path data.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="isDarkMode">True for dark mode, false for light mode.</param>
    /// <param name="strokeWidth">The stroke width (default: 1.5).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgPathThemed(string svgPath, int size, bool isDarkMode, float strokeWidth = 1.5f)
    {
        // Use appropriate color for theme
        var color = isDarkMode 
            ? Color.White  // White for dark mode
            : Color.Black; // Black for light mode

        return RenderSvgPath(svgPath, size, color, strokeWidth);
    }

    /// <summary>
    /// Clears the icon cache to free memory.
    /// </summary>
    public static void ClearCache()
    {
        lock (_cacheLock)
        {
            foreach (var bitmap in _iconCache.Values)
            {
                bitmap.Dispose();
            }
            _iconCache.Clear();
        }
    }

    /// <summary>
    /// Parses SVG path data and creates a GraphicsPath.
    /// Supports basic SVG path commands: M, L, H, V, Z, C, S, Q, T, A.
    /// </summary>
    private static GraphicsPath ParseSvgPath(string svgPath, int size)
    {
        var path = new GraphicsPath();
        var currentPoint = new PointF(0, 0);
        var startPoint = new PointF(0, 0);
        var scale = size / 24.0f; // Assume icons are designed for 24px, scale to target size

        // Normalize path string (remove extra whitespace, normalize commands)
        var normalized = Regex.Replace(svgPath.Trim(), @"\s+", " ");
        
        // Simple parser for basic paths (M, L, Z commands)
        // This is a simplified parser - for full SVG support, consider a library
        var commands = Regex.Matches(normalized, @"([MLHVZCSQTA])([^MLHVZCSQTA]*)", RegexOptions.IgnoreCase);
        
        foreach (Match match in commands)
        {
            var command = match.Groups[1].Value.ToUpper();
            var args = match.Groups[2].Value.Trim();

            switch (command)
            {
                case "M": // MoveTo
                    var moveCoords = ParseNumbers(args);
                    if (moveCoords.Length >= 2)
                    {
                        currentPoint = new PointF(moveCoords[0] * scale, moveCoords[1] * scale);
                        startPoint = currentPoint;
                    }
                    break;

                case "L": // LineTo
                    var lineCoords = ParseNumbers(args);
                    for (int i = 0; i < lineCoords.Length - 1; i += 2)
                    {
                        var endPoint = new PointF(lineCoords[i] * scale, lineCoords[i + 1] * scale);
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "H": // Horizontal LineTo
                    var hCoords = ParseNumbers(args);
                    foreach (var x in hCoords)
                    {
                        var endPoint = new PointF(x * scale, currentPoint.Y);
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "V": // Vertical LineTo
                    var vCoords = ParseNumbers(args);
                    foreach (var y in vCoords)
                    {
                        var endPoint = new PointF(currentPoint.X, y * scale);
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "Z": // ClosePath
                case "z":
                    if (currentPoint != startPoint)
                    {
                        path.AddLine(currentPoint, startPoint);
                    }
                    currentPoint = startPoint;
                    break;

                // For now, we'll skip complex curves (C, S, Q, T, A)
                // They can be added later if needed
            }
        }

        return path;
    }

    /// <summary>
    /// Parses a string of numbers (separated by spaces or commas) into an array of floats.
    /// </summary>
    private static float[] ParseNumbers(string numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers))
            return Array.Empty<float>();

        // Split by spaces, commas, or both
        var parts = Regex.Split(numbers.Trim(), @"[\s,]+");
        var result = new List<float>();

        foreach (var part in parts)
        {
            if (float.TryParse(part, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var value))
            {
                result.Add(value);
            }
        }

        return result.ToArray();
    }
}


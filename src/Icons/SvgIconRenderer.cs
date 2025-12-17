using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace ClassicPanel.Icons;

/// <summary>
/// Renders SVG path data to bitmaps with theme-aware colors.
/// Supports light and dark mode with automatic color adaptation.
/// Enhanced to support viewBox, stroke attributes, and multiple paths.
/// </summary>
public static class SvgIconRenderer
{
    private static readonly Dictionary<string, Bitmap> _iconCache = new();
    private static readonly object _cacheLock = new();

    /// <summary>
    /// Renders an SVG icon data to a bitmap with the specified size and color.
    /// </summary>
    /// <param name="iconData">The SVG icon data.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering.</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgIcon(SvgIconData iconData, int size, Color color)
    {
        if (iconData == null)
            throw new ArgumentNullException(nameof(iconData));

        if (size <= 0)
            throw new ArgumentException("Size must be greater than zero.", nameof(size));

        // Create cache key
        var cacheKey = $"{iconData.ViewBox}|{string.Join("|", iconData.Paths)}|{size}|{color.ToArgb()}|{iconData.StrokeWidth}|{iconData.Fill}";

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

            // Parse viewBox to get scaling
            var viewBox = ParseViewBox(iconData.ViewBox);
            var scaleX = size / viewBox.Width;
            var scaleY = size / viewBox.Height;
            var scale = Math.Min(scaleX, scaleY); // Maintain aspect ratio

            // Calculate offset to center the icon
            var offsetX = (size - viewBox.Width * scale) / 2;
            var offsetY = (size - viewBox.Height * scale) / 2;

            using (var pen = new Pen(color, iconData.StrokeWidth * scale))
            {
                pen.StartCap = iconData.StrokeLineCap;
                pen.EndCap = iconData.StrokeLineCap;
                pen.LineJoin = iconData.StrokeLineJoin;

                using (var brush = iconData.Fill ? new SolidBrush(color) : null)
                {
                    // Render all paths
                    foreach (var pathData in iconData.Paths)
                    {
                        var path = ParseSvgPath(pathData, viewBox, scale, offsetX, offsetY);
                        
                        if (iconData.Fill && brush != null)
                        {
                            graphics.FillPath(brush, path);
                        }
                        else
                        {
                            graphics.DrawPath(pen, path);
                        }
                    }
                }
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
    /// Renders an SVG path to a bitmap with the specified size and color (backward compatibility).
    /// </summary>
    /// <param name="svgPath">The SVG path data (e.g., "M 0 0 L 10 10 Z").</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="color">The color to use for rendering.</param>
    /// <param name="strokeWidth">The stroke width (default: 2.0).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgPath(string svgPath, int size, Color color, float strokeWidth = 2.0f)
    {
        var iconData = SvgIconData.FromPath(svgPath, strokeWidth);
        return RenderSvgIcon(iconData, size, color);
    }

    /// <summary>
    /// Renders an SVG icon with theme-aware colors (light or dark mode).
    /// </summary>
    /// <param name="iconData">The SVG icon data.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="isDarkMode">True for dark mode, false for light mode.</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgIconThemed(SvgIconData iconData, int size, bool isDarkMode)
    {
        // Use appropriate color for theme
        var color = isDarkMode 
            ? Color.White  // White for dark mode
            : Color.Black; // Black for light mode

        return RenderSvgIcon(iconData, size, color);
    }

    /// <summary>
    /// Renders an SVG path with theme-aware colors (light or dark mode) - backward compatibility.
    /// </summary>
    /// <param name="svgPath">The SVG path data.</param>
    /// <param name="size">The size of the icon in pixels.</param>
    /// <param name="isDarkMode">True for dark mode, false for light mode.</param>
    /// <param name="strokeWidth">The stroke width (default: 2.0).</param>
    /// <returns>A bitmap containing the rendered icon.</returns>
    public static Bitmap RenderSvgPathThemed(string svgPath, int size, bool isDarkMode, float strokeWidth = 2.0f)
    {
        var iconData = SvgIconData.FromPath(svgPath, strokeWidth);
        return RenderSvgIconThemed(iconData, size, isDarkMode);
    }

    /// <summary>
    /// Parses a viewBox string (e.g., "0 0 24 24").
    /// </summary>
    private static RectangleF ParseViewBox(string viewBox)
    {
        if (string.IsNullOrWhiteSpace(viewBox))
            return new RectangleF(0, 0, 24, 24); // Default

        var numbers = ParseNumbers(viewBox);
        if (numbers.Length >= 4)
        {
            return new RectangleF(numbers[0], numbers[1], numbers[2], numbers[3]);
        }

        return new RectangleF(0, 0, 24, 24); // Default
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
    /// Supports basic SVG path commands: M, L, H, V, Z, A (both absolute and relative).
    /// </summary>
    private static GraphicsPath ParseSvgPath(string svgPath, RectangleF viewBox, float scale, float offsetX, float offsetY)
    {
        var path = new GraphicsPath();
        var currentPoint = new PointF(0, 0);
        var startPoint = new PointF(0, 0);

        // Character-by-character parser for more accurate parsing
        // This handles cases like "M12 2v2" correctly
        int i = 0;
        while (i < svgPath.Length)
        {
            // Skip whitespace
            while (i < svgPath.Length && char.IsWhiteSpace(svgPath[i]))
                i++;
            
            if (i >= svgPath.Length)
                break;

            // Get command
            char commandChar = svgPath[i];
            const string validCommands = "MLHVZAmmlhvza";
            if (validCommands.IndexOf(commandChar) < 0)
            {
                i++;
                continue;
            }

            bool isRelative = char.IsLower(commandChar);
            string command = commandChar.ToString().ToUpper();
            i++; // Move past command

            // Extract arguments until next command or end of string
            int argsStart = i;
            while (i < svgPath.Length)
            {
                char c = svgPath[i];
                // Stop if we hit a command character (case-insensitive check)
                if (validCommands.IndexOf(c) >= 0)
                    break;
                i++;
            }
            
            string args = svgPath.Substring(argsStart, i - argsStart).Trim();

            switch (command)
            {
                case "M": // MoveTo
                    var moveCoords = ParseNumbers(args);
                    if (moveCoords.Length >= 2)
                    {
                        if (isRelative)
                        {
                            currentPoint = new PointF(
                                currentPoint.X + moveCoords[0] * scale,
                                currentPoint.Y + moveCoords[1] * scale
                            );
                        }
                        else
                        {
                            currentPoint = new PointF(
                                moveCoords[0] * scale + offsetX,
                                moveCoords[1] * scale + offsetY
                            );
                        }
                        startPoint = currentPoint;
                    }
                    break;

                case "L": // LineTo
                    var lineCoords = ParseNumbers(args);
                    for (int j = 0; j < lineCoords.Length - 1; j += 2)
                    {
                        PointF endPoint;
                        if (isRelative)
                        {
                            endPoint = new PointF(
                                currentPoint.X + lineCoords[j] * scale,
                                currentPoint.Y + lineCoords[j + 1] * scale
                            );
                        }
                        else
                        {
                            endPoint = new PointF(
                                lineCoords[j] * scale + offsetX,
                                lineCoords[j + 1] * scale + offsetY
                            );
                        }
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "H": // Horizontal LineTo
                    var hCoords = ParseNumbers(args);
                    foreach (var x in hCoords)
                    {
                        PointF endPoint;
                        if (isRelative)
                        {
                            endPoint = new PointF(currentPoint.X + x * scale, currentPoint.Y);
                        }
                        else
                        {
                            endPoint = new PointF(x * scale + offsetX, currentPoint.Y);
                        }
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "V": // Vertical LineTo
                    var vCoords = ParseNumbers(args);
                    foreach (var y in vCoords)
                    {
                        PointF endPoint;
                        if (isRelative)
                        {
                            endPoint = new PointF(currentPoint.X, currentPoint.Y + y * scale);
                        }
                        else
                        {
                            endPoint = new PointF(currentPoint.X, y * scale + offsetY);
                        }
                        path.AddLine(currentPoint, endPoint);
                        currentPoint = endPoint;
                    }
                    break;

                case "Z": // ClosePath
                    if (currentPoint != startPoint)
                    {
                        path.AddLine(currentPoint, startPoint);
                    }
                    currentPoint = startPoint;
                    path.CloseFigure();
                    break;

                case "A": // ArcTo (elliptical arc)
                    var arcCoords = ParseNumbers(args);
                    // Arc format: rx ry x-axis-rotation large-arc-flag sweep-flag x y
                    if (arcCoords.Length >= 7)
                    {
                        float endX, endY;
                        if (isRelative)
                        {
                            endX = currentPoint.X + arcCoords[5] * scale;
                            endY = currentPoint.Y + arcCoords[6] * scale;
                        }
                        else
                        {
                            endX = arcCoords[5] * scale + offsetX;
                            endY = arcCoords[6] * scale + offsetY;
                        }
                        // Approximate arc with a line (can be enhanced later with proper arc rendering)
                        path.AddLine(currentPoint, new PointF(endX, endY));
                        currentPoint = new PointF(endX, endY);
                    }
                    break;
            }
        }

        return path;
    }

    /// <summary>
    /// Parses a string of numbers (separated by spaces, commas, or minus signs) into an array of floats.
    /// Handles cases like "12 2" or "12,2" or "-5.5" or "6-3" (two numbers: 6 and -3).
    /// </summary>
    private static float[] ParseNumbers(string numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers))
            return Array.Empty<float>();

        var result = new List<float>();
        
        // More robust number parsing that handles:
        // - Numbers with optional minus signs
        // - Decimal points
        // - Scientific notation (e, E)
        // - Separated by spaces, commas, or minus signs (when minus is between numbers)
        // Pattern matches: optional minus, digits, optional decimal, optional exponent
        var numberPattern = @"(-?(?:\d+\.?\d*|\.\d+)(?:[eE][+-]?\d+)?)";
        var matches = Regex.Matches(numbers, numberPattern);

        foreach (Match match in matches)
        {
            if (float.TryParse(match.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var value))
            {
                result.Add(value);
            }
        }

        return result.ToArray();
    }
}


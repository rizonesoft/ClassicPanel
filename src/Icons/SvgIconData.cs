namespace ClassicPanel.Icons;

/// <summary>
/// Represents SVG icon data with rendering attributes.
/// </summary>
public class SvgIconData
{
    /// <summary>
    /// Gets or sets the viewBox (e.g., "0 0 24 24").
    /// </summary>
    public string ViewBox { get; set; } = "0 0 24 24";

    /// <summary>
    /// Gets or sets the list of path data strings.
    /// </summary>
    public List<string> Paths { get; set; } = new();

    /// <summary>
    /// Gets or sets the stroke width (default: 2).
    /// </summary>
    public float StrokeWidth { get; set; } = 2.0f;

    /// <summary>
    /// Gets or sets whether to fill the path (default: false).
    /// </summary>
    public bool Fill { get; set; } = false;

    /// <summary>
    /// Gets or sets the stroke line cap (default: Round).
    /// </summary>
    public System.Drawing.Drawing2D.LineCap StrokeLineCap { get; set; } = System.Drawing.Drawing2D.LineCap.Round;

    /// <summary>
    /// Gets or sets the stroke line join (default: Round).
    /// </summary>
    public System.Drawing.Drawing2D.LineJoin StrokeLineJoin { get; set; } = System.Drawing.Drawing2D.LineJoin.Round;

    /// <summary>
    /// Creates an SvgIconData from a simple path string (backward compatibility).
    /// Note: For best results, use SvgXmlParser.ParseSvgXml() with base64 data URIs.
    /// </summary>
    public static SvgIconData FromPath(string path, float strokeWidth = 2.0f)
    {
        // Split path by spaces to handle multiple path commands in one string
        // This handles cases like "M12 2v2" where we have multiple commands
        var paths = new List<string>();
        if (!string.IsNullOrWhiteSpace(path))
        {
            // For simple paths, treat the whole string as one path
            // The parser will handle splitting commands
            paths.Add(path);
        }

        return new SvgIconData
        {
            ViewBox = "0 0 24 24",
            Paths = paths,
            StrokeWidth = strokeWidth,
            Fill = false
        };
    }
}


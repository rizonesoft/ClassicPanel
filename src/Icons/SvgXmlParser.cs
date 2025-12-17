using System.Xml.Linq;

namespace ClassicPanel.Icons;

/// <summary>
/// Parses SVG XML to extract icon data including paths and attributes.
/// Supports both full SVG documents and data URIs.
/// </summary>
public static class SvgXmlParser
{
    /// <summary>
    /// Parses an SVG XML string and extracts icon data.
    /// </summary>
    /// <param name="svgXml">The SVG XML string.</param>
    /// <returns>An SvgIconData object with extracted paths and attributes.</returns>
    public static SvgIconData ParseSvgXml(string svgXml)
    {
        if (string.IsNullOrWhiteSpace(svgXml))
            throw new ArgumentException("SVG XML cannot be null or empty.", nameof(svgXml));

        // Remove data URI prefix if present
        if (svgXml.StartsWith("data:image/svg+xml;base64,", StringComparison.OrdinalIgnoreCase))
        {
            var base64Data = svgXml.Substring("data:image/svg+xml;base64,".Length);
            var decoded = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64Data));
            svgXml = decoded;
        }
        else if (svgXml.StartsWith("data:image/svg+xml,", StringComparison.OrdinalIgnoreCase))
        {
            // URL-encoded SVG
            var encoded = svgXml.Substring("data:image/svg+xml,".Length);
            svgXml = Uri.UnescapeDataString(encoded);
        }

        // Parse XML
        var doc = XDocument.Parse(svgXml);
        var svgElement = doc.Root;
        if (svgElement == null || svgElement.Name.LocalName != "svg")
            throw new ArgumentException("Invalid SVG XML: root element must be 'svg'.", nameof(svgXml));

        // Extract viewBox
        var viewBox = svgElement.Attribute("viewBox")?.Value ?? "0 0 24 24";
        
        // Extract stroke attributes
        var strokeWidthAttr = svgElement.Attribute("stroke-width");
        var strokeWidth = strokeWidthAttr != null && float.TryParse(strokeWidthAttr.Value, out var sw) ? sw : 2.0f;
        
        var fillAttr = svgElement.Attribute("fill");
        var fill = fillAttr?.Value != "none";
        
        var strokeLinecapAttr = svgElement.Attribute("stroke-linecap");
        var lineCap = ParseLineCap(strokeLinecapAttr?.Value ?? "round");
        
        var strokeLinejoinAttr = svgElement.Attribute("stroke-linejoin");
        var lineJoin = ParseLineJoin(strokeLinejoinAttr?.Value ?? "round");

        // Extract all path elements
        var paths = new List<string>();
        foreach (var pathElement in svgElement.Descendants(XName.Get("path", svgElement.Name.NamespaceName)))
        {
            var dAttr = pathElement.Attribute("d");
            if (dAttr != null && !string.IsNullOrWhiteSpace(dAttr.Value))
            {
                paths.Add(dAttr.Value);
            }
        }

        // Also check for circle, rect, etc. if needed (for now, just paths)
        foreach (var circleElement in svgElement.Descendants(XName.Get("circle", svgElement.Name.NamespaceName)))
        {
            // Convert circle to path (can be enhanced later)
            var cx = float.Parse(circleElement.Attribute("cx")?.Value ?? "12");
            var cy = float.Parse(circleElement.Attribute("cy")?.Value ?? "12");
            var r = float.Parse(circleElement.Attribute("r")?.Value ?? "4");
            paths.Add($"M {cx - r} {cy} A {r} {r} 0 1 0 {cx + r} {cy} A {r} {r} 0 1 0 {cx - r} {cy} Z");
        }

        return new SvgIconData
        {
            ViewBox = viewBox,
            Paths = paths,
            StrokeWidth = strokeWidth,
            Fill = fill,
            StrokeLineCap = lineCap,
            StrokeLineJoin = lineJoin
        };
    }

    /// <summary>
    /// Parses a line cap string to LineCap enum.
    /// </summary>
    private static System.Drawing.Drawing2D.LineCap ParseLineCap(string value)
    {
        return value?.ToLowerInvariant() switch
        {
            "round" => System.Drawing.Drawing2D.LineCap.Round,
            "square" => System.Drawing.Drawing2D.LineCap.Square,
            "butt" => System.Drawing.Drawing2D.LineCap.Flat,
            _ => System.Drawing.Drawing2D.LineCap.Round
        };
    }

    /// <summary>
    /// Parses a line join string to LineJoin enum.
    /// </summary>
    private static System.Drawing.Drawing2D.LineJoin ParseLineJoin(string value)
    {
        return value?.ToLowerInvariant() switch
        {
            "round" => System.Drawing.Drawing2D.LineJoin.Round,
            "miter" => System.Drawing.Drawing2D.LineJoin.Miter,
            "bevel" => System.Drawing.Drawing2D.LineJoin.Bevel,
            _ => System.Drawing.Drawing2D.LineJoin.Round
        };
    }
}


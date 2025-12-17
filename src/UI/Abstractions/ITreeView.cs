namespace ClassicPanel.UI.Abstractions;

/// <summary>
/// Represents an abstract tree view interface for displaying hierarchical data, such as categories.
/// </summary>
public interface ITreeView
{
    /// <summary>
    /// Gets all tree nodes in the tree view.
    /// </summary>
    IReadOnlyList<ITreeNode> Nodes { get; }

    /// <summary>
    /// Gets the currently selected node.
    /// </summary>
    ITreeNode? SelectedNode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree view shows lines connecting nodes.
    /// </summary>
    bool ShowLines { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree view shows root lines.
    /// </summary>
    bool ShowRootLines { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree view shows plus/minus buttons.
    /// </summary>
    bool ShowPlusMinus { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree view is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree view is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Adds a root node to the tree view.
    /// </summary>
    /// <param name="text">The text of the node.</param>
    /// <param name="tag">Optional data associated with the node.</param>
    /// <returns>The created tree node.</returns>
    ITreeNode AddNode(string text, object? tag = null);

    /// <summary>
    /// Removes a node from the tree view.
    /// </summary>
    /// <param name="node">The node to remove.</param>
    /// <returns>True if the node was removed; otherwise, false.</returns>
    bool RemoveNode(ITreeNode node);

    /// <summary>
    /// Removes all nodes from the tree view.
    /// </summary>
    void Clear();

    /// <summary>
    /// Expands all nodes in the tree view.
    /// </summary>
    void ExpandAll();

    /// <summary>
    /// Collapses all nodes in the tree view.
    /// </summary>
    void CollapseAll();

    /// <summary>
    /// Finds a node by its tag value.
    /// </summary>
    /// <param name="tag">The tag value to search for.</param>
    /// <returns>The found node, or null if not found.</returns>
    ITreeNode? FindNodeByTag(object tag);

    /// <summary>
    /// Occurs when a node is selected.
    /// </summary>
    event EventHandler<TreeNodeSelectedEventArgs>? NodeSelected;

    /// <summary>
    /// Occurs when a node is double-clicked.
    /// </summary>
    event EventHandler<TreeNodeSelectedEventArgs>? NodeDoubleClick;
}

/// <summary>
/// Represents an abstract tree node interface.
/// </summary>
public interface ITreeNode
{
    /// <summary>
    /// Gets or sets the text of the node.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the node.
    /// </summary>
    object? Tag { get; set; }

    /// <summary>
    /// Gets the parent node of this node, or null if this is a root node.
    /// </summary>
    ITreeNode? Parent { get; }

    /// <summary>
    /// Gets all child nodes of this node.
    /// </summary>
    IReadOnlyList<ITreeNode> Nodes { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is expanded.
    /// </summary>
    bool IsExpanded { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is selected.
    /// </summary>
    bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is visible.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Adds a child node to this node.
    /// </summary>
    /// <param name="text">The text of the child node.</param>
    /// <param name="tag">Optional data associated with the child node.</param>
    /// <returns>The created child node.</returns>
    ITreeNode AddChild(string text, object? tag = null);

    /// <summary>
    /// Removes a child node from this node.
    /// </summary>
    /// <param name="child">The child node to remove.</param>
    /// <returns>True if the child node was removed; otherwise, false.</returns>
    bool RemoveChild(ITreeNode child);

    /// <summary>
    /// Removes all child nodes from this node.
    /// </summary>
    void Clear();

    /// <summary>
    /// Expands this node and all its children.
    /// </summary>
    void ExpandAll();

    /// <summary>
    /// Collapses this node and all its children.
    /// </summary>
    void CollapseAll();
}

/// <summary>
/// Provides data for tree node selection events.
/// </summary>
public class TreeNodeSelectedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the selected node.
    /// </summary>
    public ITreeNode Node { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeNodeSelectedEventArgs"/> class.
    /// </summary>
    /// <param name="node">The selected node.</param>
    public TreeNodeSelectedEventArgs(ITreeNode node)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
    }
}


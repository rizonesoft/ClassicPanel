using ClassicPanel.UI.Abstractions;

namespace ClassicPanel.UI.WinForms;

/// <summary>
/// WinForms implementation of the ITreeView interface.
/// </summary>
public class WinFormsTreeView : ITreeView
{
    private readonly System.Windows.Forms.TreeView _treeView;
    private readonly Dictionary<object, WinFormsTreeNode> _nodeMap = new();
    private readonly List<WinFormsTreeNode> _rootNodes = new();

    /// <summary>
    /// Gets the underlying WinForms TreeView control.
    /// </summary>
    public System.Windows.Forms.TreeView TreeView => _treeView;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsTreeView"/> class.
    /// </summary>
    public WinFormsTreeView()
    {
        _treeView = new System.Windows.Forms.TreeView();
        _treeView.AfterSelect += (sender, e) =>
        {
            if (e.Node != null && e.Node.Tag is WinFormsTreeNode wrapper)
            {
                NodeSelected?.Invoke(this, new TreeNodeSelectedEventArgs(wrapper));
            }
        };
        _treeView.NodeMouseDoubleClick += (sender, e) =>
        {
            if (e.Node != null && e.Node.Tag is WinFormsTreeNode wrapper)
            {
                NodeDoubleClick?.Invoke(this, new TreeNodeSelectedEventArgs(wrapper));
            }
        };
    }

    /// <inheritdoc/>
    public IReadOnlyList<ITreeNode> Nodes => _rootNodes.Cast<ITreeNode>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public ITreeNode? SelectedNode
    {
        get
        {
            if (_treeView.SelectedNode != null && _treeView.SelectedNode.Tag is WinFormsTreeNode wrapper)
            {
                return wrapper;
            }
            return null;
        }
        set
        {
            if (value is WinFormsTreeNode winFormsNode)
            {
                _treeView.SelectedNode = winFormsNode.TreeNode;
            }
            else
            {
                _treeView.SelectedNode = null;
            }
        }
    }

    /// <inheritdoc/>
    public bool ShowLines
    {
        get => _treeView.ShowLines;
        set => _treeView.ShowLines = value;
    }

    /// <inheritdoc/>
    public bool ShowRootLines
    {
        get => _treeView.ShowRootLines;
        set => _treeView.ShowRootLines = value;
    }

    /// <inheritdoc/>
    public bool ShowPlusMinus
    {
        get => _treeView.ShowPlusMinus;
        set => _treeView.ShowPlusMinus = value;
    }

    /// <inheritdoc/>
    public bool Enabled
    {
        get => _treeView.Enabled;
        set => _treeView.Enabled = value;
    }

    /// <inheritdoc/>
    public bool Visible
    {
        get => _treeView.Visible;
        set => _treeView.Visible = value;
    }

    /// <inheritdoc/>
    public ITreeNode AddNode(string text, object? tag = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Node text cannot be null or empty.", nameof(text));

        var treeNode = new System.Windows.Forms.TreeNode(text)
        {
            Tag = tag
        };

        var wrapper = new WinFormsTreeNode(treeNode, null);
        treeNode.Tag = wrapper;
        _treeView.Nodes.Add(treeNode);
        _rootNodes.Add(wrapper);

        if (tag != null)
        {
            _nodeMap[tag] = wrapper;
        }

        return wrapper;
    }

    /// <inheritdoc/>
    public bool RemoveNode(ITreeNode node)
    {
        if (node is WinFormsTreeNode winFormsNode)
        {
            if (winFormsNode.Parent == null)
            {
                _treeView.Nodes.Remove(winFormsNode.TreeNode);
                _rootNodes.Remove(winFormsNode);
            }
            else
            {
                winFormsNode.TreeNode.Parent?.Nodes.Remove(winFormsNode.TreeNode);
            }
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _treeView.Nodes.Clear();
        _rootNodes.Clear();
        _nodeMap.Clear();
    }

    /// <inheritdoc/>
    public void ExpandAll()
    {
        _treeView.ExpandAll();
    }

    /// <inheritdoc/>
    public void CollapseAll()
    {
        _treeView.CollapseAll();
    }

    /// <inheritdoc/>
    public ITreeNode? FindNodeByTag(object tag)
    {
        if (tag == null)
            return null;

        _nodeMap.TryGetValue(tag, out var node);
        return node;
    }

    /// <inheritdoc/>
    public event EventHandler<TreeNodeSelectedEventArgs>? NodeSelected;

    /// <inheritdoc/>
    public event EventHandler<TreeNodeSelectedEventArgs>? NodeDoubleClick;
}

/// <summary>
/// WinForms implementation of the ITreeNode interface.
/// </summary>
internal class WinFormsTreeNode : ITreeNode
{
    private readonly System.Windows.Forms.TreeNode _treeNode;
    private readonly WinFormsTreeNode? _parent;
    private readonly List<WinFormsTreeNode> _childNodes = new();

    /// <summary>
    /// Gets the underlying WinForms TreeNode.
    /// </summary>
    public System.Windows.Forms.TreeNode TreeNode => _treeNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormsTreeNode"/> class.
    /// </summary>
    public WinFormsTreeNode(System.Windows.Forms.TreeNode treeNode, WinFormsTreeNode? parent)
    {
        _treeNode = treeNode ?? throw new ArgumentNullException(nameof(treeNode));
        _parent = parent;
    }

    /// <inheritdoc/>
    public string Text
    {
        get => _treeNode.Text;
        set => _treeNode.Text = value;
    }

    /// <inheritdoc/>
    public object? Tag
    {
        get => _treeNode.Tag;
        set => _treeNode.Tag = value;
    }

    /// <inheritdoc/>
    public ITreeNode? Parent => _parent;

    /// <inheritdoc/>
    public IReadOnlyList<ITreeNode> Nodes => _childNodes.Cast<ITreeNode>().ToList().AsReadOnly();

    /// <inheritdoc/>
    public bool IsExpanded
    {
        get => _treeNode.IsExpanded;
        set
        {
            if (value)
                _treeNode.Expand();
            else
                _treeNode.Collapse();
        }
    }

    /// <inheritdoc/>
    public bool IsSelected
    {
        get => _treeNode.IsSelected;
        set
        {
            if (value)
                _treeNode.TreeView?.SelectedNode = _treeNode;
            else if (_treeNode.TreeView?.SelectedNode == _treeNode)
            {
                _treeNode.TreeView.SelectedNode = null;
            }
        }
    }

    /// <inheritdoc/>
    public bool IsVisible
    {
        get => _treeNode.IsVisible;
        set
        {
            // WinForms doesn't have a direct IsVisible property, but we can use Visible property
            // Note: This is a simplified implementation
        }
    }

    /// <inheritdoc/>
    public ITreeNode AddChild(string text, object? tag = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Node text cannot be null or empty.", nameof(text));

        var childNode = new System.Windows.Forms.TreeNode(text)
        {
            Tag = tag
        };

        var wrapper = new WinFormsTreeNode(childNode, this);
        childNode.Tag = wrapper;
        _treeNode.Nodes.Add(childNode);
        _childNodes.Add(wrapper);

        return wrapper;
    }

    /// <inheritdoc/>
    public bool RemoveChild(ITreeNode child)
    {
        if (child is WinFormsTreeNode winFormsChild)
        {
            _treeNode.Nodes.Remove(winFormsChild.TreeNode);
            _childNodes.Remove(winFormsChild);
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _treeNode.Nodes.Clear();
        _childNodes.Clear();
    }

    /// <inheritdoc/>
    public void ExpandAll()
    {
        _treeNode.ExpandAll();
    }

    /// <inheritdoc/>
    public void CollapseAll()
    {
        _treeNode.Collapse();
        foreach (var child in _childNodes)
        {
            child.CollapseAll();
        }
    }
}


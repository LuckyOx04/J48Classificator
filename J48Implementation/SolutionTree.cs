namespace J48Implementation;

public class SolutionTree
{
    public class Node
    {
        public string FieldName { get; set; }
        public List<int> ValuesPositions { get; set; }
        public Dictionary<string, Node> Children { get; set; }
        private bool _hasParent = false;
        public bool HasParent => _hasParent;
        
        public bool IsLeaf => Children.Count == 0;

        public Node(string fieldName)
        {
            this.Children = new Dictionary<string, Node>();
            this.ValuesPositions = new List<int>();
            this.FieldName = fieldName;
        }

        public Node(string fieldName, List<int> valuesPositions)
        {
            this.Children = new Dictionary<string, Node>();
            this.ValuesPositions = new List<int>(valuesPositions);
            this.FieldName = fieldName;
        }

        public void AddChild(string branchName, Node child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child), "Child cannot be null");
            }

            if (child.HasParent)
            {
                throw new ArgumentException("This node already has a parent");
            }
            if (!this.Children.ContainsKey(branchName))
            {
                this.Children.Add(branchName, child);
                this._hasParent = true;
            }
            else
            {
                throw new ArgumentException("branchName already exists");
            }
        }

        public Node GetChild(string branchName)
        {
            if (this.Children.ContainsKey(branchName))
            {
                return this.Children[branchName];
            }
            else
            {
                throw new ArgumentException("branchName does not exist");
            }
        }
    }
    
    private Node _root;
    public Node Root => _root;
    
    public SolutionTree(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
        {
            throw new ArgumentNullException(nameof(fieldName), "Field name cannot be null or empty");
        }
        
        this._root = new Node(fieldName);
    }
}
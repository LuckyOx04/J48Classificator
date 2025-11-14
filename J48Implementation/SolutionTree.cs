namespace J48Implementation;

public class SolutionTree
{
    public class Node
    {
        public string FieldName { get; set; }
        public string? ClassValueAnswer { get; set; }
        public Dictionary<string, Node> Children { get; set; }
        private bool _hasParent;
        
        public bool IsLeaf => Children.Count == 0;

        public Node(string fieldName)
        {
            this.Children = new Dictionary<string, Node>();
            this.ClassValueAnswer = string.Empty;
            this.FieldName = fieldName;
            this._hasParent = false;
        }

        public Node(string fieldName, string classValueAnswer)
        {
            this.Children = new Dictionary<string, Node>();
            this.ClassValueAnswer = classValueAnswer;
            this.FieldName = fieldName;
        }

        public void AddChild(string branchName, Node child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child), "Child cannot be null");
            }

            if (child._hasParent)
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

    public void PrintTreeDfs(Node? node, string padding)
    {
        if (node == null)
        {
            return;
        }

        if (node.IsLeaf)
        {
            Console.WriteLine($"{padding}{node.ClassValueAnswer}");
        }
        else
        {
            Console.WriteLine($"{padding}{node.FieldName}");
            Console.Write($"{padding}Branches: ");
            foreach (var branch in node.Children.Keys)
            {
                Console.Write($"{branch}, ");
            }
            Console.WriteLine();
        }

        foreach (var branch in node.Children.Keys)
        {
            Console.WriteLine($"{padding}{branch}:");
            PrintTreeDfs(node.Children[branch], $"{padding}    ");
        }
    }
}
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
            if (this.Children.TryAdd(branchName, child))
            {
                this._hasParent = true;
            }
            else
            {
                throw new ArgumentException("branchName already exists");
            }
        }

        public Node GetChild(string branchName)
        {
            Node? result = null;
            if (this.Children.TryGetValue(branchName, out result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("branchName does not exist");
            }
        }
    }
    
    private readonly Node _root;
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
            Console.WriteLine(node.ClassValueAnswer);
        }
        else
        {
            Console.WriteLine($"{node.FieldName.ToUpper()}╗");
            padding = padding.PadRight(node.FieldName.Length + padding.Length);
            Console.WriteLine(padding + "║");
        }

        int keysCount = node.Children.Keys.Count;
        int iterations = 1;
        foreach (var branch in node.Children.Keys)
        {
            bool isLastBranch = keysCount == iterations++;
            string beginningSymbol = isLastBranch ? "╚" : "╟";
            string outputString = $"{beginningSymbol}══{branch}══ᐳ ";
            Console.Write($"{padding}{outputString}");
            string column = isLastBranch ? " " : "║";
            string newPadding = $"{padding}{column}";
            newPadding = newPadding.PadRight(outputString.Length + newPadding.Length - 1);
            PrintTreeDfs(node.Children[branch], newPadding);
        }
    }
}
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
            Children = new Dictionary<string, Node>();
            ClassValueAnswer = string.Empty;
            FieldName = fieldName;
            _hasParent = false;
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
            if (Children.TryAdd(branchName, child))
            {
                _hasParent = true;
            }
            else
            {
                throw new ArgumentException("branchName already exists");
            }
        }

        public Node GetChild(string branchName)
        {
            Node? result;
            if (Children.TryGetValue(branchName, out result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Branch name {branchName} does not exist");
            }
        }
        
        public bool HasBranch(string branchName)
        {
            return Children.ContainsKey(branchName);
        }
    }
    
    public Node Root { get; private set; }
    
    public SolutionTree(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
        {
            throw new ArgumentNullException(nameof(fieldName), "Field name cannot be null or empty");
        }
        
        Root = new Node(fieldName);
    }

    public override string ToString()
    {
        string result = "";
        void PrintTreeDfs(Node? node, string padding)
        {

            if (node == null)
            {
                return;
            }

            if (node.IsLeaf)
            {
                result += $"{node.ClassValueAnswer}\n";
                result += $"{padding}\n";
            }
            else
            {
                result += $"{node.FieldName.ToUpper()} ╗\n";
                padding = padding.PadRight(node.FieldName.Length + padding.Length + 1);
                result += $"{padding}║\n";
            }

            int keysCount = node.Children.Keys.Count;
            int iterations = 1;
            foreach (var branch in node.Children.Keys)
            {
                bool isLastBranch = keysCount == iterations++;
                string beginningSymbol = isLastBranch ? "╚" : "╟";
                string outputString = $"{beginningSymbol}══ {branch} ══ᐳ ";
                result += $"{padding}{outputString}";
                string column = isLastBranch ? " " : "║";
                string newPadding = $"{padding}{column}";
                newPadding = newPadding.PadRight(outputString.Length + newPadding.Length - 1);

                PrintTreeDfs(node.Children[branch], newPadding);
            }
        }
        
        PrintTreeDfs(Root, "");
        return result;
    }
}
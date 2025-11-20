namespace J48Implementation;

public class InstancesClassifier
{
    public List<Dictionary<string, string>> TestingData { get; set; }
    private string _classField;
    public string ClassField => _classField;
    private SolutionTree _solutionTree;

    public InstancesClassifier(List<Dictionary<string, string>> testingData, string classField, SolutionTree solutionTree)
    {
        this.TestingData = testingData;
        this._classField = classField;
        this._solutionTree = solutionTree;
    }
    
    public string? ClassifyInstance(Dictionary<string, string> instance)
    {
        SolutionTree.Node currentNode = this._solutionTree.Root;
        while (!currentNode.IsLeaf)
        {
            currentNode = currentNode.GetChild(instance[currentNode.FieldName]);
        }

        return currentNode.ClassValueAnswer;
    }
}
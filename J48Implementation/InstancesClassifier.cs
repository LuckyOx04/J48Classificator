namespace J48Implementation;

public class InstancesClassifier
{
    public List<List<string>> Data { get; set; }
    private string _classField;
    public string ClassField => _classField;
    private SolutionTree _solutionTree;

    public InstancesClassifier(List<List<string>> data, string classField, SolutionTree solutionTree)
    {
        this.Data = data;
        this._classField = classField;
        this._solutionTree = solutionTree;
    }
    
    private void ClassifyInstance(Dictionary<string, string> instance)
    {
        
    }
}
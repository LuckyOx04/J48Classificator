namespace J48Implementation;

public class SolutionTreeBuilder
{
    private FieldEntropyAlgorithm _fieldEntropyAlgorithm;
    private Dictionary<string, List<string>> _data;
    private string _classField;

    public SolutionTreeBuilder(Dictionary<string, List<string>> data, string classField)
    {
        this._data = data;
        this._classField = classField;
        this._fieldEntropyAlgorithm = new FieldEntropyAlgorithm(data, classField);
    }
    
    private (double maxEntropy, string maxEntropyField) GetMaxFieldEntropy()
    {
        double maxEntropy = Double.MinValue;
        string maxEntropyField = "";
        double currentEntropy = 0;
        foreach (var key in _data.Keys.Where(key => key != _classField))
        {
            currentEntropy = _fieldEntropyAlgorithm.GetInformationGainForField(key);
            if (currentEntropy > maxEntropy)
            {
                maxEntropy = currentEntropy;
                maxEntropyField = key;
            }
        }
        
        return (maxEntropy, maxEntropyField);
    }
    
    private Dictionary<string, List<int>> GetValuesPositions(List<string> values, List<int> positions)
    {
        Dictionary<string, List<int>> valuesPositions = new Dictionary<string, List<int>>();
        List<string> subListOfValues = positions.Select(i => values[i]).ToList();
        for (int i = 0; i < subListOfValues.Count; i++)
        {
            if (!valuesPositions.ContainsKey(values[i]))
            {
                valuesPositions.Add(subListOfValues[i], new List<int>());
                valuesPositions[values[i]].Add(positions[i]);
            }
            else
            {
                valuesPositions[values[i]].Add(positions[i]);
            }
        }
        
        return valuesPositions;
    }
    
    private SolutionTree GetInitialTree()
    {
        (double maxEntropy, string maxEntropyField) = GetMaxFieldEntropy();
        SolutionTree solutionTree = new SolutionTree(maxEntropyField);
        int[] indexes = Enumerable.Range(0, _data[maxEntropyField].Count).ToArray();
        solutionTree.Root.ValuesPositions = new List<int>(indexes);
        return solutionTree;
    }

    public SolutionTree Build()
    {
        SolutionTree initialTree = GetInitialTree();
    }
}
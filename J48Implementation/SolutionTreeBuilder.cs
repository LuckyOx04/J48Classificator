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
    
    private string GetMaxFieldEntropy(Dictionary<string, List<string>> data)
    {
        double maxEntropy = Double.MinValue;
        string maxEntropyField = "";
        double currentEntropy = 0;
        foreach (var key in data.Keys.Where(key => key != _classField))
        {
            currentEntropy = _fieldEntropyAlgorithm.GetInformationGainForField(key);
            if (currentEntropy > maxEntropy)
            {
                maxEntropy = currentEntropy;
                maxEntropyField = key;
            }
        }
        
        return maxEntropyField;
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

    private Dictionary<string, List<string>> RemoveDataField(Dictionary<string, List<string>> data,
        string fieldToRemove)
    {
        if (fieldToRemove == string.Empty)
        {
            return data;
        }
        Dictionary<string, List<string>> newData = new Dictionary<string, List<string>>(data);
        if (!newData.Remove(fieldToRemove))
        {
            Console.Error.WriteLine($"Could not remove field {fieldToRemove}.");
        }
        
        return newData;
    }

    private Dictionary<string, List<string>> RemoveDataRows(Dictionary<string, List<string>> data,
        List<int> rowPositionsToKeep)
    {
        Dictionary<string, List<string>> newData = new Dictionary<string, List<string>>(data);
        foreach (var key in newData.Keys)
        {
            int listCount = newData[key].Count;
            for (int i = 0; i < listCount; i++)
            {
                if (!rowPositionsToKeep.Contains(i))
                {
                    newData[key].RemoveAt(i);
                }
            }
        }
        
        return newData;
    }

    private void BuildTree(SolutionTree.Node node, Dictionary<string, List<string>> data)
    {
        if (node.ValuesPositions.Select(i => data[_classField][i]).Distinct().Count() > 1)
        {
            return;
        }
        
        Dictionary<string, List<string>> newData = RemoveDataField(data, node.FieldName);
        newData = RemoveDataRows(newData, node.ValuesPositions);
        string maxEntropyField = GetMaxFieldEntropy(newData);
        Dictionary<string, List<int>> newValuesPositions = GetValuesPositions(newData[maxEntropyField],
            node.ValuesPositions);
        foreach (var key in newValuesPositions.Keys)
        {
            SolutionTree.Node newNode = new SolutionTree.Node(maxEntropyField, newValuesPositions[key]);
            node.AddChild(key, newNode);
            BuildTree(newNode, newData);
        }
    }
    
    private SolutionTree GetInitialTree()
    {
        string maxEntropyField = GetMaxFieldEntropy(_data);
        SolutionTree solutionTree = new SolutionTree(maxEntropyField);
        int[] indexes = Enumerable.Range(0, _data[maxEntropyField].Count).ToArray();
        solutionTree.Root.ValuesPositions = new List<int>(indexes);
        return solutionTree;
    }

    public SolutionTree Build()
    {
        SolutionTree initialTree = GetInitialTree();
        BuildTree(initialTree.Root, _data);
        return initialTree;
    }
}
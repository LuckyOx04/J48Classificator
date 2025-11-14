namespace J48Implementation;

public class SolutionTreeBuilder
{
    private readonly FieldEntropyAlgorithm _fieldEntropyAlgorithm;
    private readonly Dictionary<string, List<string>> _data;
    private readonly string _classField;

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
        double currentEntropy;
        _fieldEntropyAlgorithm.Data = data;
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
    
    private Dictionary<string, List<int>> GetValuesPositions(List<string> values)
    {
        Dictionary<string, List<int>> valuesPositions = new Dictionary<string, List<int>>();
        int currentPosition = 0;
        foreach (var value in values)
        {
            if (!valuesPositions.ContainsKey(value))
            {
                valuesPositions.Add(value, new List<int>());
                valuesPositions[value].Add(currentPosition++);
            }
            else
            {
                valuesPositions[value].Add(currentPosition++);
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

    private Dictionary<string, List<string>> KeepDataRows(Dictionary<string, List<string>> data,
        List<int> rowPositionsToKeep)
    {
        Dictionary<string, List<string>> newData = new Dictionary<string, List<string>>();
        
        foreach (var key in data.Keys)
        {
            foreach (var position in rowPositionsToKeep)
            {
                if (!newData.ContainsKey(key))
                {
                    newData.Add(key, new List<string>());
                    newData[key].Add(data[key][position]);
                }
                else
                {
                    newData[key].Add(data[key][position]);
                }
            }
        }
        
        return newData;
    }

    private void BuildTree(SolutionTree.Node node, Dictionary<string, List<string>> data)
    {
        string maxEntropyField = "";
        Dictionary<string, List<string>> newData;
        Dictionary<string, List<int>> newValuesPositions = GetValuesPositions(data[node.FieldName]);
        foreach (var key in newValuesPositions.Keys)
        {
            newData = RemoveDataField(data, node.FieldName);
            newData = KeepDataRows(newData, newValuesPositions[key]);

            if (newData[_classField].Distinct().Count() <= 1)
            {
                SolutionTree.Node answerNode = new SolutionTree.Node("");
                answerNode.ClassValueAnswer = newData[_classField].FirstOrDefault();
                node.AddChild(key, answerNode);
                continue;
            }
            
            maxEntropyField = GetMaxFieldEntropy(newData);
            
            SolutionTree.Node newNode = new SolutionTree.Node(maxEntropyField);
            node.AddChild(key, newNode);
            BuildTree(newNode, newData);
        }
    }
    
    private SolutionTree GetInitialTree()
    {
        string maxEntropyField = GetMaxFieldEntropy(_data);
        SolutionTree solutionTree = new SolutionTree(maxEntropyField);
        return solutionTree;
    }

    public SolutionTree Build()
    {
        SolutionTree initialTree = GetInitialTree();
        BuildTree(initialTree.Root, _data);
        return initialTree;
    }
}
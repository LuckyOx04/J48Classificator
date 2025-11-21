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
    
    private string? ClassifyInstance(Dictionary<string, string> instance)
    {
        SolutionTree.Node currentNode = this._solutionTree.Root;
        while (!currentNode.IsLeaf)
        {
            currentNode = currentNode.GetChild(instance[currentNode.FieldName]);
        }

        return currentNode.ClassValueAnswer;
    }

    private List<string> GetPureClassResults()
    {
        List<string> result = new List<string>();
        foreach (var instance in TestingData)
        {
            result.Add(instance[_classField]);
        }
        
        return result;
    }

    private List<string> GetClassifiedClassResults()
    {
        List<string> result = new List<string>();
        foreach (var instance in TestingData)
        {
            string? classificationResult = ClassifyInstance(instance);
            if (classificationResult != null)
            {
                result.Add(classificationResult);
            }
        }

        return result;
    }

    private List<string> GetDistinctValues(List<string> values)
    {
        List<string> result = new List<string>();
        foreach (var value in values)
        {
            if (!result.Contains(value))
            {
                result.Add(value);
            }
        }
        
        return result;
    }

    private Dictionary<string, Dictionary<string, int>> GetInitialConfusionMatrix(List<string> classificationResults,
        List<string> pureResults)
    {
        Dictionary<string, Dictionary<string, int>> confusionMatrix = new Dictionary<string, Dictionary<string, int>>();
        List<string> pureDistinctValues = GetDistinctValues(pureResults);
        List<string> classificationDistinctValues = GetDistinctValues(classificationResults);
        List<string> distinctValues = classificationDistinctValues.Union(pureDistinctValues).ToList();
        foreach (var row in distinctValues)
        {
            confusionMatrix.Add(row, new Dictionary<string, int>());
            foreach (var column in distinctValues)
            {
                confusionMatrix[row][column] = 0;
            }
        }
        
        return confusionMatrix;
    }
    
    
    private Dictionary<string, Dictionary<string, int>> GetConfusionMatrix()
    {
        List<string> pureClassResults = GetPureClassResults();
        List<string> classifiedClassResults = GetClassifiedClassResults();
        Dictionary<string, Dictionary<string, int>> confusionMatrix = GetInitialConfusionMatrix(pureClassResults,
            classifiedClassResults);
        
        for (int i = 0; i < pureClassResults.Count; i++)
        {
            confusionMatrix[pureClassResults[i]][classifiedClassResults[i]]++;
        }
        
        return confusionMatrix;
    }

    public void PrintConfusionMatrix()
    {
        Dictionary<string, Dictionary<string, int>> confusionMatrix = GetConfusionMatrix();
        foreach (var row in confusionMatrix.Keys)
        {
            foreach (var column in confusionMatrix.Keys)
            {
                Console.Write($"[{confusionMatrix[row][column]}]");
            }
            Console.WriteLine();
        }
    }
}
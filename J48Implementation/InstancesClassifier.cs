namespace J48Implementation;

public class InstancesClassifier
{
    public List<Dictionary<string, string>> TestingData { get; set; }
    private string _classField;
    public string ClassField => _classField;
    private SolutionTree _solutionTree;

    public InstancesClassifier(List<Dictionary<string, string>> testingData, string classField, SolutionTree solutionTree)
    {
        TestingData = testingData;
        _classField = classField;
        _solutionTree = solutionTree;
    }
    
    private string? ClassifyInstance(Dictionary<string, string> instance)
    {
        SolutionTree.Node currentNode = _solutionTree.Root;
        while (!currentNode.IsLeaf)
        {
            string branchName = instance[currentNode.FieldName];
            if (currentNode.HasBranch(branchName))
            {
                currentNode = currentNode.GetChild(branchName);
            }
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

    private double GetAccuracy(Dictionary<string, Dictionary<string, int>> confusionMatrix)
    {
        int trueValues = 0;
        int totalValues = 0;
        foreach (var row in confusionMatrix.Keys)
        {
            foreach (var column in confusionMatrix[row].Keys)
            {
                if (row == column)
                {
                    trueValues += confusionMatrix[row][column];
                }
                totalValues += confusionMatrix[row][column];
            }
        }
        return trueValues / (double)totalValues;
    }

    private Dictionary<string, double> GetRecall(Dictionary<string, Dictionary<string, int>> confusionMatrix)
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        foreach (var row in confusionMatrix.Keys)
        {
            int rowSum = 0;
            foreach (var column in confusionMatrix.Keys)
            {
                rowSum += confusionMatrix[row][column];
            }
            double recall = confusionMatrix[row][row] / (double)rowSum;
            result.Add(row, recall);
        }
        
        return result;
    }

    private Dictionary<string, double> GetPrecision(Dictionary<string, Dictionary<string, int>> confusionMatrix)
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        foreach (var column in confusionMatrix.Keys)
        {
            int columnSum = 0;
            foreach (var row in confusionMatrix.Keys)
            {
                columnSum += confusionMatrix[row][column];
            }
            double precision = confusionMatrix[column][column] / (double)columnSum;
            result.Add(column, precision);
        }
        
        return result;
    }

    public string GetConfusionMatrixAsString()
    {
        string result = "";
        Dictionary<string, Dictionary<string, int>> confusionMatrix = GetConfusionMatrix();
        Dictionary<string, double> precisions = GetPrecision(confusionMatrix);
        Dictionary<string, double> recalls = GetRecall(confusionMatrix);
        char alphaCounter = 'a';
        bool isFirstColumn = true;
        foreach (var column in confusionMatrix.Keys)
        {
            string valueToPrint = $"{alphaCounter++}".PadRight(5);
            if (isFirstColumn)
            {
                valueToPrint = valueToPrint.PadLeft(6);
            }
            result += valueToPrint;
            isFirstColumn = false;
        }
        result += "<- Classified as\n";
        alphaCounter = 'a';
        foreach (var row in confusionMatrix.Keys)
        {
            isFirstColumn = true;
            foreach (var column in confusionMatrix.Keys)
            {
                string valueToPrint = $"{confusionMatrix[row][column]}".PadRight(5);
                if (isFirstColumn)
                {
                    valueToPrint = valueToPrint.PadLeft(6);
                }
                result += valueToPrint;
                isFirstColumn = false;
            }
            result += $"| {alphaCounter++} = {row}\n";
        }

        result += $"\n\nAccuracy: {Math.Round(GetAccuracy(confusionMatrix), 3)}\n";
        result += "\nRecalls:\n";
        foreach (var key in recalls.Keys)
        {
            double recall = double.IsNaN(recalls[key]) ? 0 : recalls[key];
            result += $"For {key}: {Math.Round(recall, 3)}\n";
        }

        result += "\nPrecisions:\n";
        foreach (var key in precisions.Keys)
        {
            double precision = double.IsNaN(precisions[key]) ? 0 : precisions[key];
            result += $"For {key}: {Math.Round(precision, 3)}\n";
        }

        result += "\n";
        
        return result;
    }
}
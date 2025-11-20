namespace J48Implementation;

public class FieldEntropyAlgorithm
{
    private readonly string _classField;
    private Dictionary<string, List<string>> _trainingData;

    public Dictionary<string, List<string>> TrainingData
    {
        get => _trainingData;
        set => _trainingData = value;
    }

    public FieldEntropyAlgorithm(Dictionary<string, List<string>> trainingData, string classField)
    {
        this._classField =  classField;
        this._trainingData = trainingData;
    }
    
    private double Entropy(params double[] values)
    {
        double sum = 0;
        foreach (var fraction in values)
        {
            sum -= fraction * Math.Log2(fraction);
        }

        return sum;
    }

    private Dictionary<string, int> GetNumberOfValuesInList(List<string> values)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        foreach (var value in values)
        {
                result.TryAdd(value, 1);
                result[value]++;
        }
        
        return result;
    }

    private double[] GetEntropyInput(List<string> values)
    {
        int totalValues = values.Count;
        Dictionary<string, int> valuesCount = GetNumberOfValuesInList(values);
        List<double> entropyInput = new List<double>();
        foreach (var key in valuesCount.Keys)
        {
            entropyInput.Add(valuesCount[key]/(double)totalValues);
        }
        return entropyInput.ToArray();
    }

    private Dictionary<string, List<string>> GetClassValuesForRegularValues(List<string> values)
    {
        Dictionary<string, List<string>> classValuesForRegularValues = new Dictionary<string, List<string>>();
        int currentIndex = 0;
        List<string> classValues = _trainingData[_classField];
        foreach (var value in values)
        {
            if (!classValuesForRegularValues.ContainsKey(value))
            {
                classValuesForRegularValues.Add(value, new List<string>());
                classValuesForRegularValues[value].Add(classValues[currentIndex++]);
            }
            else
            {
                classValuesForRegularValues[value].Add(classValues[currentIndex++]);
            }
        }
        
        return classValuesForRegularValues;
    }

    private double GetEntropyForListOfValues(List<string> values)
    {
        double[] entropyInput = GetEntropyInput(values);
        return Entropy(entropyInput.ToArray());
    }

    public double GetInformationGainForField(string field)
    {
        double sum = 0;
        List<string> fieldValues = _trainingData[field];
        List<string> classValues = _trainingData[_classField];
        Dictionary<string, List<string>> groupedFieldValues = GetClassValuesForRegularValues(fieldValues);
        foreach (var key in groupedFieldValues.Keys)
        {
            double weight = groupedFieldValues[key].Count/(double)classValues.Count;
            double fieldValuesEntropy = GetEntropyForListOfValues(groupedFieldValues[key]);
            sum += weight * fieldValuesEntropy;
        }
        
        return GetEntropyForListOfValues(classValues) - sum;
    }
}
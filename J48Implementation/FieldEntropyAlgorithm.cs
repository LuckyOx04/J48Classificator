namespace J48Implementation;

public class FieldEntropyAlgorithm
{
    private string _classField;
    private Dictionary<string, List<string>> _data;

    public Dictionary<string, List<string>> Data
    {
        get => _data;
        set => _data = value;
    }

    public FieldEntropyAlgorithm(Dictionary<string, List<string>> data, string classField)
    {
        this._classField =  classField;
        this._data = data;
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
            if (!result.ContainsKey(value))
            {
                result.Add(value, 1);
            }
            else
            {
                result[value]++;
            }
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
        List<string> classValues = _data[_classField];
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
        List<string> fieldValues = _data[field];
        List<string> classValues = _data[_classField];
        Dictionary<string, List<string>> groupedFieldValues = GetClassValuesForRegularValues(fieldValues);
        double weight = 0;
        double fieldValuesEntropy = 0;
        foreach (var key in groupedFieldValues.Keys)
        {
            weight = groupedFieldValues[key].Count/(double)classValues.Count;
            fieldValuesEntropy = GetEntropyForListOfValues(groupedFieldValues[key]);
            sum += weight * fieldValuesEntropy;
        }
        
        return GetEntropyForListOfValues(classValues) - sum;
    }
}
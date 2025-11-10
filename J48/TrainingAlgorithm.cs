namespace J48;

public class TrainingAlgorithm
{
    private static double Entropy(params double[] values)
    {
        double sum = 0;
        foreach (var fraction in values)
        {
            sum -= fraction * Math.Log2(fraction);
        }

        return sum;
    }

    private static Dictionary<string, int> GetNumberOfValues(Dictionary<string, List<string>> data, string field)
    {
        Dictionary<string, int> numberOfValuesInField = new Dictionary<string, int>();
        string[] fieldValues = data[field].ToArray();
        foreach (var value in fieldValues)
        {
            if (!numberOfValuesInField.ContainsKey(value))
            {
                numberOfValuesInField.Add(value, 1);
            }
            else
            {
                numberOfValuesInField[value]++;
            }
        }
        
        return numberOfValuesInField;
    }

    public static double GetClassFieldEntropy(Dictionary<string, List<string>> data, string field)
    {
        int totalValuesInClassField = data[field].Count;
        List<double> entropyInput = new List<double>();
        Dictionary<string, int> valuesCount = GetNumberOfValues(data, field);
        int valueCount = 0;
        foreach (var key in valuesCount.Keys)
        {
            valueCount = valuesCount[key];
            entropyInput.Add(valueCount/(double)totalValuesInClassField);
        }

        return Entropy(entropyInput.ToArray());
    }
}
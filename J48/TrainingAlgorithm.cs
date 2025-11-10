namespace J48;

public class TrainingAlgorithm
{
    private double Entropy(params double[] values)
    {
        double sum = 0;
        foreach (var fraction in values)
        {
            sum -= fraction * Math.Log2(fraction);
        }

        return sum;
    }

    private Dictionary<string, int> GetNumberOfValues(Dictionary<string, List<string>> data, string field)
    {
        Dictionary<string, int> numberOfValuesInField = new Dictionary<string, int>();
        string[] fieldValues = data[field].ToArray();
        foreach (var value in fieldValues)
        {
            numberOfValuesInField[field]++;
        }
        
        return numberOfValuesInField;
    }
}
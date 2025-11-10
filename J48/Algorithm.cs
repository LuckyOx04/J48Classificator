namespace J48;

public class Algorithm
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

    private Dictionary<string, int> GetNumberOfValues(string[] values)
    {
        throw new NotImplementedException();
    }
}
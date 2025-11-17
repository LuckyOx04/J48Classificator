namespace J48Implementation;

public static class DataHelper
{
    public static Dictionary<string, List<string>> GetTrainingData(string fileName)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        List<List<string>> data = File.ReadAllLines(fileName).Select(line => line.Split(",")
            .Select(value => value.ToLower()).ToList()).ToList();

        foreach (var header in data[0])
        {
            result[header] = new List<string>();
            for (int j = 1; j < data.Count; j++)
            {
                int index = data[0].IndexOf(header);
                result[header].Add(data[j][index]);
            }
        }

        return result;
    }

    public static List<List<string>> GetTestingData(string fileName)
    {
        List<List<string>> result = File.ReadAllLines(fileName).Select(line => line.Split(",")
            .Select(value => value.ToLower()).ToList()).ToList();
        return result;
    }
}
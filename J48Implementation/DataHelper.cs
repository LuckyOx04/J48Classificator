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

    public static List<Dictionary<string, string>> GetTestingData(string fileName)
    {
        
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        List<string> lines = File.ReadAllLines(fileName).ToList();
        List<string> headers = lines[0].Split(",").Select(value => value.ToLower()).ToList();
        
        for (int i = 1; i < lines.Count; i++)
        {
            Dictionary<string, string> pair = new Dictionary<string, string>();
            string[] line = lines[i].Split(",").Select(value => value.ToLower()).ToArray();
            for (int j = 0; j < headers.Count; j++)
            {
                pair.Add(headers[j], line[j]);
            }
            result.Add(pair);
        }
        
        return result;
    }
}
namespace J48Implementation;

public static class DataHelper
{
    public static async Task<Dictionary<string, List<string>>> GetTrainingDataAsync(string fileName)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        string[] lines = await File.ReadAllLinesAsync(fileName);
         
        string[][] data = lines.Select(line => line.Split(",")
            .Select(value => value.ToLower()).ToArray()).ToArray();
        
        foreach (var header in data[0])
        {
            result[header] = new List<string>();
            for (int j = 1; j < data.Length; j++)
            {
                int index = data[0].IndexOf(header);
                result[header].Add(data[j][index]);
            }
        }

        return result;
    }

    public static async Task<List<Dictionary<string, string>>> GetTestingDataAsync(string fileName)
    {
        
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        string[] lines = await File.ReadAllLinesAsync(fileName);
        string[] headers = lines[0].Split(",").Select(value => value.ToLower()).ToArray();
        
        for (int i = 1; i < lines.Length; i++)
        {
            Dictionary<string, string> pair = new Dictionary<string, string>();
            string[] line = lines[i].Split(",").Select(value => value.ToLower()).ToArray();
            for (int j = 0; j < headers.Length; j++)
            {
                pair.Add(headers[j], line[j]);
            }
            result.Add(pair);
        }
        
        return result;
    }
}
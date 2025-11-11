namespace J48Implementation;

public class DataHelper
{
    public static Dictionary<string, List<string>> CsvToDictionatyOfColumns(string fileName)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        string[][] data = File.ReadAllLines(fileName).Select(x => x.Split(",")).ToArray();

        for (int i = 0; i < data[0].Length; i++)
        {
            result[data[0][i]] = new List<string>();
            for (int j = 1; j < data.Length; j++)
            {
                result[data[0][i]].Add(data[j][i]);
            }
        }

        return result;
    }
    
    
}
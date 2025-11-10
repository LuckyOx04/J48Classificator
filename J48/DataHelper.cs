namespace J48;

public class DataHelper
{
    public static Dictionary<string, List<string>> CsvToDictionatyOfColumns(string fileName)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        string[][] data = File.ReadAllLines(fileName).Select(x => x.Split(",")).ToArray();

        foreach (var row in data)
        {
            foreach (var column in row)
            {
                Console.Write($"{column},");
            }

            Console.WriteLine();
        }

        return result;
    }
    
    
}
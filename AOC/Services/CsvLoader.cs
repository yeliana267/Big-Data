using CsvHelper;
using System.Globalization;

public class CsvLoader
{
    public List<T> LoadCsv<T>(string filePath)
    {

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}

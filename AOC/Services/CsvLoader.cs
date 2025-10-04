using CsvHelper;
using System.Collections.Generic; // Added for List<T>
using System.Globalization;
using System.IO;
using System.Linq; // Added for ToList()

public class CsvLoader
{
    public List<T> LoadCsv<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Archivo no encontrado: {filePath}");

        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>().ToList();

            Console.WriteLine($"Se cargaron {records.Count} registros desde {Path.GetFileName(filePath)}");
            return records;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer {filePath}: {ex.Message}");
            throw;
        }
    }

}

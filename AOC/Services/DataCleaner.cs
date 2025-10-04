using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

public class DataCleaner
{
    public int CleanId(string rawId)
    {
        if (string.IsNullOrEmpty(rawId)) return 0;
        return int.Parse(new string(rawId.Where(char.IsDigit).ToArray()));
    }

    public string CleanText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return Regex.Replace(text, @"[^\w\s]", "").Trim();
    }

    public DateTime ParseDate(string dateStr)
    {
        return DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    // 🔹 Limpieza automática con Reflection
    public void AutoClean<T>(List<T> list)
    {
        foreach (var item in list)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite) continue; // saltar propiedades sin set/get

                var value = prop.GetValue(item);

                // Caso string → limpiar texto
                if (prop.PropertyType == typeof(string) && value is string strVal)
                {
                    if (prop.Name.ToLower().Contains("id"))
                    {
                        // si parece ID → limpiar dígitos
                        prop.SetValue(item, CleanId(strVal).ToString());
                    }
                    else
                    {
                        // si es texto normal
                        prop.SetValue(item, CleanText(strVal));
                    }
                }
            }
        }
    }
}

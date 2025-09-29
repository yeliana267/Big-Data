using System.Globalization;
using System.Text.RegularExpressions;

public class DataCleaner
{
    public int CleanId(string rawId)
    {
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
}

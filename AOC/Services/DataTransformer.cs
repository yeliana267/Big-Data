using Dapper;
using Npgsql;

public class DataTransformer
{
    private readonly string _connectionString;

    public DataTransformer(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Crea tabla maestra si no existe y genera un mapa dinámico nombre -> id
    /// IDs se generan en código
    /// </summary>
    public Dictionary<string, int> EnsureMasterTable(string tableName, string idColumn, IEnumerable<string> uniqueValues)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        // Consulta correcta sin comillas si la columna no está entre comillas
        var existingMap = conn.Query($"SELECT {idColumn} AS Id, nombre FROM opiniones.{tableName}")
                              .ToDictionary(x => (string)x.nombre, x => (int)x.id);

        int nextId = existingMap.Count > 0 ? existingMap.Values.Max() + 1 : 1;

        foreach (var val in uniqueValues)
        {
            if (!existingMap.ContainsKey(val))
            {
                var sql = $"INSERT INTO opiniones.{tableName} ({idColumn}, nombre) VALUES (@Id, @Nombre)";
                conn.Execute(sql, new { Id = nextId, Nombre = val });
                existingMap[val] = nextId;
                nextId++;
            }
        }

        return existingMap;
    }

    /// <summary>   
    /// Mapea nombres a IDs usando el diccionario generado
    /// </summary>
    public void MapForeignKey<T>(List<T> data, string propertyName, Dictionary<string, int> map)
    {
        foreach (var item in data)
        {
            var propNombre = typeof(T).GetProperty(propertyName + "Nombre");
            var propFk = typeof(T).GetProperty(propertyName);

            if (propNombre == null || propFk == null) continue;

            var value = (string)propNombre.GetValue(item);
            if (value != null && map.TryGetValue(value, out var fkValue))
            {
                propFk.SetValue(item, fkValue);

            }
        }
    }
}
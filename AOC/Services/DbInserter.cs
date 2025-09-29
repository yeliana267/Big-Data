using Dapper;
using Npgsql;
using System.Reflection;

public class DbInserter
{
    private readonly string _connectionString;

    public DbInserter(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Inserta datos en la tabla usando las entidades de la base de datos.
    /// </summary>
    /// <typeparam name="TModel">Clase usada para cargar CSV.</typeparam>
    /// <typeparam name="TEntity">Entidad de base de datos que coincide con la tabla.</typeparam>
    public void InsertAllGeneric<TModel, TEntity>(List<TModel> models, string tableName)
        where TEntity : new()
    {
        var entityType = typeof(TEntity);
        var entityProps = entityType.GetProperties();
        var entities = new List<TEntity>();

        foreach (var model in models)
        {
            var entity = new TEntity();

            foreach (var ep in entityProps)
            {
                // Buscar propiedad en el CSV (Model) que coincida con el nombre de la entidad
                var mp = typeof(TModel).GetProperty(ep.Name,
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (mp != null)
                {
                    var value = mp.GetValue(model);
                    if (value != null)
                        ep.SetValue(entity, value); // solo copiar el valor tal cual
                }
            }

            entities.Add(entity);
        }

        InsertData(entities, tableName);
    }

    private void InsertData<T>(List<T> data, string tableName)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        foreach (var item in data)
        {
            var props = typeof(T).GetProperties()
                                 .Where(p => p.GetValue(item) != null)
                                 .ToList();

            var columns = string.Join(", ", props.Select(p => $"\"{p.Name}\""));
            var values = string.Join(", ", props.Select(p => $"@{p.Name}"));

            // Agregamos ON CONFLICT DO NOTHING para evitar error si ya existe
            var sql = $"INSERT INTO opiniones.{tableName} ({columns}) VALUES ({values}) ON CONFLICT DO NOTHING";
            conn.Execute(sql, item);
        }
    }

}


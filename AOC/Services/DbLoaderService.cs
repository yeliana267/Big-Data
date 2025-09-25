using Npgsql;

public class DbLoaderService
{
    private readonly string _connString;

    public DbLoaderService(string connString)
    {
        _connString = connString;
    }

    public bool ProbarConexion()
    {
        try
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();
            Console.WriteLine("¡Conexión exitosa a PostgreSQL!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al conectar a PostgreSQL: " + ex.Message);
            return false;
        }
    }
}

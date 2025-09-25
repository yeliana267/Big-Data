using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
namespace AOC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .Build();
            var connectionString = config["ConnectionString"];
            var dbService = new DbLoaderService(connectionString);
            dbService.ProbarConexion();


        }
    }
}

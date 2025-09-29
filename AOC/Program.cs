using AOC.Entities;
using AOC.Models;
using Microsoft.Extensions.Configuration;

namespace AOC
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("ConnectionPosgret");

            var dbService = new PruebaConexion(connectionString);
            dbService.ProbarConexion();

            var loader = new CsvLoader();
            var cleaner = new DataCleaner();
            var transformer = new DataTransformer(connectionString);
            var inserter = new DbInserter(connectionString);

            // 1️⃣ Cargar CSVs
            var clientes = loader.LoadCsv<Clients>("C:\\big_data\\AOC\\CSV\\clients.csv");
            var productos = loader.LoadCsv<Products>("C:\\big_data\\AOC\\CSV\\products.csv");
            var opiniones = loader.LoadCsv<Surveys>("C:\\big_data\\AOC\\CSV\\surveys_part1.csv");
            var socialComments = loader.LoadCsv<SocialComments>("C:\\big_data\\AOC\\CSV\\social_comments.csv");
            var fuenteDatos = loader.LoadCsv<FuenteDatos>("C:\\big_data\\AOC\\CSV\\fuente_datos.csv");
            var webReview = loader.LoadCsv<WebReviews>("C:\\big_data\\AOC\\CSV\\web_reviews.csv");

            // 2️⃣ Limpiar datos
            foreach (var s in socialComments)
            {
                s.IdComment = cleaner.CleanId(s.IdComment).ToString();
                s.IdCliente = string.IsNullOrEmpty(s.IdCliente) ? "0" : cleaner.CleanId(s.IdCliente).ToString();
                s.IdProducto = string.IsNullOrEmpty(s.IdProducto) ? "0" : cleaner.CleanId(s.IdProducto).ToString();
                s.Comentario = cleaner.CleanText(s.Comentario);
            }

            foreach (var s in webReview)
            {
                s.IdReview = string.IsNullOrEmpty(s.IdReview) ? "0" : cleaner.CleanId(s.IdReview).ToString();
                s.IdCliente = string.IsNullOrEmpty(s.IdCliente) ? "0" : cleaner.CleanId(s.IdCliente).ToString();
                s.IdProducto = string.IsNullOrEmpty(s.IdProducto) ? "0" : cleaner.CleanId(s.IdProducto).ToString();
                s.Comentario = cleaner.CleanText(s.Comentario);
            }

            foreach (var s in opiniones)
            {
                s.Comentario = cleaner.CleanText(s.Comentario);
            }

            foreach (var p in productos)
                p.Categoria = cleaner.CleanText(p.Categoria);

            foreach (var f in fuenteDatos)
                f.TipoFuente = cleaner.CleanText(f.TipoFuente);

            // 3️⃣ Crear tablas maestras y mapear FK dinámicamente
            var categoriasMap = transformer.EnsureMasterTable("productos_categorias", "\"idCategoria\"", productos.Select(p => p.Categoria).Distinct());
            transformer.MapForeignKey(productos, "Categoria", categoriasMap);

            var clasificacionMap = transformer.EnsureMasterTable("opiniones_clasificacion", "\"idClasificacion\"", opiniones.Select(o => o.Fuente).Distinct());
            transformer.MapForeignKey(opiniones, "Fuente", clasificacionMap);

            var tipoFuenteMap = transformer.EnsureMasterTable("tipo_fuente", "\"idTipo\"", fuenteDatos.Select(f => f.TipoFuente).Distinct());
            transformer.MapForeignKey(fuenteDatos, "TipoFuente", tipoFuenteMap);
            transformer.MapForeignKey(opiniones, "TipoFuente", tipoFuenteMap);

            var tipoRedSocialMap = transformer.EnsureMasterTable("tipo_red_social", "\"idFuente\"", socialComments.Select(s => s.Fuente).Distinct());
            transformer.MapForeignKey(socialComments, "TipoFuente", tipoFuenteMap);

            // 4️⃣ Insertar en DB

            inserter.InsertAllGeneric<Clients, clientes>(clientes, "clientes");
            inserter.InsertAllGeneric<Products, productos>(productos, "products");
            inserter.InsertAllGeneric<Surveys, opiniones>(opiniones, "opiniones");
            inserter.InsertAllGeneric<SocialComments, socialComments>(socialComments, "social_comments");
            inserter.InsertAllGeneric<FuenteDatos, fuenteDatos>(fuenteDatos, "fuente_datos");
            inserter.InsertAllGeneric<WebReviews, webReviews>(webReview, "web_reviews");

            Console.WriteLine("Data insertion completed successfully.");
        }
    }
}

using AOC.Entities;
using AOC.Models;
using Microsoft.Extensions.Configuration;
using AOC.Context;
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

            //  Probar conexión
            var dbService = new PruebaConexion(connectionString);
            if (!dbService.ProbarConexion())
            {
                Console.WriteLine("No se pudo establecer la conexión. Saliendo...");
                return;
            }
            using var context = new Context.AppDbContext(connectionString);
            // 1️⃣ Inicializar servicios
            var loader = new CsvLoader();
            var cleaner = new DataCleaner();
            var transformer = new DataTransformer(context);
            //var inserter = new DbInserter(connectionString);

            // Cargar CSVs
            var clientes = loader.LoadCsv<Clients>("C:\\big_data\\AOC\\CSV\\clients.csv");
            var productos = loader.LoadCsv<Products>("C:\\big_data\\AOC\\CSV\\products.csv");
            var opiniones = loader.LoadCsv<Surveys>("C:\\big_data\\AOC\\CSV\\surveys_part1.csv");
            var socialComments = loader.LoadCsv<SocialComments>("C:\\big_data\\AOC\\CSV\\social_comments.csv");
            var fuenteDatos = loader.LoadCsv<FuenteDatos>("C:\\big_data\\AOC\\CSV\\fuente_datos.csv");
            var webReview = loader.LoadCsv<WebReviews>("C:\\big_data\\AOC\\CSV\\web_reviews.csv");

            //Limpieza de datos

            cleaner.AutoClean(socialComments);
            cleaner.AutoClean(webReview);
            cleaner.AutoClean(opiniones);
            cleaner.AutoClean(productos);
            cleaner.AutoClean(fuenteDatos);


            // -----------------------------
            // 1️⃣ Categorías de productos
            // -----------------------------
            var categoriasMap = transformer.EnsureMasterTable(
                context.ProductosCategorias,
                productos.Select(p => p.Categoria).Distinct(),
                c => c.nombre,               // Cómo obtener el nombre
                (c, v) => c.nombre = v,      // Cómo asignar el nombre
                c => c.idcategoria,          // Cómo obtener el id
                (c, id) => c.idcategoria = id // Cómo asignar el id
            );

            // -----------------------------
            // 2️⃣ Clasificación de opiniones
            // -----------------------------
            var clasificacionMap = transformer.EnsureMasterTable(
                context.OpinionesClasificacion,
                opiniones.Select(o => o.Fuente).Distinct(),
                c => c.nombre,
                (c, v) => c.nombre = v,
                c => c.idclasificacion,
                (c, id) => c.idclasificacion = id
            );

            // -----------------------------
            // 3️⃣ Tipos de fuente
            // -----------------------------
            var tipoFuenteMap = transformer.EnsureMasterTable(
                context.TipoFuente,
                fuenteDatos.Select(f => f.TipoFuente).Distinct(),
                t => t.nombre,
                (t, v) => t.nombre = v,
                t => t.idtipo,
                (t, id) => t.idtipo = id
            );

            // -----------------------------
            // 4️⃣ Tipos de red social
            // -----------------------------
            var tipoRedSocialMap = transformer.EnsureMasterTable(
                context.TipoRedSocial,
                socialComments.Select(s => s.Fuente).Distinct(),
                t => t.nombre,
                (t, v) => t.nombre = v,
                t => t.idfuente,
                (t, id) => t.idfuente = id
            );



            var categorias = context.ProductosCategorias.ToList();

            var productosEntities = productos.Select(p => new productos
            {
                idproducto = p.IdProducto,
                nombre = p.Nombre,

                categoria = categoriasMap.ContainsKey(p.Categoria)
                            ? categoriasMap[p.Categoria]
                            : 0
            }).ToList();

            context.AddIfNotExists(productosEntities, p => p.idproducto);

            var clientesEntities = clientes.Select(c => new clientes
            {
                idcliente = c.IdCliente,
                nombre = c.Nombre,
                email = c.Email,
                // agrega aquí las demás propiedades que tengas
            }).ToList();

            context.AddIfNotExists(clientesEntities, c => c.idcliente);

            var clientesMap = context.Clientes.ToDictionary(c => c.idcliente, c => c.idcliente);
            var productosMap = context.Productos.ToDictionary(p => p.idproducto, p => p.idproducto);

            var webReviewsEntities = webReview.Select(w => new webReviews
            {
                idreview = int.TryParse(w.IdReview, out var rid) ? rid : 0,
                idcliente = int.TryParse(w.IdCliente, out var cid) && clientesMap.ContainsKey(cid) ? cid : 0,
                idproducto = int.TryParse(w.IdProducto, out var pid) && productosMap.ContainsKey(pid) ? pid : 0,
                fecha = w.Fecha, // <-- si viene como string en CSV
                comentario = w.Comentario,
                rating = w.Rating
            }).ToList();

            // 3️⃣ Insertar evitando duplicados
            context.AddIfNotExists(webReviewsEntities, w => w.idreview);



            var socialCommentsEntities = socialComments
    .Select(s => new socialComments
    {
        idcomment = int.TryParse(s.IdComment.Replace("T", ""), out var idc) ? idc : 0,
        idcliente = !string.IsNullOrWhiteSpace(s.IdCliente) && s.IdCliente.StartsWith("C")
            ? int.Parse(s.IdCliente.Substring(1))
            : 0,
        idproducto = !string.IsNullOrWhiteSpace(s.IdProducto) && s.IdProducto.StartsWith("P")
            ? int.Parse(s.IdProducto.Substring(1))
            : 0,
        fuente = tipoRedSocialMap.ContainsKey(s.Fuente.ToLower()) ? tipoRedSocialMap[s.Fuente.ToLower()] : 0,
        fecha = s.Fecha,
        comentario = s.Comentario
    })
    .Where(sc => sc.idcliente > 0 && clientesMap.ContainsKey(sc.idcliente)
              && sc.idproducto > 0 && productosMap.ContainsKey(sc.idproducto)
              && sc.fuente > 0)
    .ToList();


            context.AddIfNotExists(socialCommentsEntities, s => s.idcomment);


            // Traer los mapas existentes

            // Crear entidades para insertar
            var opinionesEntities = opiniones.Select(o => new opiniones
            {
                idopinion = o.IdOpinion,
                idcliente = clientesMap.ContainsKey(o.IdCliente) ? o.IdCliente : 0,
                idproducto = productosMap.ContainsKey(o.IdProducto) ? o.IdProducto : 0,
                fecha = o.Fecha,
                comentario = o.Comentario,
                clasificacion = clasificacionMap.ContainsKey(o.Clasificacion?.ToLower() ?? "") ? clasificacionMap[o.Clasificacion.ToLower()] : 0,
                puntajesatisfaccion = int.TryParse(o.PuntajeSatisfaccion, out var ps) ? ps : 0,
                fuente = tipoFuenteMap.ContainsKey(o.Fuente?.ToLower() ?? "") ? tipoFuenteMap[o.Fuente.ToLower()] : 0
            })
            .Where(o => o.idcliente > 0 && o.idproducto > 0 && o.clasificacion > 0 && o.fuente > 0)
            .ToList();

            // Insertar evitando duplicados
            context.AddIfNotExists(opinionesEntities, o => o.idopinion);


            // Primero asegurarnos de tener el mapa de tipos de fuente

            // Convertir los CSVs a entidades de DB
            var fuenteDatosEntities = fuenteDatos.Select(f => new fuenteDatos
            {
                idfuentedatos = int.TryParse(f.IdFuente.Replace("F", ""), out var fid) ? fid : 0,
                tipofuente = tipoFuenteMap.ContainsKey(f.TipoFuente.ToLower()) ? tipoFuenteMap[f.TipoFuente.ToLower()] : 0,
                fechacarga = f.FechaCarga
            })
            .Where(f => f.tipofuente > 0) // Solo insertamos los que tienen tipofuente válido
            .ToList();

            // Insertar evitando duplicados
            context.AddIfNotExists(fuenteDatosEntities, f => f.idfuentedatos);

            Console.WriteLine("Data insertion completed using EF Core!");

        }
    }
}

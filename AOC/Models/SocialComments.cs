
namespace AOC.Models
{
    public class SocialComments
    {
        public string IdComment { get; set; }
        public string IdCliente { get; set; }
        public string IdProducto { get; set; }
        public string Fuente { get; set; }
        public DateOnly Fecha { get; set; }
        public string Comentario { get; set; }
    }
}

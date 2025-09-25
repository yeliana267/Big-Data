
namespace AOC.Models
{
    public class SocialComments
    {
        public int IdComment { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public int Fuente { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }

    }
}

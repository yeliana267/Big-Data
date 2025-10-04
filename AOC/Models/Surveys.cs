
namespace AOC.Models
{
    public class Surveys
    {
        public int IdOpinion { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public DateOnly Fecha { get; set; }
        public string Comentario { get; set; }
        public string Clasificacion { get; set; }
        public string PuntajeSatisfaccion { get; set; }
        public string Fuente { get; set; }       
    }
}

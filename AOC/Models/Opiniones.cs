public class Opiniones
{
    public int IdOpinion { get; set; }
    public int IdCliente { get; set; }
    public int IdProducto { get; set; }
    public DateTime Fecha { get; set; }
    public string Comentario { get; set; }
    public int Clasificacion { get; set; }
    public int PuntajeSatisfaccion { get; set; }
    public int Fuente { get; set; } 
}
public class opiniones
{
    public int idOpinion { get; set; }
    public int idCliente { get; set; }
    public int idProducto { get; set; }
    public DateTime fecha { get; set; }
    public string comentario { get; set; }
    public int clasificacion { get; set; }
    public int puntajeSatisfaccion { get; set; }
    public int fuente { get; set; } 
}
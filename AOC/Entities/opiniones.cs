using System.ComponentModel.DataAnnotations;

public class opiniones
{
    [Key]
    public int idopinion { get; set; }
    public int idcliente { get; set; }
    public int idproducto { get; set; }
    public DateOnly fecha { get; set; }
    public string comentario { get; set; }
    public int clasificacion { get; set; }
    public int puntajesatisfaccion { get; set; }
    public int fuente { get; set; } 
}
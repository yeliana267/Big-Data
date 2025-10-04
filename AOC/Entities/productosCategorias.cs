using System.ComponentModel.DataAnnotations;

public class productosCategorias
{
    [Key]

    public int idcategoria { get; set; }
    public string nombre { get; set; }
}
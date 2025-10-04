using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class opinionesClasificacion
    {
        [Key]

        public int idclasificacion { get; set; }
        public string nombre { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class tipoFuente
    {
        [Key]

        public int idtipo { get; set; }
        public string nombre { get; set; }
    }
}

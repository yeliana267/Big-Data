using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class productos
    {
        [Key]

        public int idproducto { get; set; }
        public string nombre { get; set; }
        public int categoria { get; set; }
    }
}

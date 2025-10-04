using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class socialComments
    {
        [Key]

        public int idcomment { get; set; }
        public int idcliente { get; set; }
        public int idproducto { get; set; }
        public int fuente { get; set; }
        public DateOnly fecha { get; set; }
        public string comentario { get; set; }

    }
}

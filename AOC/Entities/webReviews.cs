using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class webReviews
    {
        [Key]

        public int idreview { get; set; }
        public int idcliente { get; set; }
        public int idproducto { get; set; }
        public DateOnly fecha { get; set; }
        public string comentario { get; set; }
        public int rating { get; set; }
    }
}

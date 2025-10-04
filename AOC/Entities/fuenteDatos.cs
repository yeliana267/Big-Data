using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class fuenteDatos
    {
        [Key]
        public int idfuentedatos { get; set; }
        public int tipofuente { get; set; }
        public DateOnly fechacarga { get; set; }
    }
}

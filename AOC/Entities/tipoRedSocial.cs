using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class tipoRedSocial
    {
        [Key]

        public int idfuente { get; set; }
        public string nombre { get; set; }
        public int idtipofuente { get; set; }
    }
}

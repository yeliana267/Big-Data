using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AOC.Entities
{
    public class clientes
    {
        [Key]
        public int idcliente { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
    }

}


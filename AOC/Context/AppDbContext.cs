using AOC.Entities;
using Microsoft.EntityFrameworkCore;

namespace AOC.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<clientes> Clientes { get; set; }
        public DbSet<fuenteDatos> FuenteDatos { get; set; }
        public DbSet<opiniones> Opiniones { get; set; }
        public DbSet<opinionesClasificacion> OpinionesClasificacion { get; set; }
        public DbSet<productos> Productos { get; set; }
        public DbSet<productosCategorias> ProductosCategorias { get; set; }
        public DbSet<socialComments> SocialComments { get; set; }
        public DbSet<tipoFuente> TipoFuente { get; set; }
        public DbSet<tipoRedSocial> TipoRedSocial { get; set; }
        public DbSet<webReviews> WebReviews { get; set; }
    }
}

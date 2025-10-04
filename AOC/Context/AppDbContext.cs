using AOC.Entities;
using Microsoft.EntityFrameworkCore;

namespace AOC.Context
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_connectionString);

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<clientes>().ToTable("clientes", "opiniones");
            modelBuilder.Entity<fuenteDatos>().ToTable("fuente_datos", "opiniones");
            modelBuilder.Entity<opinionesClasificacion>().ToTable("opiniones_clasificacion", "opiniones");
            modelBuilder.Entity<productosCategorias>().ToTable("productos_categorias", "opiniones");
            modelBuilder.Entity<socialComments>().ToTable("social_comments", "opiniones");
            modelBuilder.Entity<tipoFuente>().ToTable("tipo_fuente", "opiniones");
            modelBuilder.Entity<tipoRedSocial>().ToTable("tipo_red_social", "opiniones");
            modelBuilder.Entity<webReviews>().ToTable("web_reviews", "opiniones");
            modelBuilder.Entity<productos>().ToTable("productos", "opiniones");
            modelBuilder.Entity<opiniones>().ToTable("opiniones", "opiniones");

        }

    }
}

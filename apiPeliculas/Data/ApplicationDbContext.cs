using apiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace apiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Aqui se deben pasar todas las entidades (Modelos)
        public DbSet<Categoria> Categorias { get; set; }
    }
}

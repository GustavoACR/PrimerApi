using Microsoft.EntityFrameworkCore;

namespace PrimerApi;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Esta línea le dice a EF que cree una tabla llamada "Productos" basada en nuestro modelo
    public DbSet<Producto> Productos { get; set; }
}

using Microsoft.EntityFrameworkCore;

namespace PrimerApi;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Esta línea le dice a EF que cree una tabla llamada "Bebidas" basada en nuestro modelo
    public DbSet<Bebida> Bebidas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}

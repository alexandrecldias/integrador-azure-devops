using Microsoft.EntityFrameworkCore;
using IntegradorApi.Domain;

namespace IntegradorApi.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Parametro> Parametros => Set<Parametro>();
}

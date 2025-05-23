using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration.OpenAdm;

namespace OpenAdm.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<EmpresaOpenAdm> Empresas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmpresaOpenAdmConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Infra.Context;

public class ParceiroContext(DbContextOptions options) 
    : DbContext(options)
{
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
    }
}

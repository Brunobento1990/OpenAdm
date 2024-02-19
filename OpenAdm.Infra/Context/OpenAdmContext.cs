using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Infra.Context;

public class OpenAdmContext(DbContextOptions<OpenAdmContext> options) 
    : DbContext(options)
{
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<ConfiguracaoParceiro> ConfiguracoesParceiro { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoParceiroConfiguration());
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;
using OpenAdm.Infra.EntityConfiguration.OpenAdm;

namespace OpenAdm.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<EmpresaOpenAdm> Empresas { get; set; }
    public DbSet<EnderecoParceiro> EnderecoParceiro { get; set; }
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<TelefoneParceiro> TelefonesParceiro { get; set; }
    public DbSet<RedeSocial> RedesSociais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmpresaOpenAdmConfiguration());
        modelBuilder.ApplyConfiguration(new ParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new TelefoneParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new RedeSocialConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoParceiroConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

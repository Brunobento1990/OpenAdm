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

    public DbSet<ConfiguracoesDePedido> ConfiguracoesDePedidos { get; set; }
    public DbSet<EmpresaOpenAdm> Empresas { get; set; }
    public DbSet<EnderecoParceiro> EnderecoParceiro { get; set; }
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<TelefoneParceiro> TelefonesParceiro { get; set; }
    public DbSet<RedeSocial> RedesSociais { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<ConfiguracaoDePagamento> ConfiguracoesDePagamento { get; set; }
    public DbSet<AcessoEcommerce> AcessosEcommerce { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<LojaParceira> LojasParceiras { get; set; }
    public DbSet<TopUsuario> TopUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmpresaOpenAdmConfiguration());
        modelBuilder.ApplyConfiguration(new ParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new TelefoneParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new RedeSocialConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracoesDePedidoConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDePagamentoConfiguration());
        modelBuilder.ApplyConfiguration(new AcessoEcommerceConfiguration());
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new LojasParceirasConfiguration());
        modelBuilder.ApplyConfiguration(new TopUsuariosConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

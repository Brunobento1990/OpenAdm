using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.EntityConfiguration;
using OpenAdm.Data.EntityConfiguration.OpenAdm;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;
using OpenAdm.Infra.EntityConfiguration.OpenAdm;

namespace OpenAdm.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ConfiguracoesDePedido> ConfiguracoesDePedidos { get; set; }
    public DbSet<EmpresaOpenAdm> Empresas { get; set; }
    public DbSet<LinkEmpresa> LinksEmpresas { get; set; }
    public DbSet<EnderecoParceiro> EnderecoParceiro { get; set; }
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<TelefoneParceiro> TelefonesParceiro { get; set; }
    public DbSet<RedeSocial> RedesSociais { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<FuncionarioEsqueceuSenha> FuncionariosEsqueceramSenha { get; set; }
    public DbSet<ConfiguracaoDePagamento> ConfiguracoesDePagamento { get; set; }
    public DbSet<AcessoEcommerce> AcessosEcommerce { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<LojaParceira> LojasParceiras { get; set; }
    public DbSet<ConfiguracaoDeFrete> ConfiguracoesDeFrete { get; set; }
    public DbSet<EventoAplicacao> EventosAplicacao { get; set; }
    public DbSet<ParcelaCobranca> ParcelasCobrancas { get; set; }
    public DbSet<CobrancaPedidoEcommerce> CobrancasPedidosEcommerce { get; set; }
    public DbSet<LinkBioConfiguracao> LinkBioConfiguracoes { get; set; }
    public DbSet<LinkBioItem> LinkBioItens { get; set; }
    public DbSet<LinkBioEvento> LinkBioEventos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmpresaOpenAdmConfiguration());
        modelBuilder.ApplyConfiguration(new LinkEmpresaConfiguration());
        modelBuilder.ApplyConfiguration(new ParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new TelefoneParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new RedeSocialConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoParceiroConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracoesDePedidoConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioEsqueceuSenhaConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDePagamentoConfiguration());
        modelBuilder.ApplyConfiguration(new AcessoEcommerceConfiguration());
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new LojasParceirasConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDeFreteConfiguration());
        modelBuilder.ApplyConfiguration(new EventoAplicacaoConfiguration());
        modelBuilder.ApplyConfiguration(new ParcelaCobrancaConfiguration());
        modelBuilder.ApplyConfiguration(new CobrancaPedidoEcommerceConfiguration());
        modelBuilder.ApplyConfiguration(new LinkBioConfiguracaoConfiguration());
        modelBuilder.ApplyConfiguration(new LinkBioItemConfiguration());
        modelBuilder.ApplyConfiguration(new LinkBioEventoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Infra.Context;

public class ParceiroContext(DbContextOptions options, IParceiroAutenticado parceiroAutenticado)
    : DbContext(options)
{
    private readonly IParceiroAutenticado _parceiroAutenticado = parceiroAutenticado;
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Peso> Pesos { get; set; }
    public DbSet<Tamanho> Tamanhos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<PesoProduto> PesosProdutos { get; set; }
    public DbSet<TamanhoProduto> TamanhosProdutos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    public DbSet<TabelaDePreco> TabelaDePreco { get; set; }
    public DbSet<ItemTabelaDePreco> ItensTabelaDePreco { get; set; }
    public DbSet<ProdutoMaisVendido> ProdutosMaisVendidos { get; set; }
    public DbSet<ConfiguracaoDeEmail> ConfiguracoesDeEmail { get; set; }
    public DbSet<ConfiguracoesDePedido> ConfiguracoesDePedidos { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<MovimentacaoDeProduto> MovimentacoesDeProdutos { get; set; }
    public DbSet<LojaParceira> LojasParceiras { get; set; }
    public DbSet<TopUsuario> TopUsuarios { get; set; }
    public DbSet<ContasAReceber> ContasAReceber { get; set; }
    public DbSet<FaturaContasAReceber> FaturasContasAReceber { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_parceiroAutenticado.StringConnection);
        //optionsBuilder.UseNpgsql("User ID=postgres; Password=1234; Host=localhost; Port=4449; Database=open-adm-cliente; Pooling=true;");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FaturaContasAReceberConfiguration());
        modelBuilder.ApplyConfiguration(new ContasAReceberConfiguration());
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new PesoConfiguration());
        modelBuilder.ApplyConfiguration(new PesosProdutosConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new TamanhoConfiguration());
        modelBuilder.ApplyConfiguration(new TamanhosProdutosConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        modelBuilder.ApplyConfiguration(new ItensPedidoConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
        modelBuilder.ApplyConfiguration(new TabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ItensTabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutosMaisVendidosConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDeEmailConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracoesDePedidoConfiguration());
        modelBuilder.ApplyConfiguration(new EstoqueConfiguration());
        modelBuilder.ApplyConfiguration(new MovimentacaoDeProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new LojasParceirasConfiguration());
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Infra.Context;

public class ParceiroContext : DbContext
{
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public ParceiroContext(
        DbContextOptions<ParceiroContext> options,
        IParceiroAutenticado parceiroAutenticado) : base(options)
    {
        _parceiroAutenticado = parceiroAutenticado;
    }


    public DbSet<EnderecoEntregaPedido> EnderecosEntregaPedido { get; set; }
    public DbSet<ConfiguracaoDePagamento> ConfiguracoesDePagamento { get; set; }
    public DbSet<Banner> Banners { get; set; }
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
    public DbSet<ConfiguracaoDeFrete> ConfiguracoesDeFrete { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<MovimentacaoDeProduto> MovimentacoesDeProdutos { get; set; }
    public DbSet<LojaParceira> LojasParceiras { get; set; }
    public DbSet<TopUsuario> TopUsuarios { get; set; }
    public DbSet<Fatura> Faturas { get; set; }
    public DbSet<Parcela> Parcelas { get; set; }
    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
    public DbSet<AcessoEcommerce> AcessosEcommerce { get; set; }
    public DbSet<EnderecoUsuario> EnderecoUsuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TabelaDePreco>().HasData(new TabelaDePreco(
            id: Guid.Parse("4b43157b-8029-4e55-be53-1b8d27ea9be2"),
            dataDeCriacao: DateTime.MinValue,
            dataDeAtualizacao: DateTime.MinValue,
            numero: 1,
            descricao: "E-commerce",
            ativaEcommerce: true));
        modelBuilder.ApplyConfiguration(new AcessoEcommerceConfiguration());
        modelBuilder.ApplyConfiguration(new TransacaoFinanceiraConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoEntregaPedidoConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDePagamentoConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDeFreteConfiguration());
        modelBuilder.ApplyConfiguration(new ParcelaConfiguration());
        modelBuilder.ApplyConfiguration(new FaturaConfiguration());
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
        modelBuilder.ApplyConfiguration(new TabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ItensTabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutosMaisVendidosConfiguration());
        modelBuilder.ApplyConfiguration(new EstoqueConfiguration());
        modelBuilder.ApplyConfiguration(new MovimentacaoDeProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new LojasParceirasConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoUsuarioConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_parceiroAutenticado.ConnectionString);
        //optionsBuilder.UseNpgsql("User ID=postgres; Password=1234; Host=localhost; Port=4814; Database=dev; Pooling=true;");
        base.OnConfiguring(optionsBuilder);
    }
}

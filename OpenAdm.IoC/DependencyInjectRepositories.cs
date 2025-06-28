using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Azure.Storage;
using OpenAdm.Infra.Cached.Cached;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Cached.Services;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.IoC;

public static class DependencyInjectRepositories
{
    public static void InjectRepositories(this IServiceCollection services, string connectionString, string instanceName)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = instanceName;
        });

        services.AddScoped(typeof(IGenericBaseRepository<>), typeof(GenericBaseRepository<>));
        services.AddScoped(typeof(ICachedService<>), typeof(CachedService<>));
        services.AddScoped<IAcessoEcommerceRepository, AcessoEcommerceRepository>();

        services.AddScoped<EmpresaOpenAdmRepository>();
        services.AddScoped<IEmpresaOpenAdmRepository, EmpresaOpenAdmCached>();

        services.AddScoped<IConfiguracaoDePagamentoRepository, ConfiguracaoDePagamentoRepository>();
        services.AddScoped<ILoginUsuarioRepository, LoginUsuarioRepository>();
        services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
        services.AddScoped<ITransacaoFinanceiraRepository, TransacaoFinanceiraRepository>();
        services.AddScoped<IBannerRepository, BannerRepository>();

        services.AddScoped<ConfiguracaoDeFreteRepository>();
        services.AddScoped<IConfiguracaoDeFreteRepository, ConfiguracaoDeFreteCached>();

        services.AddScoped<ILoginFuncionarioRepository, LoginFuncionarioRepository>();

        services.AddScoped<IPedidoRepository, PedidoRepository>();

        services.AddScoped<CategoriaRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaCached>();

        //services.AddScoped<ProdutoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<UsuarioRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioCached>();

        services.AddScoped<IItensPedidoRepository, ItensPedidoRepository>();
        services.AddScoped<ITabelaDePrecoRepository, TabelaDePrecoRepository>();
        services.AddScoped<IConfiguracoesDePedidoRepository, ConfiguracoesDePedidoRepository>();
        services.AddScoped<PesoRepository>();
        services.AddScoped<IPesoRepository, PesoCached>();
        services.AddScoped<TamanhoRepository>();
        services.AddScoped<ITamanhoRepository, TamanhoCached>();
        services.AddScoped<IPesosProdutosRepository, PesosProdutosRepository>();
        services.AddScoped<ITamanhosProdutoRepository, TamanhosProdutoRepository>();
        services.AddScoped<IItemTabelaDePrecoRepository, ItemTabelaDePrecoRepository>();
        services.AddScoped<IUploadImageBlobClient, UploadImageBlobClient>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IMovimentacaoDeProdutoRepository, MovimentacaoDeProdutoRepository>();
        services.AddScoped<ILojasParceirasRepository, LojasParceirasRepository>();
        services.AddScoped<TopUsuariosRepository>();
        services.AddScoped<ITopUsuariosRepository, TopUsuariosCached>();
        services.AddScoped<IProdutosMaisVendidosRepository, ProdutosMaisVendidosRepository>();
        services.AddScoped<IFaturaRepository, FaturaRepository>();
        services.AddScoped<IParcelaRepository, ParcelaRepository>();
        services.AddScoped<IEnderecoEntregaPedidoRepository, EnderecoEntregaPedidoRepository>();
        services.AddScoped<IParceiroRepository, ParceiroRepository>();
        services.AddScoped<IMigrationService, MigrationRepository>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Services;
using OpenAdm.Application.Services.Carrinhos;

namespace OpenAdm.IoC;

public static class DependencyInjectyApplication
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ILoginFuncionarioService, LoginFuncionarioService>();
        services.AddTransient<IPedidoService, PedidoService>();
        services.AddScoped<IHomeSevice, HomeSevice>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<ILoginUsuarioService, LoginUsuarioService>();
        services.AddScoped<ICarrinhoService, CarrinhoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IItensPedidoService, ItensPedidoService>();
        services.AddScoped<IEsqueceuSenhaService, EsqueceuSenhaService>();
        services.AddScoped<IProcessarPedidoService, ProcessarPedidoService>();
        services.AddScoped<IPesoService, PesoService>();
        services.AddScoped<ITamanhoService, TamanhoService>();
        services.AddScoped<ITabelaDePrecoService, TabelaDePrecoService>();
        services.AddScoped<IConfiguracoesDePedidoService, ConfiguracoesDePedidoService>();
        services.AddScoped<IConfiguracoesDeEmailService, ConfiguracoesDeEmailService>();
        services.AddScoped<IItemTabelaDePrecoService, ItemTabelaDePrecoService>();
        services.AddScoped<IMovimentacaoDeProdutosService, MovimentacaoDeProdutosService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<ILojasParceirasService, LojasParceirasService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAddCarrinhoService, AddCarrinhoService>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using System;

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
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEsqueceuSenhaService, EsqueceuSenhaService>();
    }
}

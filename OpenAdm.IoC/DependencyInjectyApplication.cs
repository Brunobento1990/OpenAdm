using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;

namespace OpenAdm.IoC;

public static class DependencyInjectyApplication
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ILoginFuncionarioService, LoginFuncionarioService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IHomeSevice, HomeSevice>();
    }
}

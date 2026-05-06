using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Infra.QueryService;

namespace OpenAdm.IoC;

public static class DependencyInjectCqs
{
    public static IServiceCollection InjectCqs(this IServiceCollection services)
    {
        services.AddScoped<ICobrancaPedidoQueryService, CobrancaPedidoQueryService>();
        services.AddScoped<ICategoriaEcommerceService, CategoriaEcommerceService>();
        services.AddScoped<IBannerEcommerceService, BannerEcommerceService>();

        return services;
    }
}
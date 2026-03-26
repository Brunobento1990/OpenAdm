using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Infra.QueryService;

namespace OpenAdm.IoC;

public static class DependencyInjectCqs
{
    public static IServiceCollection InjectCqs(this IServiceCollection services)
    {
        services.AddScoped<ICobrancaPedidoQueryService, CobrancaPedidoQueryService>();

        return services;
    }
}
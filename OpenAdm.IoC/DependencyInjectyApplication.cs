using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;

namespace OpenAdm.IoC;

public static class DependencyInjectyApplication
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IBannerService, BannerService>();
    }
}

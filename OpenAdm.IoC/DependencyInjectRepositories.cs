using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Factories.Factory;
using OpenAdm.Infra.Factories.Interfaces;
using OpenAdm.Infra.Repositories;


namespace OpenAdm.IoC;

public static class DependencyInjectRepositories
{
    public static void InjectRepositories(this IServiceCollection services)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IDomainFactory, DomainFactory>();
        services.AddTransient<IParceiroContextFactory, ParceiroContextFactory>();

        services.AddScoped<IConfiguracaoParceiroRepository, ConfiguracaoParceiroRepository>();

        services.AddScoped<IBannerRepository, BannerRepository>();
    }
}

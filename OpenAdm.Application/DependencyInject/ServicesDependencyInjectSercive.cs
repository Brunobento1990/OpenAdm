using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;

namespace OpenAdm.Application.DependencyInject;

public static class ServicesDependencyInjectSercive
{
    public static IServiceCollection AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<IConsultaCepService, ConsultaCepService>();
        services.AddScoped<IEnderecoUsuarioService, EnderecoUsuarioService>();

        return services;
    }
}

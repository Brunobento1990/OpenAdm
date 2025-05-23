using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Model;

namespace OpenAdm.IoC;

public static class DependencyInjectContext
{
    public static IServiceCollection InjectContext(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddDbContext<ParceiroContext>(contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);
        services.AddScoped<IUsuarioAutenticado, UsuarioAutenticado>();
        services.AddScoped<IParceiroAutenticado, ParceiroAutenticado>();

        return services;
    }
}

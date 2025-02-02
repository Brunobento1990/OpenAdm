using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.IoC;

public static class DependencyInjectContext
{
    public static IServiceCollection InjectContext(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ParceiroContext>(opt => opt.UseNpgsql(connectionString));
        services.AddScoped<IUsuarioAutenticado, UsuarioAutenticado>();

        return services;
    }
}

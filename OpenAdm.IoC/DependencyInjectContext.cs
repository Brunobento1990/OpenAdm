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
        services.AddDbContext<OpenAdmContext>(opt => opt.UseNpgsql(connectionString));
        services.AddDbContext<ParceiroContext>();
        services.AddScoped<IParceiroAutenticado, ParceiroAutenticado>();
        services.AddScoped<IUsuarioAutenticado, UsuarioAutenticado>();

        return services;
    }
}

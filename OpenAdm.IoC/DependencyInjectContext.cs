using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Infra.Context;

namespace OpenAdm.IoC;

public static class DependencyInjectContext
{
    public static void InjectContext(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<OpenAdmContext>(opt => opt.UseNpgsql(connectionString));
        //services.AddDbContext<ParceiroContext>(opt => opt.UseNpgsql(connectionString));
    }
}

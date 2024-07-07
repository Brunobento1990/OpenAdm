using Domain.Pkg.Entities;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class AppLogRepository : IAppLogRepository
{
    private readonly IServiceProvider _serviceProvider;

    public AppLogRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task AddAsync(AppLog appLog)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OpenAdmContext>();
        await context.AddAsync(appLog);
        await context.SaveChangesAsync();
    }
}

using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.HttpService.Services;

namespace OpenAdm.IoC;

public static class DependencyInjectIHttpClient
{
    public static void InjectHttpClient(this IServiceCollection services, string url)
    {
        services.AddTransient<IDiscordHttpService, DiscordHttpService>();
        services.AddHttpClient("Discord", x =>
        {
            x.BaseAddress = new Uri(url);
        });
    }
}

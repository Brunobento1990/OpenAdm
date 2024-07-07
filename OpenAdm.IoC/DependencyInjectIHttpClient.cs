using Domain.Pkg.Interfaces;
using Domain.Pkg.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
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
        services.AddScoped<IEmailService, EmailService>();
    }

    public static void InjectHttpClientFrete(this IServiceCollection services, string url, string token)
    {
        services.AddScoped<IFreteService, FreteService>();
        services.AddHttpClient("Frete", x =>
        {
            x.BaseAddress = new Uri(url);
            x.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        });
        services.AddScoped<IEmailService, EmailService>();
    }
}

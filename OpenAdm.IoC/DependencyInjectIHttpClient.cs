using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.HttpService.Services;

namespace OpenAdm.IoC;

public static class DependencyInjectIHttpClient
{
    public static void InjectHttpClient(this IServiceCollection services,
        string url,
        string urlApiCep,
        string urlMercadoPago,
        string urlConsultaCnpj)
    {
        services.AddTransient<ICepHttpService, CepHttpService>();
        services.AddTransient<IDiscordHttpService, DiscordHttpService>();
        services.AddTransient<IMercadoPagoHttpService, MercadoPagoHttpService>();
        services.AddTransient<ICnpjHttpService, CnpjHttpService>();
        services.AddHttpClient("Discord", x =>
        {
            x.BaseAddress = new Uri(url);
        });

        services.AddHttpClient("MercadoPago", x =>
        {
            x.BaseAddress = new Uri(urlMercadoPago);
        });

        services.AddHttpClient("CepHttpService", x =>
        {
            x.BaseAddress = new Uri(urlApiCep);
        });

        services.AddHttpClient(HttpServiceEnum.ConsultaCnpj.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlConsultaCnpj);
        });
    }
}

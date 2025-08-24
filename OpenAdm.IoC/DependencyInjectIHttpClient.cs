using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.HttpClient.Interfaces;
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
        string urlConsultaCnpj,
        string urlApiViaCep,
        string urlWhatsApp,
        string keyWhatsApp)
    {
        services.AddTransient<IHttpClientCep, CepHttpService>();
        services.AddTransient<IDiscordHttpService, DiscordHttpService>();
        services.AddTransient<IHttpClientMercadoPago, MercadoPagoHttpService>();
        services.AddTransient<IHttpClientConsultaCnpj, CnpjHttpService>();
        services.AddHttpClient(HttpServiceEnum.Discord.ToString(), x =>
        {
            x.BaseAddress = new Uri(url);
        });

        services.AddHttpClient(HttpServiceEnum.MercadoPago.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlMercadoPago);
        });

        services.AddHttpClient(HttpServiceEnum.CepHttpService.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlApiCep);
        });

        services.AddHttpClient(HttpServiceEnum.ConsultaCnpj.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlConsultaCnpj);
        });

        services.AddHttpClient(HttpServiceEnum.ConsultaCep.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlApiViaCep);
        });

        services.AddScoped<IChatWppHttpClient, ChatWppHttpClient>();

        services.AddHttpClient(HttpServiceEnum.WhatsApp.ToString(), x =>
        {
            x.BaseAddress = new Uri(urlWhatsApp);
            x.DefaultRequestHeaders.Add("apiKey", keyWhatsApp);
        });
    }
}

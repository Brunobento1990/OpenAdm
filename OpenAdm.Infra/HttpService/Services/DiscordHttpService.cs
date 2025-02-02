using OpenAdm.Infra.HttpService.Interfaces;
using System.Net.Http.Headers;
using OpenAdm.Infra.Model;
using OpenAdm.Infra.Enums;

namespace OpenAdm.Infra.HttpService.Services;

public class DiscordHttpService(IHttpClientFactory httpClientFactory) 
    : IDiscordHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly string _nomeCliente = HttpServiceEnum.Discord.ToString();
    public async Task NotifyExceptionAsync(DiscordModel discordModel, string webHookId, string webHookToken)
    {
        if (string.IsNullOrWhiteSpace(webHookId))
            throw new Exception("Web hook do discord inválido");

        if (string.IsNullOrWhiteSpace(webHookToken))
            throw new Exception("Web token do discord inválido");

        var url = $"{webHookId}/{webHookToken}";
        var httpClient = _httpClientFactory.CreateClient(_nomeCliente);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await httpClient.PostAsync(url, discordModel.ToJson());
    }
}

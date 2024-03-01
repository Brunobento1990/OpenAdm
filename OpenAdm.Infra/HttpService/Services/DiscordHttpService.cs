using OpenAdm.Infra.HttpService.Interfaces;
using System.Net.Http.Headers;
using OpenAdm.Infra.Model;
using Domain.Pkg.Errors;

namespace OpenAdm.Infra.HttpService.Services;

public class DiscordHttpService(IHttpClientFactory httpClientFactory) 
    : IDiscordHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task NotifyExceptionAsync(DiscordModel discordModel, string webHookId, string webHookToken)
    {
        if (string.IsNullOrWhiteSpace(webHookId))
            throw new Exception(CodigoErrors.WebHookIdDiscordInvalido);

        if (string.IsNullOrWhiteSpace(webHookToken))
            throw new Exception(CodigoErrors.WebHookTokenDiscordInvalido);

        var url = $"{webHookId}/{webHookToken}";
        var httpClient = _httpClientFactory.CreateClient("Discord");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await httpClient.PostAsync(url, discordModel.ToJson());
    }
}

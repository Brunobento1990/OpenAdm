using OpenAdm.Infra.HttpService.Interfaces;
using System.Net.Http.Headers;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Services;

public class DiscordHttpService : IDiscordHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DiscordHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task NotifyExceptionAsync(string message, string webHookId, string webHookToken)
    {
        var url = $"{webHookId}/{webHookToken}";
        var httpClient = _httpClientFactory.CreateClient("Discord");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await httpClient.PostAsync(url, BodyErrorDiscord(message));
    }

    private static StringContent BodyErrorDiscord(string message)
    {
        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = message,
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        return discordModel.ToJson();
    }
}

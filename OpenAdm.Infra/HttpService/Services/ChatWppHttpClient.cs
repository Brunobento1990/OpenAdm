using OpenAdm.Application.Dtos.WhatsApp;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Services;

public class ChatWppHttpClient : IChatWppHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatWppHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> EnviarPdfAsync(EnviarPDFWppDTO enviarPDFWppDTO)
    {
        var client = _httpClientFactory.CreateClient($"{HttpServiceEnum.WhatsApp}");

        var response = await client.PostAsync("/message/sendMedia/open-adm", JsonSerializerOptionsApi.ToJson(enviarPDFWppDTO));
        var responseBody = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode;
    }
}

using Microsoft.Extensions.Configuration;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Model;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.HttpService.Request;
using OpenAdm.Worker.Application.HttpService.Response;
using OpenAdm.Worker.Infra.Enum;

namespace OpenAdm.Worker.Infra.HttpClient;

public class HttpClientWhatsApp : IHttpClientWhatsApp
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _instance = "open-adm";

    public HttpClientWhatsApp(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        var instance = configuration["WhatsApp:Instance"];
        if (!string.IsNullOrWhiteSpace(instance))
        {
            _instance = instance.ToLower();
        }
    }

    public async Task<bool> EnviarPdfAsync(EnviarPDFWppRequest request)
    {
        using var client = _httpClientFactory.CreateClient(nameof(HttpClientEnum.WhatsApp));

        var response = await client.PostAsync($"message/sendMedia/{_instance}",
            JsonSerializerOptionsApi.ToJson(request));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EnviarMsgAsync(EnviarMsgWppRequest request)
    {
        using var client = _httpClientFactory.CreateClient(nameof(HttpClientEnum.WhatsApp));

        var response = await client.PostAsync($"message/sendText/{_instance}",
            JsonSerializerOptionsApi.ToJson(request));
        return response.IsSuccessStatusCode;
    }

    public async Task<ResultPartner<StatusConexaoWhatsAppResponse>> StatusConexaoAsync()
    {
        using var client = _httpClientFactory.CreateClient(nameof(HttpClientEnum.WhatsApp));

        var response = await client.GetAsync($"/instance/connectionState/{_instance}");

        if (!response.IsSuccessStatusCode)
        {
            var erroBody = await response.Content.ReadAsStringAsync();

            return (ResultPartner<StatusConexaoWhatsAppResponse>)(string.IsNullOrWhiteSpace(erroBody)
                ? "Sem response do erro"
                : erroBody);
        }

        var body = await response.Content.ReadAsStreamAsync();

        var bodyParse = JsonSerializerOptionsApi.FromJson<StatusConexaoWhatsAppResponse>(body);

        if (bodyParse == null)
        {
            return (ResultPartner<StatusConexaoWhatsAppResponse>)
                "Não foi possível dar parse no json da resposta da conexão do whatsApp";
        }

        return (ResultPartner<StatusConexaoWhatsAppResponse>)bodyParse;
    }
}
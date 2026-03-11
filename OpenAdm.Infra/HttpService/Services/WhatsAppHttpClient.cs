using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Dtos.WhatsApp;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Services;

public class WhatsAppHttpClient : IWhatsAppHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _instance = "open-adm";

    public WhatsAppHttpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        var instance = configuration["WhatsApp:Instance"];
        if (!string.IsNullOrWhiteSpace(instance))
        {
            _instance = instance.ToLower();
        }
    }

    public async Task<bool> EnviarPdfAsync(EnviarPDFWppDTO enviarPDFWppDTO)
    {
        var client = _httpClientFactory.CreateClient($"{HttpServiceEnum.WhatsApp}");

        var response = await client.PostAsync($"message/sendMedia/{_instance}",
            JsonSerializerOptionsApi.ToJson(enviarPDFWppDTO));
        //var responseBody = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode;
    }

    public async Task<ResultPartner<StatusConexaoWhatsAppResponse>> StatusConexaoAsync()
    {
        var client = _httpClientFactory.CreateClient($"{HttpServiceEnum.WhatsApp}");

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
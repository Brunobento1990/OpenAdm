using System.Text.RegularExpressions;
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
            _instance = instance;
        }
    }
    
    private string NormalizarTelefoneWuzApi(string telefone)
    {
        telefone = Regex.Replace(telefone, @"\D", "");
        
        if (telefone.Length == 13 &&
            telefone.StartsWith("55") &&
            telefone[4] == '9')
        {
            telefone = telefone.Remove(4, 1);
        }

        return telefone;
    }

    public async Task<bool> EnviarMsgAsync(EnviarMsgWppRequest request)
    {
        using var client = _httpClientFactory.CreateClient(nameof(HttpClientEnum.WhatsApp));

        var response = await client.PostAsync("chat/send/text",
            JsonSerializerOptionsApi.ToJson(new EnviarMsgWuzApiWppRequest()
            {
                Phone = NormalizarTelefoneWuzApi(request.Number),
                Body = request.Text
            }));
        return response.IsSuccessStatusCode;
    }

    public async Task<ResultPartner<StatusConexaoWhatsAppResponse>> StatusConexaoAsync()
    {
        using var client = _httpClientFactory.CreateClient(nameof(HttpClientEnum.WhatsApp));

        var response = await client.GetAsync($"instance/connectionState/{_instance}");

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

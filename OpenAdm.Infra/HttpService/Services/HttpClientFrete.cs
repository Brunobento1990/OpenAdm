using System.Net.Http.Headers;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Services;

public class HttpClientFrete : IHttpClientFrete
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientFrete(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ResultPartner<DadosCotacaoFreteResponse>> CotarFreteAsync(CotacaoFreteRequest request, string jwt)
    {
        var httpClient = _httpClientFactory.CreateClient($"{HttpServiceEnum.Frete}");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response =
            await httpClient.PostAsync("api/v2/me/shipment/calculate", JsonSerializerOptionsApi.ToJson(request));

        var body = await response.Content.ReadAsStreamAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializerOptionsApi.FromJson<ErroFreteResponse>(body);

            return (ResultPartner<DadosCotacaoFreteResponse>)(error?.Message ?? "Erro desconhecido ao cotar o frete");
        }

        var bodyParse = JsonSerializerOptionsApi.FromJson<ICollection<CotacaoFreteResponse>>(body);

        if (bodyParse == null)
        {
            return (ResultPartner<DadosCotacaoFreteResponse>)
                "Erro desconhecido ao converter a resposta da cotação de frete";
        }

        return (ResultPartner<DadosCotacaoFreteResponse>)new DadosCotacaoFreteResponse()
        {
            Dados = bodyParse
        };
    }
}
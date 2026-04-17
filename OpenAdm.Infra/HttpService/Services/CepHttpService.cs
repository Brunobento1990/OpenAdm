using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Enums;
using System.Text.Json;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Infra.HttpService.Services;

public sealed class CepHttpService : IHttpClientCep
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CepHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConsultaCepResponse> ConsultaCepAsync(string cepOrigem)
    {
        var client = _httpClientFactory.CreateClient($"{HttpServiceEnum.ConsultaCep}");
        var url = $"/ws/{cepOrigem}/json";
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new ExceptionApi($"Não foi possível consultar o cep: {cepOrigem}");
        }

        var body = await response.Content.ReadAsStreamAsync();

        return JsonSerializer.Deserialize<ConsultaCepResponse>(body, JsonSerializerOptionsApi.Options)
               ?? throw new ExceptionApi($"Erro ao consultar o CEP: {cepOrigem}");
    }
}
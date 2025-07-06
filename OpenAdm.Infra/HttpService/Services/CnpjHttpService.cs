using System.Text.Json;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Services;

public class CnpjHttpService : IHttpClientConsultaCnpj
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _nomeCliente = HttpServiceEnum.ConsultaCnpj.ToString();
    public CnpjHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConsultaCnpjResponse> ConsultarCnpjAsync(string cnpj)
    {
        var client = _httpClientFactory.CreateClient(_nomeCliente);

        var response = await client.GetAsync(cnpj);
        var body = await response.Content.ReadAsStreamAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (body == null || body.Length == 0)
            {
                throw new ExceptionApi("Não foi possível obter a resposta da consulta do seu CNPJ");
            }

            var erro = JsonSerializer.Deserialize<ConsultaCnpjErroResponse>(body, JsonSerializerOptionsApi.Options());

            if (!string.IsNullOrWhiteSpace(erro?.Message))
            {
                throw new ExceptionApi(erro.Message);
            }

            throw new ExceptionApi("Não foi possível efetuar a consulta do seu CNPJ");
        }

        return JsonSerializer.Deserialize<ConsultaCnpjResponse>(body, JsonSerializerOptionsApi.Options())
            ?? throw new ExceptionApi("Não foi possível efetuar a consulta do seu CNPJ!");
    }
}

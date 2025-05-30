﻿using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.Model;
using System.Text.Json;

namespace OpenAdm.Infra.HttpService.Services;

public sealed class CepHttpService : IHttpClientCep
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _nomeCliente = $"{HttpServiceEnum.CepHttpService.ToString()}";

    public CepHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConsultaCepResponse> ConsultaCepAsync(string cepOrigem)
    {
        var client = _httpClientFactory.CreateClient(_nomeCliente);
        var url = $"ws/json/{cepOrigem}";
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var erro = string.IsNullOrWhiteSpace(body) ? $"Erro ao consultar o CEP: {cepOrigem}" : body;
            throw new Exception(erro);
        }

        if (body.Contains("CEP not found"))
        {
            throw new ExceptionApi("CEP inválido!");
        }

        return JsonSerializer.Deserialize<ConsultaCepResponse>(body, JsonSerializerOptionsApi.Options())
            ?? throw new Exception($"Erro ao consultar o CEP: {cepOrigem}");
    }

    public async Task<CotacaoFreteResponse> CotarFreteAsync(CotacaoFreteRequest cotacaoFreteRequest)
    {
        var client = _httpClientFactory.CreateClient(_nomeCliente);
        var url = $"ws/json-frete/{cotacaoFreteRequest.CepOrigem}/{cotacaoFreteRequest.CepDestino}/{cotacaoFreteRequest.Peso}/{cotacaoFreteRequest.Altura}/{cotacaoFreteRequest.Largura}/{cotacaoFreteRequest.Comprimento}/{cotacaoFreteRequest.ChaveDeAcesso}";
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var erro = string.IsNullOrWhiteSpace(body) ? "Erro na cotação de frete" : body;
            throw new Exception(erro);
        }

        return JsonSerializer.Deserialize<CotacaoFreteResponse>(body, JsonSerializerOptionsApi.Options())
            ?? throw new Exception("Erro ao desserealizar response da api cotação de frete");
    }
}

﻿using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;
using System.Net.Http.Headers;
using System.Text.Json;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Infra.HttpService.Services;

public sealed class MercadoPagoHttpService : IMercadoPagoHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MercadoPagoHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MercadoPagoResponse> PostAsync(MercadoPagoRequest mercadoPagoRequest, string accessToken, string idempotencyKey)
    {
        var httpClient = _httpClientFactory.CreateClient("MercadoPago");

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("X-Idempotency-Key", idempotencyKey);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.PostAsync("payments", JsonSerializerOptionsApi.ToJson(mercadoPagoRequest));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(error);
            throw new ExceptionApi($"Não foi possível gerar o pagamento.", enviarErroDiscord: true);
        }
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MercadoPagoResponse>(content, JsonSerializerOptionsApi.Options())
            ?? throw new Exception("Não foi possível desserealizar o objeto response do mercado pago!");
    }
}

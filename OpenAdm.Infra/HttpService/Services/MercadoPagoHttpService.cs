using OpenAdm.Infra.Model;
using System.Net.Http.Headers;
using System.Text.Json;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Infra.Enums;
using Serilog;

namespace OpenAdm.Infra.HttpService.Services;

public sealed class MercadoPagoHttpService : IHttpClientMercadoPago
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _nomeCliente = HttpServiceEnum.MercadoPago.ToString();

    public MercadoPagoHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MercadoPagoPagamentoResponse> GerarPagamentoAsync(
        MercadoPagoPagamentoRequest mercadoPagoPagamentoRequest, string accessToken, string idempotencyKey)
    {
        using var httpClient = _httpClientFactory.CreateClient(_nomeCliente);

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("X-Idempotency-Key", idempotencyKey);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response =
            await httpClient.PostAsync("/v1/payments", JsonSerializerOptionsApi.ToJson(mercadoPagoPagamentoRequest));
        var body = await response.Content.ReadAsStreamAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Log.Error(error);   
            throw new ExceptionApi($"Não foi possível gerar o pagamento.");
        }

        return JsonSerializer.Deserialize<MercadoPagoPagamentoResponse>(body, JsonSerializerOptionsApi.Options())
               ?? throw new Exception("Não foi possível desserealizar o objeto response do mercado pago!");
    }
}
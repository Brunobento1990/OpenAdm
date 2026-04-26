using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientMercadoPago
{
    Task<MercadoPagoPagamentoResponse> GerarPagamentoAsync(MercadoPagoPagamentoRequest mercadoPagoPagamentoRequest,
        string accessToken, string idempotencyKey);

    Task<ObterPagamentoMercadoPagoResponse> ObterPagamentoPagamentoAsync(string pagamentoId, string accessToken);
}
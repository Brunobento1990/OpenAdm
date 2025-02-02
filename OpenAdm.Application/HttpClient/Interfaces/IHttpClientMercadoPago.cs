using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientMercadoPago
{
    Task<MercadoPagoResponse> PostAsync(MercadoPagoRequest mercadoPagoRequest, string accessToken, string idempotencyKey);
}

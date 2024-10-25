using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Interfaces;

public interface IMercadoPagoHttpService
{
    Task<MercadoPagoResponse> PostAsync(MercadoPagoRequest mercadoPagoRequest, string accessToken);
}

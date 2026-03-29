using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class GerarCobrancaPixPedidoService : IGerarCobrancaPedidoService
{
    private readonly IHttpClientMercadoPago _httpClientMercadoPago;

    public GerarCobrancaPixPedidoService(
        IHttpClientMercadoPago httpClientMercadoPago)
    {
        _httpClientMercadoPago = httpClientMercadoPago;
    }

    public async Task<ResultPartner<PagamentoViewModel>> GerarCobrancaAsync(Pedido pedido, string urlWebHook,
        string accessToken)
    {
        var mercadoPagoRequest = GerarBodyMercadoPado(pedido, pedido.ValorTotalCobrar, urlWebHook);
        var resultPix = await _httpClientMercadoPago
            .GerarPagamentoAsync(mercadoPagoRequest, accessToken, pedido.Id.ToString());

        return (ResultPartner<PagamentoViewModel>)new PagamentoViewModel()
        {
            LinkPagamento = resultPix.Point_of_interaction?.Transaction_data?.Ticket_url,
            QrCodePixBase64 = resultPix.Point_of_interaction?.Transaction_data?.Qr_code_base64,
            QrCodePix = resultPix.Point_of_interaction?.Transaction_data?.Qr_code,
            IdExterno = resultPix.Id.ToString()
        };
    }

    private static MercadoPagoPagamentoRequest GerarBodyMercadoPado(Pedido pedido, decimal valor, string urlWebHook)
    {
        return new MercadoPagoPagamentoRequest()
        {
            Description = $"Pedido {pedido.Numero}",
            Transaction_amount = valor,
            External_reference = pedido.Id.ToString(),
            Notification_url = urlWebHook,
            Payer = new()
            {
                Email = pedido.Usuario.Email,
                First_name = pedido.Usuario.Nome,
                Identification = new()
                {
                    Type = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? "CPF" : "CNPJ",
                    Number = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj)
                        ? pedido.Usuario.Cpf ?? ""
                        : pedido.Usuario.Cnpj ?? ""
                }
            }
        };
    }
}
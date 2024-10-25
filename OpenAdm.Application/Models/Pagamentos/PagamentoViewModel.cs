using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Pagamentos;

public class PagamentoViewModel
{
    public string? QrCodePix { get; set; }
    public string? QrCodePixBase64 { get; set; }
    public string? LinkPagamento { get; set; }
    public string? MercadoPagoId { get; set; }
    public decimal Total { get; set; }
    public int QuantidadeDeParcelas { get; set; }
    public DateTime PrimeiroVencimento { get; set; }
}

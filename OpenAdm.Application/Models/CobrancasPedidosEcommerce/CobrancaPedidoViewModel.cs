using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.CobrancasPedidosEcommerce;

public sealed class CobrancaPedidoViewModel
{
    public Guid Id { get; set; }
    public long Numero { get; set; }
    public Guid PedidoId { get; set; }
    public decimal Total { get; set; }
    public StatusCobrancaPedidoEcommerceEnum Status { get; set; }

    public static explicit operator CobrancaPedidoViewModel(CobrancaPedidoEcommerce cobranca)
    {
        return new CobrancaPedidoViewModel
        {
            Id = cobranca.Id,
            Numero = cobranca.Numero,
            PedidoId = cobranca.PedidoId,
            Total = cobranca.Total,
            Status = cobranca.Status
        };
    }
}

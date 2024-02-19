using Domain.Pkg.Enum;

namespace OpenAdm.Application.Dtos.Pedidos;

public class UpdateStatusPedidoDto
{
    public Guid PedidoId { get; set; }
    public StatusPedido StatusPedido { get; set; }
}

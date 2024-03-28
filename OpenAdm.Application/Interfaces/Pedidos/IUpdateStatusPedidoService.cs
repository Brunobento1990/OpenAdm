using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Models.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IUpdateStatusPedidoService
{
    Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto);
}

using OpenAdm.Application.Dtos.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICancelarPedido
{
    Task<bool> CancelarAsync(CancelarPedidoDto cancelarPedidoDto);
}

using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class CancelarPedido : ICancelarPedido
{
    private readonly IPedidoRepository _pedidoRepository;

    public CancelarPedido(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<bool> CancelarAsync(CancelarPedidoDto cancelarPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(cancelarPedidoDto.PedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido");
        pedido.Cancelar(cancelarPedidoDto.Motivo);
        await _pedidoRepository.UpdateAsync(pedido);

        return true;
    }
}

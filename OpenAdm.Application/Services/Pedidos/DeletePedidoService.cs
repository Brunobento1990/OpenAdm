using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class DeletePedidoService : IDeletePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public DeletePedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível lozalizar o pedido");

        return await _pedidoRepository.DeleteAsync(pedido);
    }
}

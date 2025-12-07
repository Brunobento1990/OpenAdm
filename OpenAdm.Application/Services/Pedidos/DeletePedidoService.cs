using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class DeletePedidoService : IDeletePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IEstoqueService _estoqueService;

    public DeletePedidoService(IPedidoRepository pedidoRepository, IEstoqueService estoqueService)
    {
        _pedidoRepository = pedidoRepository;
        _estoqueService = estoqueService;
    }

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
                     ?? throw new ExceptionApi("Não foi possível lozalizar o pedido");

        await _estoqueService.MovimentacaoDePedidoCanceladoOuExcluidoAsync(pedido.ItensPedido);

        return await _pedidoRepository.DeleteAsync(pedido);
    }
}
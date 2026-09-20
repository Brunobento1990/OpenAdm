using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class DeletePedidoService : IDeletePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IFaturaRepository _faturaRepository;

    public DeletePedidoService(IPedidoRepository pedidoRepository, IFaturaRepository faturaRepository)
    {
        _pedidoRepository = pedidoRepository;
        _faturaRepository = faturaRepository;
    }

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
                     ?? throw new ExceptionApi("Não foi possível lozalizar o pedido");

        pedido.Excluir();
        if (pedido.Fatura is not null)
        {
            var historico = pedido.Fatura.Cancelar();
            await _faturaRepository.AddHistoricoAsync(historico);
        }
        
        return await _pedidoRepository.SaveChangesAsync() > 0;
    }
}

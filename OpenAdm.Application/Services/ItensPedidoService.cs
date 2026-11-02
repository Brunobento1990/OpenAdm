using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ItensPedidoService(IItensPedidoRepository itensPedidoRepository) : IItensPedidoService
{
    public async Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var itens = await itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId);
        return itens.Select(x => new ItensPedidoViewModel().ToModel(x)).ToList();
    }
}

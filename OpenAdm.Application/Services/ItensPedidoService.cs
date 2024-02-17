using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ItensPedidoService : IItensPedidoService
{
    private readonly IItensPedidoRepository _itensPedidoRepository;

    public ItensPedidoService(IItensPedidoRepository itensPedidoRepository)
    {
        _itensPedidoRepository = itensPedidoRepository;
    }

    public async Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var itens = await _itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId);

        return itens.Select(x => new ItensPedidoViewModel().ToModel(x)).ToList();
    }
}

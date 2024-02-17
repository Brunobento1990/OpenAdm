using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ItensPedidoCached(ItensPedidoRepository itensPedidoRepository, ICachedService<ItensPedido> cachedService) 
    : IItensPedidoRepository
{
    private readonly ItensPedidoRepository _itensPedidoRepository = itensPedidoRepository;
    private readonly ICachedService<ItensPedido> _cachedService = cachedService;

    public async Task<IList<ItensPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var key = pedidoId.ToString();
        var itensPedidos = await _cachedService.GetListItemAsync(key);

        if (itensPedidos == null)
        {
            itensPedidos = await _itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId);

            if (itensPedidos.Count > 0)
            {
                await _cachedService.SetListItemAsync(key, itensPedidos);
            }
        }

        return itensPedidos;
    }
}

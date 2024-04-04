using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;
using Domain.Pkg.Entities;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Cached.Cached;

public class ItensPedidoCached
    : GenericRepository<ItensPedido>, IItensPedidoRepository
{
    private readonly ItensPedidoRepository _itensPedidoRepository;
    private readonly ICachedService<ItensPedido> _cachedService;

    public ItensPedidoCached(
        ParceiroContext parceiroContext, 
        ItensPedidoRepository itensPedidoRepository, 
        ICachedService<ItensPedido> cachedService) : base(parceiroContext)
    {
        _itensPedidoRepository = itensPedidoRepository;
        _cachedService = cachedService;
    }

    public async Task<ItensPedido?> GetItemPedidoByIdAsync(Guid id)
    {
        var item = await _itensPedidoRepository.GetItemPedidoByIdAsync(id);
        
        if(item != null)
        {
            await _cachedService.RemoveCachedAsync(item.PedidoId.ToString());
        }
        
        return item;
    }

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

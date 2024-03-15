using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ItemTabelaDePrecoCached : IItemTabelaDePrecoRepository
{
    private readonly ICachedService<TabelaDePreco> _cachedService;
    private readonly ICachedService<ItensTabelaDePreco> _cachedServiceItem;
    private readonly ItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public ItemTabelaDePrecoCached(
        ICachedService<TabelaDePreco> cachedService, 
        ItemTabelaDePrecoRepository itemTabelaDePrecoRepository, 
        ICachedService<ItensTabelaDePreco> cachedServiceItem)
    {
        _cachedService = cachedService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _cachedServiceItem = cachedServiceItem;
    }

    public async Task<ItensTabelaDePreco> AddAsync(ItensTabelaDePreco entity)
    {
        var key = entity.TabelaDePrecoId.ToString();
        await _cachedService.RemoveCachedAsync(key);
        await _cachedServiceItem.RemoveCachedAsync(entity.Id.ToString());
        return await _itemTabelaDePrecoRepository.AddAsync(entity);
    }

    public async Task AddRangeAsync(IList<ItensTabelaDePreco> itensTabelaDePrecos)
    {
        await _itemTabelaDePrecoRepository.AddRangeAsync(itensTabelaDePrecos);
    }

    public async Task<bool> DeleteAsync(ItensTabelaDePreco entity)
    {
        var key = entity.TabelaDePrecoId.ToString();
        await _cachedService.RemoveCachedAsync(key);
        await _cachedServiceItem.RemoveCachedAsync(entity.Id.ToString());
        return await _itemTabelaDePrecoRepository.DeleteAsync(entity);
    }

    public async Task DeleteItensTabelaDePrecoByProdutoIdAsync(Guid produtoId)
    {
        await _itemTabelaDePrecoRepository.DeleteItensTabelaDePrecoByProdutoIdAsync(produtoId);
    }

    public async Task<ItensTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id)
    {
        var key = id.ToString();
        var item = await _cachedServiceItem.GetItemAsync(key);

        if(item == null)
        {
            item = await _itemTabelaDePrecoRepository.GetItemTabelaDePrecoByIdAsync(id);

            if(item != null)
            {
                await _cachedServiceItem.SetItemAsync(key, item);
            }
        }

        return item;
    }

    public async Task<IList<ItensTabelaDePreco>> GetItensTabelaDePrecoByIdProdutosAsync(IList<Guid> produtosIds)
    {
        return await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
    }

    public async Task<ItensTabelaDePreco> UpdateAsync(ItensTabelaDePreco entity)
    {
        var key = entity.TabelaDePrecoId.ToString();
        await _cachedService.RemoveCachedAsync(key);
        await _cachedServiceItem.RemoveCachedAsync(entity.Id.ToString());
        return await _itemTabelaDePrecoRepository.UpdateAsync(entity);
    }
}

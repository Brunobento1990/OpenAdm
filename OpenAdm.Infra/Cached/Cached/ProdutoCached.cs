using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.Cached.Cached;

public class ProdutoCached : IProdutoRepository
{
    private readonly ProdutoRepository _produtoRepository;
    private readonly ICachedService<Produto> _cachedService;
    private const string _keyListMaisVendidos = "produtos-mais-vendidos";
    private const string _keyList = "produtos";
    public ProdutoCached(ICachedService<Produto> cachedService, ProdutoRepository produtoRepository)
    {
        _cachedService = cachedService;
        _produtoRepository = produtoRepository;
    }

    public async Task<IList<Produto>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _cachedService.GetListItemAsync(_keyListMaisVendidos);

        if (produtosMaisVendidos == null)
        {
            produtosMaisVendidos = await _produtoRepository.GetProdutosMaisVendidosAsync();
            if (produtosMaisVendidos.Count > 0)
            {
                await _cachedService.SetListItemAsync(_keyListMaisVendidos, produtosMaisVendidos);
            }
        }

        return produtosMaisVendidos;
    }

    public async Task<PaginacaoViewModel<Produto>> GetProdutosAsync(int page)
    {
        var key = $"produtos-{page}";

        var produtos = await _cachedService.GetListItemAsync(key);
        var count = 0;

        if (produtos == null)
        {
            var paginacao = await _produtoRepository.GetProdutosAsync(page);
            if (paginacao.Values?.Count > 0)
            {
                produtos = paginacao.Values;
                count = paginacao.TotalPage;
                await _cachedService.SetListItemAsync(key, paginacao.Values);
            }

        }
        else
        {
            count = await _produtoRepository.GetTotalPageProdutosAsync();
        }


        return new PaginacaoViewModel<Produto>()
        {
            TotalPage = count,
            Values = produtos ?? new List<Produto>()
        };
    }

    public async Task<IList<Produto>> GetProdutosByCategoriaIdAsync(Guid categoriaId)
    {
        var key = $"produtos-por-categoria-{categoriaId}";
        var produtos = await _cachedService.GetListItemAsync(key);

        if (produtos == null)
        {
            produtos = await _produtoRepository.GetProdutosByCategoriaIdAsync(categoriaId);

            if (produtos != null)
            {
                await _cachedService.SetListItemAsync(key, produtos);
            }
        }

        return produtos ?? new List<Produto>();
    }

    public async Task<IList<Produto>> GetProdutosByListIdAsync(List<Guid> ids)
    {
        return await _produtoRepository.GetProdutosByListIdAsync(ids);
    }

    public async Task<Produto> AddAsync(Produto entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _produtoRepository.AddAsync(entity);
    }

    public async Task<Produto> UpdateAsync(Produto entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _produtoRepository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Produto entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _produtoRepository.DeleteAsync(entity);
    }

    public async Task<PaginacaoViewModel<Produto>> GetPaginacaoProdutoAsync(FilterModel<Produto> filterModel)
    {
        return await _produtoRepository.GetPaginacaoProdutoAsync(filterModel);
    }

    public async Task<Produto?> GetProdutoByIdAsync(Guid id)
    {
        var key = id.ToString();
        var produto = await _cachedService.GetItemAsync(key);

        if(produto == null)
        {
            produto = await _produtoRepository.GetProdutoByIdAsync(id);

            if(produto != null)
            {
                await _cachedService.SetItemAsync(key, produto);
            }
        }

        return produto;
    }
}

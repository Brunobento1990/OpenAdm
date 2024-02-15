using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ProdutoCached : GenericRepository<Produto>, IProdutoRepository
{
    private readonly ProdutoRepository _produtoRepository;
    private readonly ICachedService<Produto> _cachedService;
    private const string _keyListMaisVendidos = "produtos-mais-vendidos";
    public ProdutoCached(ParceiroContext parceiroContext, ICachedService<Produto> cachedService, ProdutoRepository produtoRepository) : base(parceiroContext)
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
            Values = produtos
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
}

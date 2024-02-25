using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto)
    {
        var produto = createProdutoDto.ToEntity();
        await _produtoRepository.AddAsync(produto);

        var pesosProdutos = createProdutoDto.ToPesosProdutos(produto.Id);
        var tamanhosProdutos = createProdutoDto.ToTamanhosProdutos(produto.Id);

        await _produtoRepository.AddRangePesosProdutosAsync(pesosProdutos);
        await _produtoRepository.AddRangeTamanhosProdutosAsync(tamanhosProdutos);

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task<PaginacaoViewModel<ProdutoViewModel>> GetPaginacaoAsync(PaginacaoProdutoDto paginacaoProdutoDto)
    {
        var paginacao = await _produtoRepository.GetPaginacaoProdutoAsync(paginacaoProdutoDto);

        return new PaginacaoViewModel<ProdutoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new ProdutoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<PaginacaoViewModel<ProdutoViewModel>> GetProdutosAsync(int page)
    {
        var paginacao = await _produtoRepository.GetProdutosAsync(page);

        return new PaginacaoViewModel<ProdutoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new ProdutoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<IList<ProdutoViewModel>> GetProdutosByCategoriaIdAsync(Guid categoriaId)
    {
        var produtos = await _produtoRepository.GetProdutosByCategoriaIdAsync(categoriaId);

        return produtos.Select(x => new ProdutoViewModel().ToModel(x)).ToList();
    }
}

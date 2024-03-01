using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using System.Text;

namespace OpenAdm.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesosProdutosRepository _pesosProdutosRepository;
    private readonly ITamanhosProdutoRepository _tamanhosProdutoRepository;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IPesosProdutosRepository pesosProdutosRepository,
        ITamanhosProdutoRepository tamanhosProdutoRepository)
    {
        _produtoRepository = produtoRepository;
        _pesosProdutosRepository = pesosProdutosRepository;
        _tamanhosProdutoRepository = tamanhosProdutoRepository;
    }

    public async Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto)
    {
        var produto = createProdutoDto.ToEntity();

        var pesosProdutos = createProdutoDto.ToPesosProdutos(produto.Id);
        var tamanhosProdutos = createProdutoDto.ToTamanhosProdutos(produto.Id);

        await _pesosProdutosRepository.AddRangeAsync(pesosProdutos);
        await _tamanhosProdutoRepository.AddRangeAsync(tamanhosProdutos);

        await _produtoRepository.AddAsync(produto);

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task DeleteProdutoAsync(Guid id)
    {
        var produto = await _produtoRepository.GetProdutoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var resultPesos = await _pesosProdutosRepository.DeleteRangeAsync(produto.Id);
        var resultTamanhos = await _tamanhosProdutoRepository.DeleteRangeAsync(produto.Id);

        if (resultPesos && resultTamanhos)
        {
            await _produtoRepository.DeleteAsync(produto);
        }
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

    public async Task<ProdutoViewModel> GetProdutoViewModelByIdAsync(Guid id)
    {
        var produto = await _produtoRepository.GetProdutoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task<ProdutoViewModel> UpdateProdutoAsync(UpdateProdutoDto updateProdutoDto)
    {
        var produto = await _produtoRepository.GetProdutoByIdAsync(updateProdutoDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
        produto.Update(
            updateProdutoDto.Descricao,
            updateProdutoDto.EspecificacaoTecnica,
            Encoding.UTF8.GetBytes(updateProdutoDto.Foto),
            updateProdutoDto.CategoriaId,
            updateProdutoDto.Referencia);

        var resultPesos = await _pesosProdutosRepository.DeleteRangeAsync(produto.Id);
        var resultTamanhos = await _tamanhosProdutoRepository.DeleteRangeAsync(produto.Id);

        if (resultPesos && resultTamanhos)
        {
            var tamanhosProdutos = updateProdutoDto.ToTamanhosProdutos();
            var pesosProdutos = updateProdutoDto.ToPesosProdutos();

            await _pesosProdutosRepository.AddRangeAsync(pesosProdutos);
            await _tamanhosProdutoRepository.AddRangeAsync(tamanhosProdutos);
            await _produtoRepository.UpdateAsync(produto);
        }

        produto.Categoria.Produtos = new();
        produto.Pesos.ForEach(x => x.Produtos = new());
        produto.Tamanhos.ForEach(x => x.Produtos = new());

        return new ProdutoViewModel().ToModel(produto);
    }
}

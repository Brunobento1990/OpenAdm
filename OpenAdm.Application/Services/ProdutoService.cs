using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Azure.Interfaces;
using OpenAdm.Infra.Paginacao;
using System.Text;

namespace OpenAdm.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesosProdutosRepository _pesosProdutosRepository;
    private readonly ITamanhosProdutoRepository _tamanhosProdutoRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IPesosProdutosRepository pesosProdutosRepository,
        ITamanhosProdutoRepository tamanhosProdutoRepository,
        IUploadImageBlobClient uploadImageBlobClient)
    {
        _produtoRepository = produtoRepository;
        _pesosProdutosRepository = pesosProdutosRepository;
        _tamanhosProdutoRepository = tamanhosProdutoRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
    }

    public async Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto)
    {
        var nomeFoto = $"{Guid.NewGuid()}.jpg";
        createProdutoDto.Foto = await _uploadImageBlobClient.UploadImageAsync(createProdutoDto.Foto, nomeFoto);
        var produto = createProdutoDto.ToEntity(nomeFoto);

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

        if (!string.IsNullOrWhiteSpace(produto.NomeFoto))
        {
            var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(produto.NomeFoto);

            if (!resultBlobDelete)
                throw new ExceptionApi();
        }

        var resultPesos = await _pesosProdutosRepository.DeleteRangeAsync(produto.Id);
        var resultTamanhos = await _tamanhosProdutoRepository.DeleteRangeAsync(produto.Id);

        if (resultPesos && resultTamanhos)
        {
            await _produtoRepository.DeleteAsync(produto);
        }
    }

    public async Task<IList<ProdutoViewModel>> GetAllProdutosAsync()
    {
        var produtos = await _produtoRepository.GetAllProdutosAsync();

        return produtos.Select(x => new ProdutoViewModel().ToModel(x)).ToList();
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

        var foto = produto.UrlFoto;
        var nomeFoto = produto.NomeFoto;

        if (!updateProdutoDto.Foto.StartsWith("https://") && !string.IsNullOrWhiteSpace(updateProdutoDto.Foto))
        {
            if (!string.IsNullOrWhiteSpace(produto.NomeFoto))
            {
                var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(produto.NomeFoto);
                if (!resultBlobDelete)
                    throw new ExceptionApi();
            }

            nomeFoto = $"{Guid.NewGuid()}.jpg";
            foto = await _uploadImageBlobClient.UploadImageAsync(updateProdutoDto.Foto, nomeFoto);
        }

        produto.Update(
            updateProdutoDto.Descricao,
            updateProdutoDto.EspecificacaoTecnica,
            updateProdutoDto.CategoriaId,
            updateProdutoDto.Referencia,
            foto,
            nomeFoto);

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

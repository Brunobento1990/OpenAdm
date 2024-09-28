using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Azure.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesosProdutosRepository _pesosProdutosRepository;
    private readonly ITamanhosProdutoRepository _tamanhosProdutoRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IPesosProdutosRepository pesosProdutosRepository,
        ITamanhosProdutoRepository tamanhosProdutoRepository,
        IUploadImageBlobClient uploadImageBlobClient,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository)
    {
        _produtoRepository = produtoRepository;
        _pesosProdutosRepository = pesosProdutosRepository;
        _tamanhosProdutoRepository = tamanhosProdutoRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
    }

    public async Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto)
    {
        var nomeFoto = $"{Guid.NewGuid()}.jpeg";
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
            ?? throw new ExceptionApi("Não foi possível localizar o produto");

        if (!string.IsNullOrWhiteSpace(produto.NomeFoto))
        {
            var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(produto.NomeFoto);

            if (!resultBlobDelete)
                throw new ExceptionApi("Não foi possível localizar a imagem do produto para fazer a exclusão");
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

    public async Task<PaginacaoViewModel<ProdutoViewModel>> GetProdutosAsync(PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto)
    {
        var paginacao = await _produtoRepository.GetProdutosAsync(paginacaoProdutoEcommerceDto);
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository
            .GetItensTabelaDePrecoByIdProdutosAsync(paginacao.Values.Select(x => x.Id).ToList());

        var produtosViewModel = new List<ProdutoViewModel>();

        foreach (var produto in paginacao.Values)
        {
            var produtoViewModel = new ProdutoViewModel().ToModel(produto);

            if(produtoViewModel.Tamanhos != null)
            {
                foreach (var tamanho in produtoViewModel.Tamanhos)
                {
                    var preco = itensTabelaDePreco.FirstOrDefault(
                        x => x.ProdutoId == produto.Id && x.TamanhoId == tamanho.Id);

                    if(preco != null)
                    {
                        tamanho.PrecoProdutoView = new PrecoProdutoViewModel()
                        {
                            ProdutoId = produto.Id,
                            TamanhoId = tamanho.Id,
                            ValorUnitarioAtacado = preco.ValorUnitarioAtacado,
                            ValorUnitarioVarejo = preco.ValorUnitarioVarejo
                        };
                    }
                }
            }

            if(produtoViewModel.Pesos != null)
            {
                foreach (var peso in produtoViewModel.Pesos)
                {
                    var preco = itensTabelaDePreco.FirstOrDefault(
                       x => x.ProdutoId == produto.Id && x.PesoId == peso.Id);

                    if (preco != null)
                    {
                        peso.PrecoProdutoView = new PrecoProdutoViewModel()
                        {
                            ProdutoId = produto.Id,
                            PesoId = peso.Id,
                            ValorUnitarioAtacado = preco.ValorUnitarioAtacado,
                            ValorUnitarioVarejo = preco.ValorUnitarioVarejo
                        };
                    }
                }
            }



            produtosViewModel.Add(produtoViewModel);
        }

        return new PaginacaoViewModel<ProdutoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = produtosViewModel
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
            ?? throw new ExceptionApi("Não foi possível localizar o produto");

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task<ProdutoViewModel> UpdateProdutoAsync(UpdateProdutoDto updateProdutoDto)
    {
        var produto = await _produtoRepository.GetProdutoByIdAsync(updateProdutoDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o produto");

        var foto = produto.UrlFoto;
        var nomeFoto = produto.NomeFoto;

        if (!updateProdutoDto.Foto.StartsWith("https://") && !string.IsNullOrWhiteSpace(updateProdutoDto.Foto))
        {
            if (!string.IsNullOrWhiteSpace(produto.NomeFoto))
            {
                var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(produto.NomeFoto);
                if (!resultBlobDelete)
                    throw new ExceptionApi("Não foi possível localizar a imagem do produto para fazer a exclusão");
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(updateProdutoDto.Foto, nomeFoto);
        }

        produto.Update(
            updateProdutoDto.Descricao,
            updateProdutoDto.EspecificacaoTecnica,
            updateProdutoDto.CategoriaId,
            updateProdutoDto.Referencia,
            foto,
            nomeFoto,
            updateProdutoDto.Peso);

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

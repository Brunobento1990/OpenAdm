using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesosProdutosRepository _pesosProdutosRepository;
    private readonly ITamanhosProdutoRepository _tamanhosProdutoRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IPesosProdutosRepository pesosProdutosRepository,
        ITamanhosProdutoRepository tamanhosProdutoRepository,
        IUploadImageBlobClient uploadImageBlobClient,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _produtoRepository = produtoRepository;
        _pesosProdutosRepository = pesosProdutosRepository;
        _tamanhosProdutoRepository = tamanhosProdutoRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto)
    {
        createProdutoDto.Validar();

        if (string.IsNullOrWhiteSpace(createProdutoDto.NovaFoto))
        {
            throw new ExceptionApi("Informe a foto do produto");
        }

        var nomeFoto = $"{Guid.NewGuid()}.jpeg";
        createProdutoDto.NovaFoto = await _uploadImageBlobClient.UploadImageAsync(createProdutoDto.NovaFoto, nomeFoto);
        var produto = createProdutoDto.ToEntity(nomeFoto);

        var pesosProdutos = createProdutoDto.ToPesosProdutos(produto.Id);
        var tamanhosProdutos = createProdutoDto.ToTamanhosProdutos(produto.Id);

        await _pesosProdutosRepository.AddRangeAsync(pesosProdutos);
        await _tamanhosProdutoRepository.AddRangeAsync(tamanhosProdutos);
        await _produtoRepository.AdicionarAsync(produto);
        await _produtoRepository.SaveChangesAsync();

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task DeleteProdutoAsync(Guid id)
    {

        var produto = await _produtoRepository.GetProdutoByIdExcluirAsync(id)
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

    public async Task<IEnumerable<ProdutoViewModel>> GetDropDownPaginacaoAsync(PaginacaoDropDown<Produto> paginacaoDropDown)
    {
        var produtos = await _produtoRepository.PaginacaoDropDownAsync(paginacaoDropDown);
        return produtos.Select(x => new ProdutoViewModel().ToModel(x));
    }

    public async Task<PaginacaoViewModel<ProdutoViewModel>> GetPaginacaoAsync(FilterModel<Produto> paginacaoProdutoDto)
    {
        var paginacao = await _produtoRepository.PaginacaoAsync(paginacaoProdutoDto);

        foreach (var produto in paginacao.Values)
        {
            if (produto.Categoria != null)
            {
                produto.Categoria.Produtos = [];
            }
        }

        return new PaginacaoViewModel<ProdutoViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new ProdutoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<PaginacaoViewModel<ProdutoViewModel>> GetProdutosAsync(PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto)
    {
        var paginacao = await _produtoRepository.GetProdutosAsync(paginacaoProdutoEcommerceDto);
        var produtosViewModel = await MapearProdutosAsync(paginacao.Values);

        return new PaginacaoViewModel<ProdutoViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = produtosViewModel
        };
    }

    public async Task<IList<ProdutoViewModel>> GetProdutosByCategoriaIdAsync(Guid categoriaId)
    {
        var produtos = await _produtoRepository.GetProdutosByCategoriaIdAsync(categoriaId);
        var produtosViewModel = await MapearProdutosAsync(produtos);

        return produtosViewModel;
    }

    public async Task<ProdutoViewModel> GetProdutoViewModelByIdAsync(Guid id)
    {
        var produto = await _produtoRepository.GetProdutoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o produto");

        return new ProdutoViewModel().ToModel(produto);
    }

    public async Task InativarAtivarEcommerceAsync(Guid id)
    {
        var produto = await _produtoRepository.GetProdutoByIdParaEditarAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o produto");
        produto.InativarAtivarEcommerce();

        await _produtoRepository.UpdateAsync(produto);
    }

    public async Task<ProdutoViewModel> UpdateProdutoAsync(UpdateProdutoDto updateProdutoDto)
    {
        updateProdutoDto.Validar();
        var produto = await _produtoRepository.GetProdutoByIdParaEditarAsync(updateProdutoDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o produto");

        var foto = produto.UrlFoto;
        var nomeFoto = produto.NomeFoto;

        if (!string.IsNullOrWhiteSpace(updateProdutoDto.NovaFoto))
        {
            if (!string.IsNullOrWhiteSpace(produto.NomeFoto))
            {
                var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(produto.NomeFoto);
                if (!resultBlobDelete)
                    throw new ExceptionApi("Não foi possível localizar a imagem do produto para fazer a exclusão");
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(updateProdutoDto.NovaFoto, nomeFoto);
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
            await _itemTabelaDePrecoRepository.DeleteItensTabelaDePrecoByProdutoIdAsync(produto.Id);

            await _pesosProdutosRepository.AddRangeAsync(pesosProdutos);
            await _tamanhosProdutoRepository.AddRangeAsync(tamanhosProdutos);

            var itens = updateProdutoDto.ObterItensTabelaDePrecoEdit() ?? [];

            _produtoRepository.Update(produto);
            await _itemTabelaDePrecoRepository.AddRangeAsync(itens);
        }

        if (produto.Categoria != null)
        {
            produto.Categoria.Produtos = [];
        }
        produto.Pesos.ForEach(x => x.Produtos = []);
        produto.Tamanhos.ForEach(x => x.Produtos = []);

        return new ProdutoViewModel().ToModel(produto);
    }

    private async Task<IList<ProdutoViewModel>> MapearProdutosAsync(IList<Produto> produtos)
    {
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository
            .GetItensTabelaDePrecoByIdProdutosAsync(produtos.Select(x => x.Id).ToList());
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoOrNullAsync();
        var isAtacado = usuario?.IsAtacado == true;
        var produtosViewModel = new List<ProdutoViewModel>();

        var precoPorTamanho = itensTabelaDePreco
        .Where(x => x.TamanhoId != null)
        .ToDictionary(
            x => (x.ProdutoId, x.TamanhoId),
            x => x
        );

        var precoPorPeso = itensTabelaDePreco
            .Where(x => x.PesoId != null)
            .ToDictionary(
                x => (x.ProdutoId, x.PesoId),
                x => x
            );

        foreach (var produto in produtos)
        {
            var produtoViewModel = new ProdutoViewModel().ToViewModel(produto);

            if (produtoViewModel.Tamanhos != null)
            {
                foreach (var tamanho in produtoViewModel.Tamanhos)
                {
                    if (precoPorTamanho.TryGetValue((produto.Id, tamanho.Id), out var preco))
                    {
                        tamanho.PrecoProduto = new PrecoProdutoViewModel()
                        {
                            ProdutoId = produto.Id,
                            TamanhoId = tamanho.Id,
                            ValorUnitario = isAtacado ?
                                preco.ValorUnitarioAtacado : preco.ValorUnitarioVarejo
                        };
                    }
                }
            }

            if (produtoViewModel.Pesos != null)
            {
                foreach (var peso in produtoViewModel.Pesos)
                {
                    if (precoPorPeso.TryGetValue((produto.Id, peso.Id), out var preco))
                    {
                        peso.PrecoProduto = new PrecoProdutoViewModel()
                        {
                            ProdutoId = produto.Id,
                            PesoId = peso.Id,
                            ValorUnitario = isAtacado ?
                                preco.ValorUnitarioAtacado : preco.ValorUnitarioVarejo
                        };
                    }
                }
            }
            produtosViewModel.Add(produtoViewModel);
        }

        return produtosViewModel;
    }
}

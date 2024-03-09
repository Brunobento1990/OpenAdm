using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Services;

public class CarrinhoService : ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ITokenService _tokenService;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public CarrinhoService(
        ICarrinhoRepository carrinhoRepository,
        IProdutoRepository produtoRepository,
        ITokenService tokenService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _tokenService = tokenService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
    }

    public async Task<bool> AdicionarProdutoAsync(IList<AddCarrinhoModel> addCarrinhoModel)
    {
        var _key = _tokenService.GetTokenUsuarioViewModel().Id.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = Guid.Parse(_key);

        foreach (var item in addCarrinhoModel)
        {
            var addProduto = carrinho?
            .Produtos
            .FirstOrDefault(x =>
                x.ProdutoId == item.ProdutoId &&
                x.PesoId == item.PesoId &&
                x.TamanhoId == item.TamanhoId);

            if (addProduto == null)
            {
                addProduto = new()
                {
                    ProdutoId = item.ProdutoId,
                    TamanhoId = item.TamanhoId,
                    PesoId = item.PesoId,
                    Quantidade = item.Quantidade
                };

                carrinho?.Produtos.Add(addProduto);
            }
            else
            {
                addProduto.Quantidade += item.Quantidade;
            }
        }

        await _carrinhoRepository.AdicionarProdutoAsync(carrinho, _key);

        return true;
    }

    public async Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId)
    {
        var _key = _tokenService.GetTokenUsuarioViewModel().Id.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = Guid.Parse(_key);

        var produto = carrinho.Produtos.FirstOrDefault(x => x.ProdutoId == produtoId);

        if (produto != null)
        {
            carrinho.Produtos.Remove(produto);
            await _carrinhoRepository.AdicionarProdutoAsync(carrinho, _key);
        }

        return true;
    }

    public async Task<IList<CarrinhoViewModel>> GetCarrinhoAsync()
    {
        var _key = _tokenService.GetTokenUsuarioViewModel().Id.ToString();

        var carrinhosViewModels = new List<CarrinhoViewModel>();

        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        var produtosIds = carrinho.Produtos.Select(x => x.ProdutoId).ToList();

        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

        foreach (var produto in produtos)
        {
            var carrinhoViewModel = new CarrinhoViewModel()
            {
                Categoria = new CategoriaViewModel().ToModel(produto.Categoria),
                CategoriaId = produto.CategoriaId,
                Descricao = produto.Descricao,
                EspecificacaoTecnica = produto.EspecificacaoTecnica,
                Id = produto.Id,
                Referencia = produto.Referencia,
                Numero = produto.Numero,
                Foto = produto.UrlFoto ?? ""
            };

            carrinhoViewModel.Tamanhos = produto.Tamanhos.OrderBy(x => x.Numero).Select(x => new TamanhoCarrinhoViewModel()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Numero = x.Numero,
                PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                {
                    Quantidade = (decimal)(carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.TamanhoId == x.Id)?
                                .Quantidade ?? 0),

                    ValorUnitarioVarejo = itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.TamanhoId == x.Id)?.ValorUnitarioVarejo,
                    ValorUnitarioAtacado = itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.TamanhoId == x.Id)?.ValorUnitarioAtacado
                }
            }).ToList();

            carrinhoViewModel.Pesos = produto.Pesos.OrderBy(x => x.Numero).Select(x => new PesoCarrinhoViewModel()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Numero = x.Numero,
                PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                {
                    Quantidade = (decimal)(carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.PesoId == x.Id)?
                                .Quantidade ?? 0),
                    ValorUnitarioAtacado = itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.PesoId == x.Id)?.ValorUnitarioAtacado,
                    ValorUnitarioVarejo = itensTabelaDePreco
                        .FirstOrDefault(item => item.PesoId == x.Id && item.ProdutoId == produto.Id)?.ValorUnitarioVarejo
                }
            }).ToList();

            carrinhosViewModels.Add(carrinhoViewModel);
        }

        return carrinhosViewModels;
    }

    public async Task<int> GetCountCarrinhoAsync()
    {
        var _key = _tokenService.GetTokenUsuarioViewModel().Id.ToString();
        return await _carrinhoRepository.GetCountCarrinhoAsync(_key);
    }
    public async Task LimparCarrinhoDoUsuarioAsync(Guid usuarioId)
    {
        await _carrinhoRepository.DeleteCarrinhoAsync(usuarioId.ToString());
    }
}

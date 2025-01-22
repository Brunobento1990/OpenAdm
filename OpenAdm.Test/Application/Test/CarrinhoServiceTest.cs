using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Application.Services.Carrinhos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using OpenAdm.Test.Domain.Builder;
using OpenAdm.Test.Memory;

namespace OpenAdm.Test.Application.Test;

public class CarrinhoServiceTest
{
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticado;
    private readonly Mock<IProdutoRepository> _produtoRepository;
    private readonly Mock<IItemTabelaDePrecoRepository> _itemTabelaDePrecoRepository;
    private readonly CarrinhoRepositoryMemory _carrinhoRepository;
    private readonly AddCarrinhoService _addCarrinhoService;
    private readonly GetCarrinhoService _getCarrinhoService;
    private readonly GetCountCarrinhoService _getCountCarrinhoService;

    public CarrinhoServiceTest()
    {
        _usuarioAutenticado = new();
        _produtoRepository = new();
        _itemTabelaDePrecoRepository = new();
        _carrinhoRepository = new CarrinhoRepositoryMemory();
        _addCarrinhoService = new AddCarrinhoService(_carrinhoRepository, _usuarioAutenticado.Object);
        _getCarrinhoService = new GetCarrinhoService(
            _carrinhoRepository,
            _produtoRepository.Object,
            _itemTabelaDePrecoRepository.Object,
            _usuarioAutenticado.Object);
        _getCountCarrinhoService = new GetCountCarrinhoService(_carrinhoRepository, _usuarioAutenticado.Object);
    }

    [Fact]
    public async Task DeveAdicionarItensERetornarCountCorretamente()
    {
        var usuario = UsuarioBuilder.Init().Build();
        _usuarioAutenticado.Setup(x => x.GetUsuarioAutenticadoAsync()).ReturnsAsync(usuario);

        var produto1 = new AddCarrinhoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 10
        };
        var produto2 = new AddCarrinhoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 364
        };
        var produto3 = new AddCarrinhoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 147
        };

        var result1 = await _addCarrinhoService.AddCarrinhoAsync(new List<AddCarrinhoModel>() { produto1 });
        var result2 = await _addCarrinhoService.AddCarrinhoAsync(new List<AddCarrinhoModel>() { produto2 });
        var result3 = await _addCarrinhoService.AddCarrinhoAsync(new List<AddCarrinhoModel>() { produto3 });

        var count = await _getCountCarrinhoService.GetCountCarrinhoAsync();
        Assert.Equal(1, result1);
        Assert.Equal(2, result2);
        Assert.Equal(3, result3);
        Assert.Equal(3, count);
    }

    //[Fact]
    //public async Task DeveRetornarProdutosDoCarrinhoCorretos()
    //{
    //    var quantidade = 12;
    //    var categoria = CategoriaBuilder.Init().Build();
    //    var carrinhoRepository = new CarrinhoRepositoryMemory();
    //    var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho", 0);
    //    var produto = ProdutoBuilder.Init().Build();
    //    produto.Categoria = categoria;
    //    produto.Tamanhos.Add(tamanho);
    //    var itemTabelaDePreco = new ItemTabelaDePreco(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, produto.Id, 1, 2, Guid.NewGuid(), tamanho.Id, null);
    //    var usuario = UsuarioBuilder.Init().Build();
    //    var carrinho = new CarrinhoModel()
    //    {
    //        UsuarioId = usuario.Id,
    //        Produtos = new()
    //        {
    //            new AddCarrinhoModel()
    //            {
    //                ProdutoId = produto.Id,
    //                TamanhoId = tamanho.Id,
    //                Quantidade = quantidade
    //            }
    //        }
    //    };

    //    await carrinhoRepository.AdicionarProdutoAsync(carrinho, usuario.Id.ToString());

    //    var produtoRepository = new Mock<IProdutoRepository>();
    //    var itemTabelaDePrecoRepository = new Mock<IItemTabelaDePrecoRepository>();
    //    produtoRepository.Setup(x => x.GetProdutosByListIdAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<Produto>() { produto });
    //    itemTabelaDePrecoRepository.Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<ItemTabelaDePreco>() { itemTabelaDePreco });
    //    var carrinhoService = new GetCarrinhoService(carrinhoRepository, produtoRepository.Object, itemTabelaDePrecoRepository.Object);
    //    var carrinhoReturn = await carrinhoService.GetCarrinhoAsync(new UsuarioViewModel().ToModel(usuario));

    //    var produtoCarrinho = carrinhoReturn.FirstOrDefault(x => x.Id == produto.Id);

    //    Assert.NotNull(produtoCarrinho);
    //    Assert.Equal(produto.Id, produtoCarrinho.Id);
    //    Assert.Equal(quantidade, produtoCarrinho.Tamanhos?.FirstOrDefault(x => x.Id == tamanho.Id)?.PrecoProduto.Quantidade);
    //}

    //[Fact]
    //public async Task DeveAdicionarProdutoNoCarrinho()
    //{
    //    var usuario = UsuarioBuilder.Init().Build();
    //    var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho", 0);
    //    var produto = ProdutoBuilder.Init().Build();
    //    var addCarrinho = new List<AddCarrinhoModel>()
    //    {
    //        new()
    //        {
    //            TamanhoId = tamanho.Id,
    //            Quantidade = 3,
    //            ProdutoId = produto.Id
    //        }
    //    };
    //    var carrinho = new CarrinhoModel()
    //    {
    //        UsuarioId = usuario.Id,
    //        Produtos = addCarrinho
    //    };

    //    var carrinhoRepository = new CarrinhoRepositoryMemory();
    //    var carrinhoService = new AddCarrinhoService(carrinhoRepository);
    //    var result = await carrinhoService.AddCarrinhoAsync(addCarrinho, new UsuarioViewModel().ToModel(usuario));

    //    var carrinhoCount = new GetCountCarrinhoService(carrinhoRepository);
    //    var count = await carrinhoCount.GetCountCarrinhoAsync(usuario.Id);

    //    Assert.Equal(carrinho.Produtos.Count, count);
    //    Assert.True(result);
    //}

    //[Fact]
    //public async Task DeveDeletarProdutoDoCarrinho()
    //{
    //    var usuario = UsuarioBuilder.Init().Build();
    //    var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho", 0);
    //    var produto = ProdutoBuilder.Init().Build();
    //    var addCarrinho = new List<AddCarrinhoModel>()
    //    {
    //        new()
    //        {
    //            TamanhoId = tamanho.Id,
    //            Quantidade = 3,
    //            ProdutoId = produto.Id
    //        }
    //    };

    //    var carrinhoRepository = new CarrinhoRepositoryMemory();
    //    var carrinhoService = new AddCarrinhoService(carrinhoRepository);
    //    var result = await carrinhoService.AddCarrinhoAsync(addCarrinho, new UsuarioViewModel().ToModel(usuario));

    //    var deleteCarrinho = new DeleteProdutoCarrinhoService(carrinhoRepository);
    //    var resultDelete = await deleteCarrinho.DeleteProdutoCarrinhoAsync(produto.Id, usuario.Id);

    //    Assert.True(resultDelete);
    //    Assert.True(result);
    //}
}

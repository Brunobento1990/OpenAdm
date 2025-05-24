using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;
using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;

namespace OpenAdm.Test.Application.Test;

public class PedidoServiceTest
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProcessarPedidoService> _processarPedidoServiceMock;
    private readonly Mock<IItemTabelaDePrecoRepository> _itemTabelaDePrecoRepositoryMock;
    private readonly Mock<IPdfPedidoService> _pdfPedidoService;
    private readonly Mock<ICarrinhoRepository> _carrinhoRepository;
    private readonly Mock<IFaturaService> _contasAReceberService;
    private readonly Mock<IConfiguracaoDePagamentoService> _configuracaoDePagamentoService;
    private readonly Mock<IConfiguracoesDePedidoService> _configuracoesDePedidoService;
    public PedidoServiceTest()
    {
        _pedidoRepositoryMock = new();
        _processarPedidoServiceMock = new();
        _itemTabelaDePrecoRepositoryMock = new();
        _pdfPedidoService = new();
        _carrinhoRepository = new();
        _contasAReceberService = new();
        _configuracaoDePagamentoService = new();
        _configuracoesDePedidoService = new();
    }

    //[Fact]
    //public async Task NaoDeveAlterarStatusPedidoJaEntregue()
    //{
    //    var builder = PedidoBuilder.Init();
    //    var pedido = builder.ComStatusPedido(StatusPedido.Entregue).Build();
    //    var pedidoUpdateStatus = builder.BuildStatusPedidoDto();

    //    _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

    //    var pedidoService = new UpdateStatusPedidoService(_pedidoRepositoryMock.Object, _processarPedidoServiceMock.Object);

    //    await Assert
    //        .ThrowsAnyAsync<ExceptionApi>(
    //            async () => await pedidoService.UpdateStatusPedidoAsync(pedidoUpdateStatus));
    //}

    //[Fact]
    //public async Task DeveAlterarStatusPedido()
    //{
    //    var builder = PedidoBuilder.Init();
    //    var pedido = builder.Build();
    //    var pedidoUpdateStatus = builder.BuildStatusPedidoDto();

    //    _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

    //    var pedidoService = new UpdateStatusPedidoService(_pedidoRepositoryMock.Object, _processarPedidoServiceMock.Object);
    //    var pedidoViewModel = await pedidoService.UpdateStatusPedidoAsync(pedidoUpdateStatus);

    //    Assert.NotNull(pedidoViewModel);
    //    Assert.IsType<PedidoViewModel>(pedidoViewModel);
    //    Assert.Equal(pedido.Id, pedidoViewModel.Id);
    //    Assert.Equal(pedido.StatusPedido, pedidoViewModel.StatusPedido);
    //}

    //[Fact]
    //public async Task DeveEfetuarDownloadBase64DoPedido()
    //{
    //    var pedido = PedidoBuilder.Init().Build();
    //    var configuracoesDePedidoRepository = new Mock<IConfiguracoesDePedidoRepository>();
    //    _pedidoRepositoryMock.Setup(x => x.GetPedidoCompletoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
    //    var pedidoService = new PedidoDownloadService(
    //        _pedidoRepositoryMock.Object,
    //        configuracoesDePedidoRepository.Object,
    //        _pdfPedidoService.Object);
    //    var pdf = await pedidoService.DownloadPedidoPdfAsync(pedido.Id);

    //    Assert.NotNull(pdf);
    //}

    [Fact]
    public async Task DeveExcluirPedido()
    {
        var pedido = PedidoBuilder.Init().Build();

        _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        _pedidoRepositoryMock.Setup(x => x.DeleteAsync(pedido)).ReturnsAsync(true);

        var pedidoService = new DeletePedidoService(_pedidoRepositoryMock.Object);
        var result = await pedidoService.DeletePedidoAsync(pedido.Id);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task DeveCriarPedido()
    {
        var primeiroProduto = new ItemPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 1
        };
        var segundoProduto = new ItemPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 2,
            ValorUnitario = 1
        };

        var itensPedidoModel = new List<ItemPedidoModel>()
        {
           primeiroProduto,
           segundoProduto
        };

        var itensTabelaDePreco = new List<ItemTabelaDePreco>()
        {
            new (Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                1,
                primeiroProduto.ProdutoId,
                1,
                2,
                Guid.NewGuid(),
                primeiroProduto.TamanhoId,
                primeiroProduto.PesoId),
             new (Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                1,
                segundoProduto.ProdutoId,
                1,
                2,
                Guid.NewGuid(),
                segundoProduto.TamanhoId,
                segundoProduto.PesoId),

        };

        var produtosIds = itensTabelaDePreco.Select(x => x.ProdutoId).ToList();

        _itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);

        var efetuarCobranca = new EfetuarCobrancaViewModel()
        {
            Cobrar = false
        };
        _configuracaoDePagamentoService.Setup(x => x.CobrarAsync()).ReturnsAsync(efetuarCobranca);
        var pedidoMinimo = new PedidoMinimoViewModel();
        _configuracoesDePedidoService.Setup(x => x.GetPedidoMinimoAsync()).ReturnsAsync(pedidoMinimo);

        var service = new CreatePedidoService(
            _pedidoRepositoryMock.Object,
            _processarPedidoServiceMock.Object,
            _itemTabelaDePrecoRepositoryMock.Object,
            _carrinhoRepository.Object,
            _contasAReceberService.Object, _configuracaoDePagamentoService.Object, _configuracoesDePedidoService.Object);

        var usuario = UsuarioBuilder.Init().Build();
        var pedidoModel = await service.CreatePedidoAsync(itensPedidoModel, usuario);

        Assert.NotNull(pedidoModel);
        Assert.Equal(StatusPedido.Aberto, pedidoModel.StatusPedido);
    }

    [Fact]
    public async Task NaoDeveCriarPedidoSemItens()
    {
        var itensPedido = new List<ItemPedidoModel>();

        var itensTabelaDePreco = new List<ItemTabelaDePreco>()
        {
            new (Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                1,
                Guid.NewGuid(),
                1,
                2,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid())
        };

        var produtosIds = itensPedido.Select(x => x.ProdutoId).ToList();

        _itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);
        var efetuarCobranca = new EfetuarCobrancaViewModel()
        {
            Cobrar = false
        };
        _configuracaoDePagamentoService.Setup(x => x.CobrarAsync()).ReturnsAsync(efetuarCobranca);
        var service = new CreatePedidoService(
            _pedidoRepositoryMock.Object,
            _processarPedidoServiceMock.Object,
            _itemTabelaDePrecoRepositoryMock.Object,
            _carrinhoRepository.Object,
            _contasAReceberService.Object,
            _configuracaoDePagamentoService.Object, _configuracoesDePedidoService.Object);

        var usuario = UsuarioBuilder.Init().Build();

        await Assert.ThrowsAsync<ExceptionApi>(async () => await service.CreatePedidoAsync(itensPedido, usuario));
    }

    [Fact]
    public async Task NaoDeveCriarPedidoDeUsuarioJuridicoComValorUnitarioVarejo()
    {
        var item = new ItemPedidoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 5
        };

        var itens = new List<ItemPedidoModel>()
        {
            item
        };

        var usuarioViewModel = UsuarioBuilder.Init().Build();

        var itensTabelaDePreco = new List<ItemTabelaDePreco>()
        {
            new (Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                1,
                item.ProdutoId,
                1,
                2,
                Guid.NewGuid(),
                item.TamanhoId,
                item.PesoId)
        };

        var produtosIds = itens.Select(x => x.ProdutoId).ToList();

        _itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);
        var efetuarCobranca = new EfetuarCobrancaViewModel()
        {
            Cobrar = false
        };
        _configuracaoDePagamentoService.Setup(x => x.CobrarAsync()).ReturnsAsync(efetuarCobranca);
        var pedidoMinimo = new PedidoMinimoViewModel();
        _configuracoesDePedidoService.Setup(x => x.GetPedidoMinimoAsync()).ReturnsAsync(pedidoMinimo);
        var service = new CreatePedidoService(
            _pedidoRepositoryMock.Object,
            _processarPedidoServiceMock.Object,
            _itemTabelaDePrecoRepositoryMock.Object,
            _carrinhoRepository.Object, _contasAReceberService.Object,
            _configuracaoDePagamentoService.Object, _configuracoesDePedidoService.Object);

        await Assert.ThrowsAsync<Exception>(async () => await service.CreatePedidoAsync(itens, usuarioViewModel));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveCriarPedidoDeUsuarioSemTelefone(string? telefone)
    {
        var item = new ItemPedidoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 5
        };

        var itens = new List<ItemPedidoModel>()
        {
            item
        };

        var usuarioViewModel = UsuarioBuilder.Init().SemTelefone(telefone).Build();

        var itensTabelaDePreco = new List<ItemTabelaDePreco>()
        {
            new (Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                1,
                item.ProdutoId,
                1,
                2,
                Guid.NewGuid(),
                item.TamanhoId,
                item.PesoId)
        };

        var produtosIds = itens.Select(x => x.ProdutoId).ToList();

        _itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);
        var efetuarCobranca = new EfetuarCobrancaViewModel()
        {
            Cobrar = false
        };
        _configuracaoDePagamentoService.Setup(x => x.CobrarAsync()).ReturnsAsync(efetuarCobranca);
        var pedidoMinimo = new PedidoMinimoViewModel();
        _configuracoesDePedidoService.Setup(x => x.GetPedidoMinimoAsync()).ReturnsAsync(pedidoMinimo);
        var service = new CreatePedidoService(
            _pedidoRepositoryMock.Object,
            _processarPedidoServiceMock.Object,
            _itemTabelaDePrecoRepositoryMock.Object,
            _carrinhoRepository.Object, _contasAReceberService.Object,
            _configuracaoDePagamentoService.Object, _configuracoesDePedidoService.Object);

        var exception = await Assert.ThrowsAsync<ExceptionApi>(async () => await service.CreatePedidoAsync(itens, usuarioViewModel));

        Assert.NotNull(exception);
        Assert.IsType<ExceptionApi>(exception);
        Assert.Equal("Seu cadastro esta incompleto, acesse sua conta e cadastre seu telefone!", exception.Message);
    }
}

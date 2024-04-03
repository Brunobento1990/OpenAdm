using OpenAdm.Application.Interfaces;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;
using Domain.Pkg.Enum;
using OpenAdm.Application.Models.Pedidos;
using Domain.Pkg.Model;
using Domain.Pkg.Entities;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Application.Services.Pedidos;
using Domain.Pkg.Pdfs.Configure;

namespace OpenAdm.Test.Application.Test;

public class PedidoServiceTest
{
    [Fact]
    public async Task NaoDeveAlterarStatusPedidoJaEntregue()
    {
        var builder = PedidoBuilder.Init();
        var pedido = builder.ComStatusPedido(StatusPedido.Entregue).Build();
        var pedidoUpdateStatus = builder.BuildStatusPedidoDto();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        var itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new UpdateStatusPedidoService(pedidoRepositoryMock.Object, processarPedidoServiceMock.Object);

        await Assert
            .ThrowsAnyAsync<ExceptionApi>(
                async () => await pedidoService.UpdateStatusPedidoAsync(pedidoUpdateStatus));
    }

    [Fact]
    public async Task DeveAlterarStatusPedido()
    {
        var builder = PedidoBuilder.Init();
        var pedido = builder.Build();
        var pedidoUpdateStatus = builder.BuildStatusPedidoDto();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        var itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new UpdateStatusPedidoService(pedidoRepositoryMock.Object, processarPedidoServiceMock.Object);
        var pedidoViewModel = await pedidoService.UpdateStatusPedidoAsync(pedidoUpdateStatus);

        Assert.NotNull(pedidoViewModel);
        Assert.IsType<PedidoViewModel>(pedidoViewModel);
        Assert.Equal(pedido.Id, pedidoViewModel.Id);
        Assert.Equal(pedido.StatusPedido, pedidoViewModel.StatusPedido);
    }

    //[Fact]
    //public async Task DeveEfetuarDownloadBase64DoPedido()
    //{
    //    var pedido = PedidoBuilder.Init().Build();
    //    ConfigurePdfQuest.ConfigureQuest();
    //    var pedidoRepositoryMock = new Mock<IPedidoRepository>();
    //    var configuracoesDePedidoRepository = new Mock<IConfiguracoesDePedidoRepository>();

    //    pedidoRepositoryMock.Setup(x => x.GetPedidoCompletoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

    //    var pedidoService = new PedidoDownloadService(pedidoRepositoryMock.Object, configuracoesDePedidoRepository.Object);
    //    var pdf = await pedidoService.DownloadPedidoPdfAsync(pedido.Id);

    //    Assert.NotNull(pdf);
    //}

    [Fact]
    public async Task DeveExcluirPedido()
    {
        var pedido = PedidoBuilder.Init().Build();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        pedidoRepositoryMock.Setup(x => x.DeleteAsync(pedido)).ReturnsAsync(true);

        var pedidoService = new DeletePedidoService(pedidoRepositoryMock.Object);
        var result = await pedidoService.DeletePedidoAsync(pedido.Id);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task DeveCriarPedido()
    {
        var primeiroProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 1
        };
        var segundoProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 2,
            ValorUnitario = 1
        };

        var itensPedidoModel = new List<ItensPedidoModel>()
        {
           primeiroProduto,
           segundoProduto
        };

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        var itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();

        var itensTabelaDePreco = new List<ItensTabelaDePreco>()
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

        itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);

        var service = new CreatePedidoService(
            pedidoRepositoryMock.Object,
            processarPedidoServiceMock.Object,
            itemTabelaDePrecoRepositoryMock.Object);

        var usuario = new UsuarioViewModel()
        {
            Cpf = "123",
            DataDeAtualizacao = DateTime.Now,
            DataDeCriacao = DateTime.Now,
            Id = Guid.NewGuid()
        };
        var pedidoModel = await service.CreatePedidoAsync(itensPedidoModel, usuario);

        Assert.NotNull(pedidoModel);
        Assert.Equal(StatusPedido.Aberto, pedidoModel.StatusPedido);
    }

    [Fact]
    public async Task NaoDeveCriarPedidoSemItens()
    {
        var itensPedido = new List<ItensPedidoModel>();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        var itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();

        var itensTabelaDePreco = new List<ItensTabelaDePreco>()
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

        itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);

        var service = new CreatePedidoService(
            pedidoRepositoryMock.Object,
            processarPedidoServiceMock.Object,
            itemTabelaDePrecoRepositoryMock.Object);

        var usuario = new UsuarioViewModel()
        {
            Cnpj = "123",
            Cpf = "123",
            DataDeAtualizacao = DateTime.Now,
            DataDeCriacao = DateTime.Now,
            Id = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<ExceptionApi>(async () => await service.CreatePedidoAsync(itensPedido, usuario));
    }

    [Fact]
    public async Task NaoDeveCriarPedidoDeUsuarioJuridicoComValorUnitarioVarejo()
    {
        var item = new ItensPedidoModel()
        {
            PesoId = Guid.NewGuid(),
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 5
        };

        var itens = new List<ItensPedidoModel>()
        {
            item
        };

        var usuarioViewModel = new UsuarioViewModel()
        {
            Cnpj = "6165165165",
            DataDeAtualizacao = DateTime.Now,
            DataDeCriacao = DateTime.Now,
            Email = "teste@gmail.com",
            Id = Guid.NewGuid(),
            Nome = "Teste jurídico",
            Numero = 5
        };

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();
        var itemTabelaDePrecoRepositoryMock = new Mock<IItemTabelaDePrecoRepository>();

        var itensTabelaDePreco = new List<ItensTabelaDePreco>()
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

        itemTabelaDePrecoRepositoryMock.Setup((x) =>
            x.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds))
            .ReturnsAsync(itensTabelaDePreco);

        var service = new CreatePedidoService(
            pedidoRepositoryMock.Object,
            processarPedidoServiceMock.Object,
            itemTabelaDePrecoRepositoryMock.Object);

        await Assert.ThrowsAsync<ExceptionApi>(async () => await service.CreatePedidoAsync(itens, usuarioViewModel));
    }
}

using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;
using Domain.Pkg.Enum;
using OpenAdm.Application.Models.Pedidos;
using QuestPDF.Infrastructure;
using OpenAdm.Application.Dtos.Pedidos;
using Domain.Pkg.Model;
using Domain.Pkg.Entities;

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
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);

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
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);
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
    //    QuestPDF.Settings.License = LicenseType.Community;
    //    var pedidoRepositoryMock = new Mock<IPedidoRepository>();
    //    var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
    //    var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

    //    pedidoRepositoryMock.Setup(x => x.GetPedidoCompletoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

    //    var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);
    //    var pdf = await pedidoService.DownloadPedidoPdfAsync(pedido.Id);

    //    Assert.NotNull(pdf);
    //}

    [Fact]
    public async Task DeveExcluirPedido()
    {
        var pedido = PedidoBuilder.Init().Build();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        pedidoRepositoryMock.Setup(x => x.DeleteAsync(pedido)).ReturnsAsync(true);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);
        var result = await pedidoService.DeletePedidoAsync(pedido.Id);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task DeveCriarPedido()
    {
        var pedidoPorPesoModel = new PedidoPorPesoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var pedidoPorTamanhoModel = new PedidoPorTamanhoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var pedidoCreateDto = new PedidoCreateDto()
        {
            PedidosPorPeso = new List<PedidoPorPesoModel>() { pedidoPorPesoModel },
            PedidosPorTamanho = new List<PedidoPorTamanhoModel>() { pedidoPorTamanhoModel }
        };

        var tabelaDePreco = TabelaDePrecoBuilder.Init().Build();

        tabelaDePreco.ItensTabelaDePreco.AddRange(new List<ItensTabelaDePreco>()
        {
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorPesoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                null,
                pedidoPorPesoModel.PesoId),
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorTamanhoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                pedidoPorTamanhoModel.TamanhoId,
                null)
        });

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        tabelaDePrecoRepositoryMock.Setup((tb) => tb.GetTabelaDePrecoAtivaAsync()).ReturnsAsync(tabelaDePreco);

        var service = new PedidoService(
            pedidoRepositoryMock.Object,
            tabelaDePrecoRepositoryMock.Object,
            processarPedidoServiceMock.Object);

        var usuarioId = Guid.NewGuid();
        var pedidoModel = await service.CreatePedidoAsync(pedidoCreateDto, usuarioId);

        Assert.NotNull(pedidoModel);
        Assert.Equal(StatusPedido.Aberto, pedidoModel.StatusPedido);
    }

    [Fact]
    public async Task NaoDeveCriarPedidoSemItens()
    {
        var pedidoCreateDto = new PedidoCreateDto()
        {
            PedidosPorPeso = new List<PedidoPorPesoModel>(),
            PedidosPorTamanho = new List<PedidoPorTamanhoModel>()
        };

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        var tabelaDePreco = TabelaDePrecoBuilder.Init().Build();

        tabelaDePrecoRepositoryMock.Setup((tb) => tb.GetTabelaDePrecoAtivaAsync()).ReturnsAsync(tabelaDePreco);

        var service = new PedidoService(
            pedidoRepositoryMock.Object,
            tabelaDePrecoRepositoryMock.Object,
            processarPedidoServiceMock.Object);
        var usuarioId = Guid.NewGuid();

        await Assert.ThrowsAsync<ExceptionApi>(async () => await service.CreatePedidoAsync(pedidoCreateDto, usuarioId));
    }
}

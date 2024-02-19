using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enums;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

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
        var tokenServiceMock = new Mock<ITokenService>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tokenServiceMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);

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
        var tokenServiceMock = new Mock<ITokenService>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tokenServiceMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);
        var pedidoViewModel = await pedidoService.UpdateStatusPedidoAsync(pedidoUpdateStatus);

        Assert.NotNull(pedidoViewModel);
        Assert.IsType<PedidoViewModel>(pedidoViewModel);
        Assert.Equal(pedido.Id, pedidoViewModel.Id);
        Assert.Equal(pedido.StatusPedido, pedidoViewModel.StatusPedido);
    }

    [Fact]
    public async Task DeveExcluirPedido()
    {
        var pedido = PedidoBuilder.Init().Build();

        var pedidoRepositoryMock = new Mock<IPedidoRepository>();
        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var tokenServiceMock = new Mock<ITokenService>();
        var processarPedidoServiceMock = new Mock<IProcessarPedidoService>();

        pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        pedidoRepositoryMock.Setup(x => x.DeleteAsync(pedido)).ReturnsAsync(true);

        var pedidoService = new PedidoService(pedidoRepositoryMock.Object, tokenServiceMock.Object, tabelaDePrecoRepositoryMock.Object, processarPedidoServiceMock.Object);
        var result = await pedidoService.DeletePedidoAsync(pedido.Id);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }
}

using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class ItemPedidoServiceTest
{
    private readonly UpdateQuantidadeItemPedidoDto _itemPedidoDto;
    private readonly Mock<IItensPedidoRepository> _itensPedidoRepositoryMock = new();
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock = new();
    private readonly Mock<INotificarPedidoEditadoService> _notificarPedidoEditadoService = new();
    private readonly ItensPedidoService _service;

    public ItemPedidoServiceTest()
    {
        _itemPedidoDto = new UpdateQuantidadeItemPedidoDto()
        {
            Quantidade = 0,
            Id = Guid.NewGuid()
        };

        _service = new ItensPedidoService(
            _itensPedidoRepositoryMock.Object, 
            _pedidoRepositoryMock.Object,
            _notificarPedidoEditadoService.Object);
    }

    [Fact]
    public async Task NaoDeveEditarItemDoPedidoComQuantidadeInvalida()
    {
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().Build();

        _itensPedidoRepositoryMock.Setup(x => x.GetItemPedidoByIdAsync(_itemPedidoDto.Id)).ReturnsAsync(item);
        _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(item.PedidoId)).ReturnsAsync(pedido);

        await Assert.ThrowsAsync<ExceptionApi>(async () => await _service.EditarQuantidadeDoItemAsync(_itemPedidoDto));
    }

    [Fact]
    public async Task NaoDeveExcluirUltimoItemDoPedido()
    {
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().Build();
        pedido.ItensPedido = new List<ItensPedido>() { item };

        _itensPedidoRepositoryMock.Setup(x => x.GetItemPedidoByIdAsync(_itemPedidoDto.Id)).ReturnsAsync(item);
        _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(item.PedidoId)).ReturnsAsync(pedido);

        await Assert.ThrowsAsync<ExceptionApi>(async () => await _service.DeleteItemPedidoAsync(item.Id));
    }

    [Theory]
    [InlineData(StatusPedido.Faturado)]
    [InlineData(StatusPedido.Cancelado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.RotaDeEntrega)]
    public async Task NaoDeveEditarItemDoPedidoComStatusDoPedidoDiferenteDeAberto(StatusPedido statusPedido)
    {
        _itemPedidoDto.Quantidade = 10;
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().ComStatusPedido(statusPedido).Build();

        _itensPedidoRepositoryMock.Setup(x => x.GetItemPedidoByIdAsync(_itemPedidoDto.Id)).ReturnsAsync(item);
        _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(item.PedidoId)).ReturnsAsync(pedido);

        await Assert.ThrowsAsync<ExceptionApi>(async () => await _service.EditarQuantidadeDoItemAsync(_itemPedidoDto));
    }

    [Fact]
    public async Task DeveEditarItemDoPedidoComQuantidade()
    {
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().Build();
        _itemPedidoDto.Quantidade = 10;
        _itemPedidoDto.Id = item.Id;

        _itensPedidoRepositoryMock.Setup(x => x.GetItemPedidoByIdAsync(_itemPedidoDto.Id)).ReturnsAsync(item);
        _pedidoRepositoryMock.Setup(x => x.GetPedidoByIdAsync(item.PedidoId)).ReturnsAsync(pedido);

        var itemEditado = await _service.EditarQuantidadeDoItemAsync(_itemPedidoDto);

        Assert.NotNull(itemEditado);
        Assert.Equal(_itemPedidoDto.Quantidade, itemEditado.Quantidade);
        Assert.Equal(_itemPedidoDto.Id, itemEditado.Id);

    }
}

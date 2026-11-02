using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class ItemPedidoServiceTest
{
    private readonly Mock<IItensPedidoRepository> _itensPedidoRepository = new();
    private readonly Mock<IPedidoRepository> _pedidoRepository = new();
    private readonly Mock<IProdutoRepository> _produtoRepository = new();
    private readonly ItensPedidoAdministracaoService _service;

    public ItemPedidoServiceTest()
    {
        _service = new ItensPedidoAdministracaoService(
            _itensPedidoRepository.Object,
            _pedidoRepository.Object,
            _produtoRepository.Object);
    }

    [Theory]
    [InlineData(StatusPedido.Faturado)]
    [InlineData(StatusPedido.Cancelado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.RotaDeEntrega)]
    public async Task NaoDeveEditarItemComStatusDiferenteDeAberto(StatusPedido status)
    {
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().ComStatusPedido(status).Build();
        pedido.ItensPedido = [item];
        item.Pedido = pedido;
        _itensPedidoRepository.Setup(x => x.ObterParaAdministracaoAsync(item.Id)).ReturnsAsync(item);

        var resultado = await _service.EditarQuantidadeAsync(new UpdateQuantidadeItemPedidoDto
        {
            Id = item.Id,
            Quantidade = 10
        });

        Assert.Equal("Este pedido não está aberto e não pode ser alterado!", resultado.Error);
    }

    [Fact]
    public async Task DeveValidarQuantidadeDepoisDeLocalizarPedidoEItem()
    {
        var item = ItensPedidoBuilder.Init().Build();
        var pedido = PedidoBuilder.Init().Build();
        pedido.ItensPedido = [item];
        item.Pedido = pedido;
        _itensPedidoRepository.Setup(x => x.ObterParaAdministracaoAsync(item.Id)).ReturnsAsync(item);

        var resultado = await _service.EditarQuantidadeAsync(new UpdateQuantidadeItemPedidoDto
        {
            Id = item.Id,
            Quantidade = 0
        });

        Assert.Equal("A quantidade deve ser maior que zero!", resultado.Error);
        _itensPedidoRepository.Verify(x => x.Update(It.IsAny<ItemPedido>()), Times.Never);
    }
}

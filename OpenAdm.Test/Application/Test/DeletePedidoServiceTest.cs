using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class DeletePedidoServiceTest
{
    private readonly Mock<IPedidoRepository> _pedidoRepository;
    private readonly Mock<IFaturaRepository> _faturaRepository;
    private readonly DeletePedidoService _deletePedidoService;

    public DeletePedidoServiceTest()
    {
        _pedidoRepository = new Mock<IPedidoRepository>();
        _faturaRepository = new Mock<IFaturaRepository>();
        _deletePedidoService = new DeletePedidoService(
            _pedidoRepository.Object,
            _faturaRepository.Object);
    }

    [Fact]
    public async Task DeveMarcarPedidoComoExcluido()
    {
        var pedido = PedidoBuilder.Init().Build();
        _pedidoRepository.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        _pedidoRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var resultado = await _deletePedidoService.DeletePedidoAsync(pedido.Id);

        Assert.True(resultado);
        Assert.True(pedido.Excluido);
        _pedidoRepository.Verify(x => x.DeleteAsync(It.IsAny<Pedido>()), Times.Never);
    }

    [Fact]
    public async Task NaoDeveExcluirPedidoEntregue()
    {
        var pedido = PedidoBuilder.Init().ComStatusPedido(StatusPedido.Entregue).Build();
        _pedidoRepository.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        await Assert.ThrowsAsync<ExceptionApi>(() => _deletePedidoService.DeletePedidoAsync(pedido.Id));

        Assert.False(pedido.Excluido);
        _pedidoRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveCancelarFaturaEInativarParcelasAoExcluirPedido()
    {
        var pedido = PedidoBuilder.Init().Build();
        var fatura = FaturaBuilder.Init().ComPedido(pedido).Build();
        var parcela = ParcelaBuilder.Init().ComFaturaId(fatura.Id).Build();
        fatura.Parcelas.Add(parcela);
        pedido.Fatura = fatura;
        _pedidoRepository.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);
        _pedidoRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        FaturaHistorico? historicoAdicionado = null;
        _faturaRepository.Setup(x => x.AddHistoricoAsync(It.IsAny<FaturaHistorico>()))
            .Callback<FaturaHistorico>(x => historicoAdicionado = x)
            .Returns(Task.CompletedTask);

        await _deletePedidoService.DeletePedidoAsync(pedido.Id);

        Assert.Equal(StatusFaturaEnum.Cancelada, fatura.Status);
        Assert.False(parcela.Ativo);
        Assert.NotNull(historicoAdicionado);
        Assert.Equal(fatura.Id, historicoAdicionado.FaturaId);
        Assert.Equal("Fatura cancelada pela exclusão do pedido.", historicoAdicionado.Descricao);
        _faturaRepository.Verify(x => x.AddHistoricoAsync(historicoAdicionado), Times.Once);
    }

    [Fact]
    public async Task NaoDeveExcluirPedidoComParcelaIntegradaExternamente()
    {
        var pedido = PedidoBuilder.Init().Build();
        var fatura = FaturaBuilder.Init().ComPedido(pedido).Build();
        var parcela = ParcelaBuilder.Init()
            .ComFaturaId(fatura.Id)
            .ComIdExterno("parcela-externa")
            .Build();
        fatura.Parcelas.Add(parcela);
        pedido.Fatura = fatura;
        _pedidoRepository.Setup(x => x.GetPedidoByIdAsync(pedido.Id)).ReturnsAsync(pedido);

        await Assert.ThrowsAsync<ExceptionApi>(() => _deletePedidoService.DeletePedidoAsync(pedido.Id));

        Assert.False(pedido.Excluido);
        _pedidoRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        _faturaRepository.Verify(x => x.AddHistoricoAsync(It.IsAny<FaturaHistorico>()), Times.Never);
    }

}

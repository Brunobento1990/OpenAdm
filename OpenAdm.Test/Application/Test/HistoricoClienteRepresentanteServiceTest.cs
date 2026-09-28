using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Test.Application.Test;

public sealed class HistoricoClienteRepresentanteServiceTest
{
    private readonly Mock<IPedidoRepository> _repository;
    private readonly HistoricoClienteRepresentanteService _service;

    public HistoricoClienteRepresentanteServiceTest()
    {
        _repository = new Mock<IPedidoRepository>();
        _service = new HistoricoClienteRepresentanteService(_repository.Object);
    }

    [Fact]
    public async Task DeveRetornarHistoricoFormatadoEFiltradoPeloRepresentante()
    {
        var clienteId = Guid.NewGuid();
        var representanteId = Guid.NewGuid();
        var dataUltimaCompra = DateTime.UtcNow.Date.AddDays(-10);
        _repository
            .Setup(x => x.ObterHistoricoClienteRepresentanteAsync(clienteId, representanteId))
            .ReturnsAsync(new HistoricoClienteRepresentanteModel
            {
                DataUltimaCompra = dataUltimaCompra,
                TicketMedio = 1480,
                ProdutoMaisComprado = "Produto X",
                ValorUltimoPedido = 1720
            });

        var resultado = await _service.ObterAsync(clienteId, representanteId);

        Assert.Equal($"Última compra: {dataUltimaCompra:dd/MM/yyyy} há 10 dias", resultado.UltimaCompra);
        Assert.Equal("Ticket médio: R$ 1.480,00", resultado.TicketMedio);
        Assert.Equal("Produto X", resultado.ProdutoMaisComprado);
        Assert.Equal("Último pedido — R$ 1.720,00", resultado.UltimoPedido);
    }

    [Fact]
    public async Task DeveRetornarValoresPadraoQuandoClienteNaoPossuiCompras()
    {
        _repository
            .Setup(x => x.ObterHistoricoClienteRepresentanteAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new HistoricoClienteRepresentanteModel());

        var resultado = await _service.ObterAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.Equal("Nenhuma compra realizada", resultado.UltimaCompra);
        Assert.Equal("Ticket médio: R$ 0,00", resultado.TicketMedio);
        Assert.Equal("Nenhum produto comprado", resultado.ProdutoMaisComprado);
        Assert.Equal("Último pedido — R$ 0,00", resultado.UltimoPedido);
    }
}

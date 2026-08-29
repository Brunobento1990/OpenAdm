using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Pdf.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class PedidoPublicoServiceTest
{
    private const string Chave = "12345678901234567890123456789012";
    private const string Iv = "1234567890123456";

    public PedidoPublicoServiceTest()
    {
        Criptografia.Configure(Chave, Iv);
    }

    [Fact]
    public async Task DeveRetornarNuloQuandoEmpresaNaoExiste()
    {
        var empresaRepository = new Mock<IEmpresaOpenAdmRepository>();
        var parceiroAutenticado = new Mock<IParceiroAutenticado>();
        var pedidoRepository = new Mock<IPedidoPublicoRepository>();
        var pdfService = new Mock<IPdfPedidoService>();
        var service = new PedidoPublicoService(
            empresaRepository.Object, parceiroAutenticado.Object, pedidoRepository.Object, pdfService.Object);

        var resultado = await service.GerarPdfAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.Null(resultado);
        pedidoRepository.Verify(x => x.GetPedidoCompletoByIdPublicoAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeveBloquearQuandoParceiroNaoPertenceAEmpresaDaUrl()
    {
        var empresaId = Guid.NewGuid();
        var empresaRepository = new Mock<IEmpresaOpenAdmRepository>();
        empresaRepository.Setup(x => x.ObterPorIdAsync(empresaId)).ReturnsAsync(Empresa(empresaId));

        var parceiroAutenticado = new Mock<IParceiroAutenticado>();
        parceiroAutenticado.SetupProperty(x => x.Id);
        parceiroAutenticado.SetupProperty(x => x.ConnectionString);
        parceiroAutenticado.Setup(x => x.ObterParceiroAutenticadoAsync())
            .ReturnsAsync(Parceiro(Guid.NewGuid()));
        var pedidoRepository = new Mock<IPedidoPublicoRepository>();
        var service = new PedidoPublicoService(
            empresaRepository.Object, parceiroAutenticado.Object, pedidoRepository.Object,
            Mock.Of<IPdfPedidoService>());

        var resultado = await service.GerarPdfAsync(empresaId, Guid.NewGuid());

        Assert.Null(resultado);
        Assert.Equal(empresaId, parceiroAutenticado.Object.Id);
        Assert.Equal("Host=tenant", parceiroAutenticado.Object.ConnectionString);
        pedidoRepository.Verify(x => x.GetPedidoCompletoByIdPublicoAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeveRetornarNuloQuandoEmpresaNaoPossuiParceiro()
    {
        var empresaId = Guid.NewGuid();
        var empresaRepository = new Mock<IEmpresaOpenAdmRepository>();
        empresaRepository.Setup(x => x.ObterPorIdAsync(empresaId)).ReturnsAsync(Empresa(empresaId));
        var parceiroAutenticado = new Mock<IParceiroAutenticado>();
        parceiroAutenticado.SetupProperty(x => x.Id);
        parceiroAutenticado.SetupProperty(x => x.ConnectionString);
        parceiroAutenticado.Setup(x => x.ObterParceiroAutenticadoAsync()).ThrowsAsync(new ExceptionApi("ausente"));
        var pedidoRepository = new Mock<IPedidoPublicoRepository>();
        var service = new PedidoPublicoService(
            empresaRepository.Object, parceiroAutenticado.Object, pedidoRepository.Object,
            Mock.Of<IPdfPedidoService>());

        var resultado = await service.GerarPdfAsync(empresaId, Guid.NewGuid());

        Assert.Null(resultado);
        pedidoRepository.Verify(x => x.GetPedidoCompletoByIdPublicoAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeveGerarPdfExistenteQuandoPedidoPertenceAEmpresa()
    {
        var empresaId = Guid.NewGuid();
        var pedido = PedidoBuilder.Init().Build();
        var empresaRepository = new Mock<IEmpresaOpenAdmRepository>();
        empresaRepository.Setup(x => x.ObterPorIdAsync(empresaId)).ReturnsAsync(Empresa(empresaId));
        var parceiroAutenticado = new Mock<IParceiroAutenticado>();
        parceiroAutenticado.SetupProperty(x => x.Id);
        parceiroAutenticado.SetupProperty(x => x.ConnectionString);
        parceiroAutenticado.Setup(x => x.ObterParceiroAutenticadoAsync()).ReturnsAsync(Parceiro(empresaId));
        var pedidoRepository = new Mock<IPedidoPublicoRepository>();
        pedidoRepository.Setup(x => x.GetPedidoCompletoByIdPublicoAsync(pedido.IdPublico)).ReturnsAsync(pedido);
        var pdfEsperado = new byte[] { 1, 2, 3 };
        var pdfService = new Mock<IPdfPedidoService>();
        pdfService.Setup(x => x.GeneratePdfPedido(pedido, It.IsAny<Parceiro>())).Returns(pdfEsperado);
        var service = new PedidoPublicoService(
            empresaRepository.Object, parceiroAutenticado.Object, pedidoRepository.Object,
            pdfService.Object);

        var resultado = await service.GerarPdfAsync(empresaId, pedido.IdPublico);

        Assert.Equal(pdfEsperado, resultado);
        pdfService.Verify(x => x.GeneratePdfPedido(pedido, It.Is<Parceiro>(p => p.EmpresaOpenAdmId == empresaId)), Times.Once);
    }

    private static EmpresaOpenAdm Empresa(Guid id) => new(
        id, DateTime.UtcNow, DateTime.UtcNow, 1, true, "https://loja", "https://admin",
        Criptografia.Encrypt("Host=tenant"), TipoParcelaCobrancaEnum.Gratis);

    private static Parceiro Parceiro(Guid empresaId) => new(
        Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, "Empresa", "Loja", "123", null, empresaId);
}

using OpenAdm.Application.Dtos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Pdf.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class RelatorioVendaDeProdutoServiceTest
{
    private readonly Mock<IRelatorioVendaDeProdutoRepository> _repository = new();
    private readonly Mock<IRelatorioVendaDeProdutoPdfService> _pdfService = new();
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado = new();

    [Fact]
    public async Task ListarAsync_DeveRetornarDadosPaginadosComTotalizadores()
    {
        var dados = new List<RelatorioVendaDeProdutoModel>
        {
            new() { Id = Guid.NewGuid(), Descricao = "Produto", Quantidade = 3, ValorTotal = 75 }
        };
        _repository
            .Setup(x => x.ListarAsync(null, null, 2, 50, false))
            .ReturnsAsync((dados, 4, 12, 300));

        var service = CriarService();

        var resultado = await service.ListarAsync(new RelatorioVendaDeProdutoDTO { Skip = 2 });

        Assert.Same(dados, resultado.Dados);
        Assert.Equal(4, resultado.TotalPagina);
        Assert.Equal(12, resultado.Totais.QuantidadeTotal);
        Assert.Equal(300, resultado.Totais.ValorTotal);
    }

    [Fact]
    public async Task ImprimirAsync_DeveBuscarTodosOsDadosEGerarPdf()
    {
        var dados = new List<RelatorioVendaDeProdutoModel>
        {
            new() { Id = Guid.NewGuid(), Descricao = "Produto", Quantidade = 2, ValorTotal = 40 }
        };
        var parceiro = new Parceiro(
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            1,
            "Razão social",
            "Nome fantasia",
            "123",
            null,
            Guid.NewGuid());
        var pdfEsperado = new byte[] { 1, 2, 3 };

        _repository
            .Setup(x => x.ListarAsync(null, null, 0, null, false))
            .ReturnsAsync((dados, 1, 2, 40));
        _parceiroAutenticado
            .Setup(x => x.ObterParceiroAutenticadoAsync())
            .ReturnsAsync(parceiro);
        _pdfService
            .Setup(x => x.Gerar(dados, parceiro.NomeFantasia, parceiro.Logo, null, null, 2, 40))
            .Returns(pdfEsperado);

        var resultado = await CriarService().ImprimirAsync(new RelatorioVendaDeProdutoDTO());

        Assert.Same(pdfEsperado, resultado);
        _repository.Verify(x => x.ListarAsync(null, null, 0, null, false), Times.Once);
    }

    private RelatorioVendaDeProdutoService CriarService() => new(
        _repository.Object,
        _pdfService.Object,
        _parceiroAutenticado.Object);
}

using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Test.Application.Test;

public class ResumoMensalHomeServiceTest
{
    private readonly Mock<IResumoMensalPedidoRepository> _repository = new();

    [Fact]
    public async Task ObterAsync_DeveCompararMesAtualComMesmoMesDoAnoAnterior()
    {
        var categoriaAtual = Guid.NewGuid();
        var categoriaSomenteAnterior = Guid.NewGuid();
        var anoAtual = DateTime.UtcNow.Year;

        _repository
            .Setup(x => x.ObterAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync((DateTime inicio, DateTime _) => inicio.Year == anoAtual
                ? new ResumoMensalPedidoModel
                {
                    QuantidadePedidos = 12,
                    ValorTotalVendido = 900,
                    QuantidadeItensVendidos = 30,
                    Categorias =
                    [
                        new ResumoMensalCategoriaModel
                        {
                            CategoriaId = categoriaAtual,
                            Categoria = "Bebidas",
                            QuantidadeItensVendidos = 30
                        }
                    ]
                }
                : new ResumoMensalPedidoModel
                {
                    QuantidadePedidos = 8,
                    ValorTotalVendido = 1_000,
                    QuantidadeItensVendidos = 20,
                    Categorias =
                    [
                        new ResumoMensalCategoriaModel
                        {
                            CategoriaId = categoriaAtual,
                            Categoria = "Bebidas",
                            QuantidadeItensVendidos = 10
                        },
                        new ResumoMensalCategoriaModel
                        {
                            CategoriaId = categoriaSomenteAnterior,
                            Categoria = "Alimentos",
                            QuantidadeItensVendidos = 10
                        }
                    ]
                });

        var resultado = await new ResumoMensalHomeService(_repository.Object).ObterAsync();

        Assert.Equal(DateTime.UtcNow.Month, resultado.Mes);
        Assert.Equal(12, resultado.QuantidadePedidos.Atual);
        Assert.Equal(8, resultado.QuantidadePedidos.AnoAnterior);
        Assert.Equal(4, resultado.QuantidadePedidos.Variacao);
        Assert.Equal(50, resultado.QuantidadePedidos.VariacaoPercentual);
        Assert.Equal(-100, resultado.ValorTotalVendido.Variacao);
        Assert.Equal(-10, resultado.ValorTotalVendido.VariacaoPercentual);
        Assert.Equal(2, resultado.Categorias.Count);

        var bebidas = resultado.Categorias.Single(x => x.CategoriaId == categoriaAtual);
        Assert.Equal(20, bebidas.QuantidadeItensVendidos.Variacao);
        Assert.Equal(200, bebidas.QuantidadeItensVendidos.VariacaoPercentual);

        var alimentos = resultado.Categorias.Single(x => x.CategoriaId == categoriaSomenteAnterior);
        Assert.Equal(0, alimentos.QuantidadeItensVendidos.Atual);
        Assert.Equal(-10, alimentos.QuantidadeItensVendidos.Variacao);
        Assert.Equal(-100, alimentos.QuantidadeItensVendidos.VariacaoPercentual);
    }

    [Fact]
    public async Task ObterAsync_DeveTratarAnoAnteriorSemRegistros()
    {
        var anoAtual = DateTime.UtcNow.Year;
        _repository
            .Setup(x => x.ObterAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync((DateTime inicio, DateTime _) => new ResumoMensalPedidoModel
            {
                ValorTotalVendido = inicio.Year == anoAtual ? 250 : 0
            });

        var resultado = await new ResumoMensalHomeService(_repository.Object).ObterAsync();

        Assert.Equal(100, resultado.ValorTotalVendido.VariacaoPercentual);
        Assert.Equal(0, resultado.QuantidadePedidos.VariacaoPercentual);
    }
}

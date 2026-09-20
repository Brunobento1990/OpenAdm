using OpenAdm.Application.Dtos.FaturasDtos;

namespace OpenAdm.Test.Application.Test;

public class RenegociarFaturaDtoTest
{
    [Fact]
    public void DeveValidarTotalComArredondamentoPorParcela()
    {
        var dto = new RenegociarFaturaDto
        {
            FaturaId = Guid.NewGuid(),
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 10.005m, DataDeVencimento = DateTime.UtcNow },
                new() { NumeroDaParcela = 2, Valor = 9.995m, DataDeVencimento = DateTime.UtcNow }
            ]
        };

        Assert.Null(dto.Validar());
        Assert.Null(dto.ValidarTotal(20.01m));
        Assert.NotNull(dto.ValidarTotal(20.00m));
    }

    [Fact]
    public void DeveRejeitarNumerosRepetidos()
    {
        var dto = new RenegociarFaturaDto
        {
            FaturaId = Guid.NewGuid(),
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 10, DataDeVencimento = DateTime.UtcNow },
                new() { NumeroDaParcela = 1, Valor = 10, DataDeVencimento = DateTime.UtcNow }
            ]
        };

        Assert.Contains("repetidos", dto.Validar());
    }

    [Fact]
    public void DeveRejeitarFaturaIdVazio()
    {
        var dto = new RenegociarFaturaDto
        {
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 10, DataDeVencimento = DateTime.UtcNow }]
        };

        Assert.Equal("Informe a fatura!", dto.Validar());
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void DeveRejeitarNumeroOuValorInvalido(int numero, decimal valor)
    {
        var dto = new RenegociarFaturaDto
        {
            FaturaId = Guid.NewGuid(),
            Parcelas = [new() { NumeroDaParcela = numero, Valor = valor, DataDeVencimento = DateTime.UtcNow }]
        };

        Assert.Equal("Os dados das parcelas são inválidos!", dto.Validar());
    }

    [Fact]
    public void DeveRejeitarItemNuloSemLancarExcecao()
    {
        var dto = new RenegociarFaturaDto
        {
            FaturaId = Guid.NewGuid(),
            Parcelas = [null!]
        };

        Assert.Equal("Os dados das parcelas são inválidos!", dto.Validar());
    }

    [Fact]
    public void DeveRejeitarListaVazia()
    {
        var dto = new RenegociarFaturaDto { FaturaId = Guid.NewGuid() };

        Assert.Equal("Informe ao menos uma parcela!", dto.Validar());
    }
}

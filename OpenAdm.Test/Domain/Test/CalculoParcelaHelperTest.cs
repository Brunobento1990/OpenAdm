using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Domain.Test;

public class CalculoParcelaHelperTest
{
    [Fact]
    public void DeveSepararValorRecebidoLiquidoEDescontoConcedido()
    {
        ITransacaoParaCalculo[] transacoes =
        [
            new TransacaoParaCalculo(TipoTransacaoFinanceiraEnum.Entrada, 90, 10, null)
        ];

        var valorPagoRecebido = CalculoParcelaHelper.CalcularValorPagoRecebido(
            TipoFaturaEnum.AReceber, transacoes);
        var valorPagoRecebidoLiquido = CalculoParcelaHelper.CalcularValorPagoRecebidoLiquido(
            TipoFaturaEnum.AReceber, transacoes);
        var descontoConcedido = CalculoParcelaHelper.CalcularDescontoConcedido(
            TipoFaturaEnum.AReceber, transacoes);

        Assert.Equal(100, valorPagoRecebido);
        Assert.Equal(90, valorPagoRecebidoLiquido);
        Assert.Equal(10, descontoConcedido);
    }

    [Fact]
    public void DeveDescontarValorEDescontoQuandoHouverEstorno()
    {
        ITransacaoParaCalculo[] transacoes =
        [
            new TransacaoParaCalculo(TipoTransacaoFinanceiraEnum.Saida, 90, 10, null),
            new TransacaoParaCalculo(TipoTransacaoFinanceiraEnum.Entrada, 90, 10, null)
        ];

        var valorPagoRecebidoLiquido = CalculoParcelaHelper.CalcularValorPagoRecebidoLiquido(
            TipoFaturaEnum.APagar, transacoes);
        var descontoConcedido = CalculoParcelaHelper.CalcularDescontoConcedido(
            TipoFaturaEnum.APagar, transacoes);

        Assert.Equal(0, valorPagoRecebidoLiquido);
        Assert.Equal(0, descontoConcedido);
    }

    private sealed record TransacaoParaCalculo(
        TipoTransacaoFinanceiraEnum TipoTransacaoFinanceira,
        decimal Valor,
        decimal? Desconto,
        decimal? Juros) : ITransacaoParaCalculo;
}

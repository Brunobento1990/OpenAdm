using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Domain.Helpers;

public class CalculoParcelaHelper
{
    public static decimal CalcularValorPagoRecebido(
        TipoFaturaEnum tipo,
        IEnumerable<ITransacaoParaCalculo>? transacoes)
    {
        if (transacoes == null || !transacoes.Any())
        {
            return 0;
        }

        var (tipoPago, tipoEstorno) = tipo == TipoFaturaEnum.APagar
            ? (TipoTransacaoFinanceiraEnum.Saida, TipoTransacaoFinanceiraEnum.Entrada)
            : (TipoTransacaoFinanceiraEnum.Entrada, TipoTransacaoFinanceiraEnum.Saida);

        return transacoes.Where(x => x.TipoTransacaoFinanceira == tipoPago).Sum(x => x.ValorLiquido)
               - transacoes.Where(x => x.TipoTransacaoFinanceira == tipoEstorno).Sum(x => x.ValorLiquido);
    }

    public static decimal CalcularValorPagoRecebidoLiquido(
        TipoFaturaEnum tipo,
        IEnumerable<ITransacaoParaCalculo>? transacoes)
    {
        return CalcularValorPorTipo(tipo, transacoes, x => x.Valor);
    }

    public static decimal CalcularDescontoConcedido(
        TipoFaturaEnum tipo,
        IEnumerable<ITransacaoParaCalculo>? transacoes)
    {
        return CalcularValorPorTipo(tipo, transacoes, x => x.Desconto ?? 0);
    }

    private static decimal CalcularValorPorTipo(
        TipoFaturaEnum tipo,
        IEnumerable<ITransacaoParaCalculo>? transacoes,
        Func<ITransacaoParaCalculo, decimal> seletor)
    {
        if (transacoes == null)
        {
            return 0;
        }

        var (tipoPago, tipoEstorno) = tipo == TipoFaturaEnum.APagar
            ? (TipoTransacaoFinanceiraEnum.Saida, TipoTransacaoFinanceiraEnum.Entrada)
            : (TipoTransacaoFinanceiraEnum.Entrada, TipoTransacaoFinanceiraEnum.Saida);

        return transacoes.Where(x => x.TipoTransacaoFinanceira == tipoPago).Sum(seletor)
               - transacoes.Where(x => x.TipoTransacaoFinanceira == tipoEstorno).Sum(seletor);
    }
}

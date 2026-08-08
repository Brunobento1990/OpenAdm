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
}
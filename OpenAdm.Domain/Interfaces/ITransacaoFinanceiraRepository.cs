using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface ITransacaoFinanceiraRepository
{
    Task<IList<TransacaoFinanceira>> TransacoesNoPeriodoAsync(
        DateTime dataInicial,
        DateTime dataFinal,
        Guid? pedidoId,
        Guid? clienteId);

    Task<IDictionary<int, List<TransacaoFinanceira>>> SumTotalMesesAsync(TipoFaturaEnum faturaEnum);
}

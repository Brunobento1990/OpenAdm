using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITransacaoFinanceiraRepository
{
    Task<IList<TransacaoFinanceira>> TransacoesNoPeriodoAsync(
        DateTime dataInicial,
        DateTime dataFinal,
        Guid? pedidoId,
        Guid? clienteId);
}

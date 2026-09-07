using OpenAdm.Application.Dtos.TransacoesFinanceiras;
using OpenAdm.Application.Models.Transacoes;

namespace OpenAdm.Application.Interfaces;

public interface ITransacaoFinanceiraService
{
    Task<IDictionary<DateTime, TransacoesFinanceirasPorDiaViewModel>> TransacoesNoPeriodoAsync(
        TransacaoFinanceiraNoPeriodoDto transacaoFinanceiraNoPeriodoDto);
}

using OpenAdm.Application.Dtos.TransacoesFinanceiras;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Transacoes;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly ITransacaoFinanceiraRepository _transacaoFinanceiraRepository;

    public TransacaoFinanceiraService(ITransacaoFinanceiraRepository transacaoFinanceiraRepository)
    {
        _transacaoFinanceiraRepository = transacaoFinanceiraRepository;
    }

    public async Task<IList<TransacaoFinanceiraViewModel>> TransacoesNoPeriodoAsync(
        TransacaoFinanceiraNoPeriodoDto transacaoFinanceiraNoPeriodoDto)
    {
        var transacoes = await _transacaoFinanceiraRepository
            .TransacoesNoPeriodoAsync(dataInicial: transacaoFinanceiraNoPeriodoDto.DataInicial,
                dataFinal: transacaoFinanceiraNoPeriodoDto.DataFinal,
                pedidoId: transacaoFinanceiraNoPeriodoDto.PedidoId,
                clienteId: transacaoFinanceiraNoPeriodoDto.ClienteId);

        return transacoes.Select(x => (TransacaoFinanceiraViewModel)x).ToList();
    }
}

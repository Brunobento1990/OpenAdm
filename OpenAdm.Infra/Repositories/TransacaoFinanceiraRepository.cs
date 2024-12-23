using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TransacaoFinanceiraRepository : ITransacaoFinanceiraRepository
{
    private readonly ParceiroContext _parceiroContext;

    public TransacaoFinanceiraRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IList<TransacaoFinanceira>> TransacoesNoPeriodoAsync(
        DateTime dataInicial,
        DateTime dataFinal,
        Guid? pedidoId,
        Guid? clienteId)
    {
        var query = _parceiroContext
            .TransacoesFinanceiras
            .AsNoTracking()
            .Include(x => x.Parcela)
                .ThenInclude(x => x!.Fatura)
                    .ThenInclude(x => x.Pedido)
            .Include(x => x.Parcela)
                .ThenInclude(x => x!.Fatura.Usuario)
            .Where(x => x.DataDePagamento.Date <= dataFinal.Date && x.DataDePagamento.Date >= dataInicial.Date);

        if (pedidoId.HasValue)
        {
            query = query.Where(x => x.Parcela!.Fatura.PedidoId == pedidoId);
        }

        if (clienteId.HasValue)
        {
            query = query.Where(x => x.Parcela!.Fatura.UsuarioId == clienteId);
        }

        return await query.ToListAsync();
    }
}

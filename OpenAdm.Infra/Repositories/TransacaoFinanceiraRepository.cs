using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
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

    public async Task<IDictionary<int, List<TransacaoFinanceira>>> SumTotalMesesAsync(TipoFaturaEnum faturaEnum)
    {
        var dataInicio = DateTime.Now.AddMonths(-3);
        var dataSplit = dataInicio.ToString("MM/dd/yyyy").Split('/');
        var ano = int.Parse(dataSplit[2][..4]);
        var mes = int.Parse(dataSplit[0]);

        return await _parceiroContext
            .TransacoesFinanceiras
            .AsNoTracking()
            .Include(x => x.Parcela!.Fatura)
            .Where(m => m.DataDeCriacao.Month >= mes &&
                m.DataDeCriacao.Year == ano)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.ToList());
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

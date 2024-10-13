using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class FaturaContasAReceberRepository : GenericRepository<FaturaContasAReceber>, IFaturaContasAReceberRepository
{
    public FaturaContasAReceberRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<FaturaContasAReceber?> GetByIdAsync(Guid id)
    {
        return await _parceiroContext
            .FaturasContasAReceber
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<FaturaContasAReceber>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum)
    {
        var query = _parceiroContext
            .FaturasContasAReceber
            .AsNoTracking()
            .Include(x => x.ContasAReceber)
            .Where(x => x.ContasAReceber.PedidoId == pedidoId);

        if (statusFaturaContasAReceberEnum.HasValue)
        {
            query = query.Where(x => x.Status == statusFaturaContasAReceberEnum);
        }

        return await query
            .ToListAsync();
    }

    public async Task<IDictionary<int, decimal>> SumMesesAsync()
    {
        var dataInicio = DateTime.Now.AddMonths(-3);
        var dataSplit = dataInicio.ToString("MM/dd/yyyy").Split('/');
        var ano = int.Parse(dataSplit[2][..4]);
        var mes = int.Parse(dataSplit[0]);

        return await _parceiroContext
            .FaturasContasAReceber
            .Where(m => m.DataDeCriacao.Month >= mes && m.DataDeCriacao.Year == ano && m.Status == StatusFaturaContasAReceberEnum.Pago)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Sum(x => x.Valor));
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ParcelaRepository : GenericRepository<Parcela>, IParcelaRepository
{
    public ParcelaRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<Parcela?> GetByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Parcelas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Parcela?> GetByIdExternoAsync(string idExterno)
    {
        return await _parceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura)
            .FirstOrDefaultAsync(x => x.IdExterno == idExterno);
    }

    public async Task<IList<Parcela>> GetByPedidoIdAsync(Guid pedidoId, StatusParcelaEnum? statusFaturaContasAReceberEnum)
    {
        var query = _parceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura)
            .Where(x => x.Fatura.PedidoId == pedidoId);

        if (statusFaturaContasAReceberEnum.HasValue)
        {
            query = query.Where(x => x.Status == statusFaturaContasAReceberEnum);
        }

        return await query
            .ToListAsync();
    }

    public async Task<decimal> SumAReceberAsync()
    {
        try
        {
            return await _parceiroContext
                .Parcelas
                .AsNoTracking()
                .Where(x => x.Status == StatusParcelaEnum.Pendente)
                .SumAsync(x => x.Valor);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<IDictionary<int, decimal>> SumMesesAsync()
    {
        var dataInicio = DateTime.Now.AddMonths(-3);
        var dataSplit = dataInicio.ToString("MM/dd/yyyy").Split('/');
        var ano = int.Parse(dataSplit[2][..4]);
        var mes = int.Parse(dataSplit[0]);

        return await _parceiroContext
            .Parcelas
            .AsNoTracking()
            .Where(m => m.DataDeCriacao.Month >= mes && m.DataDeCriacao.Year == ano && m.Status == StatusParcelaEnum.Pago)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Sum(x => x.Valor));
    }
}

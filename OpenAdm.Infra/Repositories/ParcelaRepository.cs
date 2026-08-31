using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Data.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ParcelaRepository : GenericRepository<Parcela>, IParcelaRepository
{
    public ParcelaRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }
    
    public async Task<Parcela?> ObterParaPagarAsync(Guid id)
    {
        return await ParceiroContext
            .Parcelas
            .Include(x => x.Transacoes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Parcela?> ObterParaEstornarAsync(Guid id)
    {
        return await ParceiroContext
            .Parcelas
            .Include(x => x.Transacoes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Parcela?> GetByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura.Usuario)
            .Include(x => x.Fatura.Pedido)
            .Include(x => x.Transacoes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Parcela?> GetByIdExternoAsync(string idExterno)
    {
        return await ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura.Usuario)
            .Include(x => x.Transacoes)
            .Include(x => x.Fatura)
                .ThenInclude(x => x.Pedido!.ItensPedido)
            .AsSingleQuery()
            .FirstOrDefaultAsync(x => x.IdExterno == idExterno);
    }

    public async Task<IList<Parcela>> GetByPedidoIdAsync(Guid pedidoId)
    {
        var query = ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura)
            .Where(x => x.Fatura.PedidoId == pedidoId);

        return await query
            .ToListAsync();
    }

    public async Task<IList<Parcela>> ListaParcelasTotalizadorAsync(TipoFaturaEnum tipoFatura)
    {
        return await ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura)
            .Include(x => x.Transacoes)
            .Where(x => x.Fatura.Tipo == tipoFatura)
            .ToListAsync();
    }

    public async Task<IDictionary<int, decimal>> SumTotalMesesAsync(TipoFaturaEnum faturaEnum)
    {
        var dataInicio = DateTime.UtcNow.AddMonths(-3);
        var dataSplit = dataInicio.ToString("MM/dd/yyyy").Split('/');
        var ano = int.Parse(dataSplit[2][..4]);
        var mes = int.Parse(dataSplit[0]);

        return await ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura)
            .Where(m => m.DataDeCriacao.Month >= mes &&
                        m.DataDeCriacao.Year == ano &&
                        m.Fatura.Tipo == faturaEnum)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Sum(x => x.Valor));
    }

    public async Task AdicionarTransacaoAsync(TransacaoFinanceira transacaoFinanceira)
    {
        await ParceiroContext.TransacoesFinanceiras.AddAsync(transacaoFinanceira);
        await ParceiroContext.SaveChangesAsync();
    }

    public async Task AdicionarTransacoesAsync(IEnumerable<TransacaoFinanceira> transacoes)
    {
        await ParceiroContext.TransacoesFinanceiras.AddRangeAsync(transacoes);
    }
}

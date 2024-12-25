using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public sealed class ParcelaRepository : GenericRepository<Parcela>, IParcelaRepository
{
    public ParcelaRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public override async Task<PaginacaoViewModel<Parcela>> PaginacaoAsync(FilterModel<Parcela> filterModel)
    {
        var select = filterModel.SelectCustom();

        var query = ParceiroContext
            .Parcelas
            .AsNoTracking()
            .Include(x => x.Fatura.Usuario)
            .Include(x => x.Transacoes)
            .WhereIsNotNull(filterModel.GetWhereBySearch());

        if (select != null)
        {
            query = query.Select(select);
        }

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel);

        var totalDeRegistros = await ParceiroContext.Parcelas
            .WhereIsNotNull(filterModel.GetWhereBySearch()).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
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
            .Include(x => x.Fatura)
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
        var dataInicio = DateTime.Now.AddMonths(-3);
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
    }
}

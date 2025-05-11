using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class MovimentacaoDeProdutoRepository : GenericRepository<MovimentacaoDeProduto>, IMovimentacaoDeProdutoRepository
{
    public MovimentacaoDeProdutoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task AddRangeAsync(IList<MovimentacaoDeProduto> movimentacaoDeProdutos)
    {
        await ParceiroContext.AddRangeAsync(movimentacaoDeProdutos);
        await ParceiroContext.SaveChangesAsync();
    }

    public async Task<IDictionary<int, List<MovimentacaoDeProduto>>> CountTresMesesAsync(DateTime dataInicio, DateTime dataFinal)
    {
        return await ParceiroContext
            .MovimentacoesDeProdutos
            .Where(m => m.DataDeCriacao >= dataInicio && m.DataDeCriacao <= dataFinal)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Select(x => x).ToList());
    }

    public async Task<IList<MovimentacaoDeProduto>> RelatorioAsync(
        DateTime dataInicial,
        DateTime dataFinal,
        IList<Guid> produtosIds,
        IList<Guid> pesosIds,
        IList<Guid> tamanhosIds)
    {
        var query = ParceiroContext
            .MovimentacoesDeProdutos
            .OrderBy(x => x.DataDeCriacao)
            .Where(x => x.DataDeCriacao.Date >= dataInicial.Date && x.DataDeCriacao.Date <= dataFinal.Date);

        if (produtosIds.Count > 0)
        {
            query = query.Where(x => produtosIds.Contains(x.ProdutoId));
        }

        if (tamanhosIds.Count > 0)
        {
            query = query.Where(x => tamanhosIds.Contains(x.TamanhoId!.Value));
        }

        if (pesosIds.Count > 0)
        {
            query = query.Where(x => pesosIds.Contains(x.PesoId!.Value));
        }

        return await query.ToListAsync();
    }

}

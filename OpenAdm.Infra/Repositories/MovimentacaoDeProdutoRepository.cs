﻿using OpenAdm.Domain.Entities;
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

    public async Task<IDictionary<int, decimal>> CountTresMesesAsync()
    {
        var dataInicio = DateTime.Now.AddMonths(-3);
        var dataSplit = dataInicio.ToString("MM/dd/yyyy").Split('/');
        var ano = int.Parse(dataSplit[2][..4]);
        var mes = int.Parse(dataSplit[0]);

        return await ParceiroContext
            .MovimentacoesDeProdutos
            .Where(m => m.DataDeCriacao.Month >= mes && m.DataDeCriacao.Year == ano)
            .GroupBy(m => m.DataDeCriacao.Month)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Sum(x => x.QuantidadeMovimentada));
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

        if(produtosIds.Count > 0)
        {
            query = query.Where(x => produtosIds.Contains(x.ProdutoId));
        }

        if(tamanhosIds.Count > 0)
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

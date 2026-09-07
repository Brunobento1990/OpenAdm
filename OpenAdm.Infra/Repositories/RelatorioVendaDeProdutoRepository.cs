using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class RelatorioVendaDeProdutoRepository : IRelatorioVendaDeProdutoRepository
{
    private readonly ParceiroContext _context;

    public RelatorioVendaDeProdutoRepository(ParceiroContext context)
    {
        _context = context;
    }

    public async Task<(ICollection<RelatorioVendaDeProdutoModel> Dados, int TotalPagina, decimal QuantidadeTotal,
        decimal ValorTotal)> ListarAsync(DateTime? dataInicial,
        DateTime? dataFinal,
        int skip, int? take, bool asc)
    {
        var query = _context.ItensPedidos
            .AsNoTracking()
            .Where(x => x.Pedido.StatusPedido == StatusPedido.Entregue);

        if (dataInicial.HasValue)
        {
            query = query.Where(x => x.Pedido.DataDeCriacao >= dataInicial.Value);
        }

        if (dataFinal.HasValue)
        {
            query = query.Where(x => x.Pedido.DataDeCriacao <= dataFinal.Value);
        }

        var quantidadeTotal = await query.SumAsync(x => x.Quantidade);
        var valorTotal = await query.SumAsync(x => x.ValorUnitario * x.Quantidade);

        var agrupado = query
            .GroupBy(x => new
            {
                x.ProdutoId,
                x.PesoId,
                x.TamanhoId,
                x.Produto.Descricao,
                x.Produto.UrlFoto,
                PesoDescricao = x.Peso!.Descricao,
                TamanhoDescricao = x.Tamanho!.Descricao
            });

        var novaQuery = agrupado.Select(g => new RelatorioVendaDeProdutoModel
        {
            Id = g.Key.ProdutoId,
            Descricao = g.Key.Descricao,
            Foto = g.Key.UrlFoto,
            Quantidade = g.Sum(x => x.Quantidade),
            ValorTotal = g.Sum(x => x.ValorUnitario * x.Quantidade),
            Peso = g.Key.PesoDescricao,
            Tamanho = g.Key.TamanhoDescricao
        });

        if (asc)
        {
            novaQuery = novaQuery.OrderBy(x => x.Quantidade);
        }
        else
        {
            novaQuery = novaQuery.OrderByDescending(x => x.Quantidade);
        }

        var totalPagina = take.HasValue
            ? await novaQuery.CountCustomAsync(take.Value)
            : 1;

        if (take.HasValue)
            novaQuery = novaQuery.Paginate(skip, take.Value);

        var dados = await novaQuery.ToListAsync();

        return (dados, totalPagina, quantidadeTotal, valorTotal);
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ProdutoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Produto>(parceiroContext), IProdutoRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<IList<Produto>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _parceiroContext
            .ProdutosMaisVendidos
            .AsNoTracking()
            .Select(x =>
                    new ProdutosMaisVendidos(
                        x.Id,
                        x.DataDeCriacao,
                        x.DataDeAtualizacao,
                        x.Numero,
                        x.QuantidadeProdutos / x.QuantidadePedidos,
                        x.QuantidadePedidos,
                        x.ProdutoId))
            .ToListAsync();

        var produtosIds = produtosMaisVendidos
            .OrderByDescending(x => x.QuantidadeProdutos)
            .Select(x => x.ProdutoId)
            .Take(3)
            .ToList();

        return await _parceiroContext
            .Produtos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.Id))
            .ToListAsync();
    }
}

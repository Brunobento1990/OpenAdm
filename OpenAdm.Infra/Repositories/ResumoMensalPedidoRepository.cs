using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Infra.Repositories;

public sealed class ResumoMensalPedidoRepository : IResumoMensalPedidoRepository
{
    private readonly ParceiroContext _context;

    public ResumoMensalPedidoRepository(ParceiroContext context)
    {
        _context = context;
    }

    public async Task<ResumoMensalPedidoModel> ObterAsync(DateTime inicio, DateTime fim)
    {
        var pedidos = _context.Pedidos
            .AsNoTracking()
            .Where(x => x.StatusPedido == StatusPedido.Entregue &&
                        x.DataDeCriacao >= inicio &&
                        x.DataDeCriacao < fim);

        var totais = await pedidos
            .GroupBy(_ => 1)
            .Select(g => new
            {
                QuantidadePedidos = g.Count(),
                ValorTotalVendido = g.SelectMany(x => x.ItensPedido)
                    .Sum(x => x.ValorUnitario * x.Quantidade),
                QuantidadeItensVendidos = g.SelectMany(x => x.ItensPedido)
                    .Sum(x => x.Quantidade)
            })
            .FirstOrDefaultAsync();

        var categorias = await _context.ItensPedidos
            .AsNoTracking()
            .Where(x => x.Pedido.StatusPedido == StatusPedido.Entregue &&
                        !x.Pedido.Excluido &&
                        x.Pedido.DataDeCriacao >= inicio &&
                        x.Pedido.DataDeCriacao < fim)
            .GroupBy(x => new { x.Produto.CategoriaId, x.Produto.Categoria.Descricao })
            .Select(g => new ResumoMensalCategoriaModel
            {
                CategoriaId = g.Key.CategoriaId,
                Categoria = g.Key.Descricao,
                QuantidadeItensVendidos = g.Sum(x => x.Quantidade)
            })
            .ToListAsync();

        return new ResumoMensalPedidoModel
        {
            QuantidadePedidos = totais?.QuantidadePedidos ?? 0,
            ValorTotalVendido = totais?.ValorTotalVendido ?? 0,
            QuantidadeItensVendidos = totais?.QuantidadeItensVendidos ?? 0,
            Categorias = categorias
        };
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Infra.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly ParceiroContext _parceiroContext;

    public HomeRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IList<StatusPedidoHomeModel>> ObterStatusPedidosAsync()
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .GroupBy(x => x.StatusPedido)
            .Select(x => new StatusPedidoHomeModel()
            {
                Status = x.Key,
                Quantidade = x.Count()
            })
            .OrderByDescending(y => y.Quantidade)
            .ToListAsync();
    }

    public async Task<int> CountDePedidosAsync()
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .CountAsync();
    }

    public async Task<ICollection<ProdutoMaisVendidoModel>> ProdutosMaisVendidosAsync(bool asc)
    {
        var query =
            _parceiroContext.ItensPedidos
                .AsNoTracking()
                .Where(x => x.Pedido.StatusPedido == StatusPedido.Entregue)
                .GroupBy(x => new
                {
                    x.ProdutoId,
                    x.PesoId,
                    x.TamanhoId,
                    x.Produto.Descricao,
                    x.Produto.UrlFoto,
                    PesoDescricao = x.Peso!.Descricao,
                    TamanhoDescricao = x.Tamanho!.Descricao
                })
                .Select(g => new ProdutoMaisVendidoModel
                {
                    Id = g.Key.ProdutoId,
                    Descricao = g.Key.Descricao,
                    Foto = g.Key.UrlFoto,
                    Quantidade = g.Sum(x => x.Quantidade),
                    ValorUnitario = g.Max(x => x.ValorUnitario),
                    Peso = g.Key.PesoDescricao,
                    Tamanho = g.Key.TamanhoDescricao
                });

        query = asc ? query.OrderBy(x => x.Quantidade) : query.OrderByDescending(x => x.Quantidade);

        return await query
            .Take(3)
            .ToListAsync();
    }

    public async Task<ICollection<ProdutoMaisVendidoModel>> ProdutosMenosVendidosAsync()
    {
        return await _parceiroContext.ItensPedidos
            .AsNoTracking()
            .GroupBy(x => new
            {
                x.ProdutoId,
                x.PesoId,
                x.TamanhoId,
                x.Produto.Descricao,
                x.Produto.UrlFoto,
                PesoDescricao = x.Peso!.Descricao,
                TamanhoDescricao = x.Tamanho!.Descricao
            })
            .Select(g => new ProdutoMaisVendidoModel
            {
                Id = g.Key.ProdutoId,
                Descricao = g.Key.Descricao,
                Foto = g.Key.UrlFoto,
                Quantidade = g.Sum(x => x.Quantidade),
                Peso = g.Key.PesoDescricao,
                Tamanho = g.Key.TamanhoDescricao
            })
            .OrderBy(x => x.Quantidade)
            .Take(3)
            .ToListAsync();
    }

    public async Task<TotalizadorProtudoEstoqueHome?> ObterTotalizadoProtudoEstoqueAsync()
    {
        var quantidade = await _parceiroContext
            .Estoques
            .AsNoTracking()
            .SumAsync(x => (decimal?)x.Quantidade) ?? 0;

        var quantidadeReservada = await _parceiroContext
            .ItensPedidos
            .AsNoTracking()
            .Where(x => x.Pedido.StatusPedido == StatusPedido.Aberto)
            .SumAsync(x => (decimal?)x.Quantidade) ?? 0;

        return new TotalizadorProtudoEstoqueHome
        {
            Quantidade = quantidade,
            QuantidadeReservada = quantidadeReservada
        };
    }

    public async Task<IList<ContadorPedidoModel>> ContatorPedido7DiasAsync(DateTime dataInicio)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .Where(p => p.DataDeCriacao >= dataInicio)
            .GroupBy(p => p.DataDeCriacao.Date)
            .Select(g => new ContadorPedidoModel()
            {
                Data = g.Key,
                Total = g.Count()
            })
            .ToListAsync();
    }
}
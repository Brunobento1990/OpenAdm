using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Infra.Repositories;

public class PedidoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Pedido>(parceiroContext), IPedidoRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(FilterModel<Pedido> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Pedidos
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .IgnoreQueryFilters()
                .OrderByDescending(x => EF.Property<Pedido>(x, filterModel.OrderBy))
                .Include(x => x.Usuario)
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }

    public async Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.Usuario)
            .Include(x => x.ItensPedido)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id)
    {
        var pedido = await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (pedido != null)
        {
            foreach (var item in pedido.ItensPedido)
            {
                if (item.Pedido != null)
                    item.Pedido = null;

                if (item.Produto != null)
                {
                    item.Produto.Tamanhos = new();
                    item.Produto.Pesos = new();
                    item.Produto.ItensPedido = new();
                    item.Produto.ItensTabelaDePreco = new();
                }

                if (item.Tamanho != null)
                {
                    item.Tamanho.ItensPedido = new();
                }

                if (item.Peso != null)
                {
                    item.Peso.ItensPedido = new();
                }
            }
        }

        return pedido;
    }

    public async Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .OrderByDescending(x => x.DataDeCriacao)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .Include(x => x.Usuario)
            .Where(x => x.DataDeCriacao.Date <= relatorioPedidoDto.DataFinal.Date &&
                x.DataDeCriacao.Date >= relatorioPedidoDto.DataInicial.Date &&
                x.StatusPedido == StatusPedido.Entregue)
            .WhereIsNotNull(relatorioPedidoDto.WhereUsuarioId())
            .ToListAsync();
    }

    public async Task<List<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        return await _parceiroContext.Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.ItensPedido)
            .OrderByDescending(x => x.Numero)
            .AsQueryable()
            .Where(x => x.UsuarioId == usuarioId && x.StatusPedido == (StatusPedido)statusPedido)
            .ToListAsync();
    }

    public async Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UsuarioId == usuarioId)
            .CountAsync();
    }
}

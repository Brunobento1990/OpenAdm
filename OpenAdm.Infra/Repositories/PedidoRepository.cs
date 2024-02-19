using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enums;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class PedidoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Pedido>(parceiroContext), IPedidoRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var (total, values) = await _parceiroContext
                .Pedidos
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Pedido>(x, paginacaoPedidoDto.OrderBy))
                .Include(x => x.Usuario)
                .WhereIsNotNull(paginacaoPedidoDto.GetWhereBySearch())
                .WhereIsNotNull(paginacaoPedidoDto.GetWhereByStatusPedido())
                .CustomFilterAsync(paginacaoPedidoDto);

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
            .Include(x => x.Usuario)
            .Include(x => x.ItensPedido)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        return await _parceiroContext.Pedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .AsQueryable()
            .Where(x => x.UsuarioId == usuarioId && x.StatusPedido == (StatusPedido)statusPedido)
            .ToListAsync();
    }
}

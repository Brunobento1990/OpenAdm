using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
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
}

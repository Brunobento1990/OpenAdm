using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(PaginacaoPedidoDto paginacaoPedidoDto);
    Task<Pedido?> GetPedidoByIdAsync(Guid id);
    Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id);
    Task<List<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido);
}

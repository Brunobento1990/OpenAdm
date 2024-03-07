using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(FilterModel<Pedido> filterModel);
    Task<Pedido?> GetPedidoByIdAsync(Guid id);
    Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id);
    Task<List<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido);
    Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId);
}

using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<Pedido?> GetPedidoByIdAsync(Guid id);
    Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id);
    Task<IList<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido);
    Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId);
    Task<decimal> GetTotalPedidoPorUsuarioAsync(Guid usuarioId);
    Task<int> GetQuantidadePorStatusUsuarioAsync(Guid usuarioId, StatusPedido statusPedido);
    Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto);
    Task<IList<Pedido>> GetPedidosEmAbertoAsync();
    Task<IDictionary<Guid, Pedido>> GetPedidosAsync(IList<Guid> ids);
    Task<int> GetCountPedidosEmAbertoAsync();
}

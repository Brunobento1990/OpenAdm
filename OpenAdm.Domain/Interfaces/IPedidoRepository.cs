using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface IPedidoRepository : IGenericRepository<Pedido>
{
    Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(FilterModel<Pedido> filterModel);
    Task<Pedido?> GetPedidoByIdAsync(Guid id);
    Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id);
    Task<List<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido);
    Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId);
    Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto);
}

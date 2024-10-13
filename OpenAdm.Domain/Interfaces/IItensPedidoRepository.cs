using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItensPedidoRepository : IGenericRepository<ItemPedido>
{
    Task<IList<ItemPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
    Task<IList<ItemPedido>> GetItensPedidoByProducaoAsync(IList<Guid> pedidosIds, IList<Guid> produtosIds, IList<Guid> pesosIds, IList<Guid> tamanhosIds);
    Task<ItemPedido?> GetItemPedidoByIdAsync(Guid id);
}

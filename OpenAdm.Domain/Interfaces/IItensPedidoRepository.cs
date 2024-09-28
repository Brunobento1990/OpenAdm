using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItensPedidoRepository : IGenericRepository<ItemPedido>
{
    Task<IList<ItemPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
    Task<ItemPedido?> GetItemPedidoByIdAsync(Guid id);
}

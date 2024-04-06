using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItensPedidoRepository : IGenericRepository<ItensPedido>
{
    Task<IList<ItensPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
    Task<ItensPedido?> GetItemPedidoByIdAsync(Guid id);
}

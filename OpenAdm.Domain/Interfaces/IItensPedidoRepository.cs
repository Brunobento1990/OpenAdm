using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItensPedidoRepository
{
    Task<IList<ItensPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
}

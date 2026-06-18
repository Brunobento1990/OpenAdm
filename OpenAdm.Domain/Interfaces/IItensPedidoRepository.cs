using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IItensPedidoRepository : IGenericRepository<ItemPedido>
{
    Task<IList<ItemPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);

    Task<IList<ItemPedido>> GetItensPedidoByProducaoAsync(IList<Guid> pedidosIds, IList<Guid> produtosIds,
        IList<Guid> pesosIds, IList<Guid> tamanhosIds);

    Task<IList<EstoqueReservadoModel>> ObterEstoquesReservadosAsync(
        IEnumerable<Guid> produtosIds);
    Task<ItemPedido?> GetItemPedidoByIdAsync(Guid id);
}
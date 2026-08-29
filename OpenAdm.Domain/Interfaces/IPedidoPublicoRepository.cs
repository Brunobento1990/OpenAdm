using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IPedidoPublicoRepository
{
    Task<Pedido?> GetPedidoCompletoByIdPublicoAsync(Guid idPublico);
}

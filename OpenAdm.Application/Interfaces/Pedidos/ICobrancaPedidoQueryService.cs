using OpenAdm.Application.Queries;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICobrancaPedidoQueryService
{
    Task<PedidoCobrancaQuery?> ObterAsync(Guid pedidoId, Guid usuarioId);
}
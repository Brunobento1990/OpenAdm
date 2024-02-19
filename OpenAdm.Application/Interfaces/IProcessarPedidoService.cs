using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces;

public interface IProcessarPedidoService
{
    Task ProcessarCreateAsync(Guid pedidoId);
    Task ProcessarProdutosMaisVendidosAsync(Pedido pedido);
}

using Domain.Pkg.Entities;

namespace OpenAdm.Application.Interfaces;

public interface IProcessarPedidoService
{
    Task ProcessarCreateAsync(Guid pedidoId);
    void ProcessarProdutosMaisVendidosAsync(Pedido pedido);
}

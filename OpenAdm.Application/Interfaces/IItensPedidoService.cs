using OpenAdm.Application.Models.Pedidos;

namespace OpenAdm.Application.Interfaces;

public interface IItensPedidoService
{
    Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
}

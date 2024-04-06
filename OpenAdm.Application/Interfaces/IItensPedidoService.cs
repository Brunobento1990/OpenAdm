using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Models.Pedidos;

namespace OpenAdm.Application.Interfaces;

public interface IItensPedidoService
{
    Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId);
    Task<ItensPedidoViewModel> EditarQuantidadeDoItemAsync(UpdateQuantidadeItemPedidoDto updateQuantidadeItemPedidoDto);
    Task<bool> DeleteItemPedidoAsync(Guid id);
}

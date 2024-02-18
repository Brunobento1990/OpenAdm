using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IPedidoService
{
    Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto);
    Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto);
    Task<bool> DeletePedidoAsync(Guid id);
    Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido);
    Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto);
}

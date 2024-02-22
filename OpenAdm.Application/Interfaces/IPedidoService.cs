using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.PaginateDto;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IPedidoService
{
    Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto);
    Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto);
    Task<bool> DeletePedidoAsync(Guid id);
    Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido);
    Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto);
    Task ReenviarPedidoViaEmailAsync(Guid pedidoId);
    Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId);
}

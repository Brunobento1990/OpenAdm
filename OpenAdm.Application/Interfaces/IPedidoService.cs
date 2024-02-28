using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IPedidoService
{
    Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto);
    Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto);
    Task<bool> DeletePedidoAsync(Guid id);
    Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId);
    Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto, Guid usuarioId);
    Task ReenviarPedidoViaEmailAsync(Guid pedidoId);
    Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId);
}

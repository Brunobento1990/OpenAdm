using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IPedidoService
{
    Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto);
    Task<IList<PedidoViewModel>> GetPedidosEmAbertAsync();
    Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId);
    Task<PedidoViewModel> GetAsync(Guid pedidoId);
    Task<byte[]> PedidoProducaoAsync(RelatorioProducaoDto relatorioProducaoDto);
}

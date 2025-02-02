using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IPedidoService
{
    Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(FilterModel<Pedido> paginacaoPedidoDto);
    Task<IList<PedidoViewModel>> GetPedidosEmAbertAsync();
    Task<IDictionary<Guid, PedidoViewModel>> GetPedidosAsync(IList<Guid> ids);
    Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId);
    Task<PedidoViewModel> GetAsync(Guid pedidoId);
    Task<PedidoViewModel> GetParaGerarPixAsync(Guid pedidoId);
    Task<byte[]> PedidoProducaoAsync(RelatorioProducaoDto relatorioProducaoDto);
}

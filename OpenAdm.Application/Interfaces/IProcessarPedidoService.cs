using OpenAdm.Application.Models.ConfiguracoesDePedidos;

namespace OpenAdm.Application.Interfaces;

public interface IProcessarPedidoService
{
    Task ProcessarCreateAsync(Guid pedidoId, ConfiguracoesDePedidoViewModel configuracoesDePedidoViewModel);
}

using OpenAdm.Application.Models.ConfiguracaoDoSistema;

namespace OpenAdm.Application.Interfaces;

public interface IProcessarPedidoService
{
    Task ProcessarCreateAsync(Guid pedidoId, ConfiguracaoDoSistemaViewModel configuracoesDePedidoViewModel);
}

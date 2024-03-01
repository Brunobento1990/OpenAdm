using OpenAdm.Application.Models.ConfiguracoesDePedidos;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracoesDePedidoService
{
    Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync();
}

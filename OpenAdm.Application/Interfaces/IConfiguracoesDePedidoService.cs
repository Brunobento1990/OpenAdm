using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracoesDePedidoService
{
    Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync();
    Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto);
    Task<PedidoMinimoViewModel> GetPedidoMinimoAsync();
}

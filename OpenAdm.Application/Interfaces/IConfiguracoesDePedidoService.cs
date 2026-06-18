using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracoesDePedidoService
{
    Task<ConfiguracoesDePedidoViewModel> GetConfiguracoesDePedidoAsync();
    Task<ConfiguracoesDePedido> ConfiguracaoDePedidoAsync();

    Task<ConfiguracoesDePedidoViewModel> CreateConfiguracoesDePedidoAsync(
        UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto);

    Task<PedidoMinimoViewModel> GetPedidoMinimoAsync();
    Task<TodasConfiguracoesViewModel> GetTodasConfiguracoesAsync();
}
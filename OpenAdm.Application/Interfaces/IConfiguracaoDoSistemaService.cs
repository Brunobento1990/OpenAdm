using OpenAdm.Application.Dtos.ConfiguracaoDoSistema;
using OpenAdm.Application.Models.ConfiguracaoDoSistema;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracaoDoSistemaService
{
    Task<ConfiguracaoDoSistemaViewModel> GetConfiguracaoDoSistemaAsync();
    Task<ConfiguracaoDoSistema> ConfiguracaoDoSistemaAsync();

    Task<ConfiguracaoDoSistemaViewModel> CreateConfiguracaoDoSistemaAsync(
        UpdateConfiguracaoDoSistemaDto updateConfiguracaoDoSistemaDto);

    Task<PedidoMinimoViewModel> GetPedidoMinimoAsync();
    Task<TodasConfiguracoesViewModel> GetTodasConfiguracoesAsync();
}

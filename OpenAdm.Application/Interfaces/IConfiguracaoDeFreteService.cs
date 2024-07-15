using OpenAdm.Application.Dtos.ConfiguracoesDeFrete;
using OpenAdm.Application.Models.ConfiguracoesDeFrete;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracaoDeFreteService
{
    Task<ConfiguracaoDeFreteViewModel> CreateOrUpdateAsync(ConfiguracaoDeFreteCreateDto configuracaoDeFreteCreateDto);
    Task<ConfiguracaoDeFreteViewModel> GetAsync();
}

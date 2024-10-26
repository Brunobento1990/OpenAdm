using OpenAdm.Application.Dtos.ConfiguracoesDeFretes;
using OpenAdm.Application.Models.ConfiguracoesDeFretes;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracaoDeFreteService
{
    Task<EfetuarCobrancaViewModel> CobrarFreteAsync();
    Task<ConfiguracaoDeFreteViewModel> GetAsync();
    Task<ConfiguracaoDeFreteViewModel> CreateOrUpdateAsync(ConfiguracaoDeFreteCreateOrUpdateDto configuracaoDeFreteCreateOrUpdateDto);
}

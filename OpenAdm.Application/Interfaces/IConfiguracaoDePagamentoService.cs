using OpenAdm.Application.Dtos.ConfiguracoesDePagamentos;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracaoDePagamentoService
{
    Task<EfetuarCobrancaViewModel> CobrarAsync();
    Task<ConfiguracaoDePagamentoViewModel?> GetAsync();
    Task<ConfiguracaoDePagamentoViewModel> CreateOrUpdateAsync(ConfiguracaoDePagamentoCreateOrUpdate configuracaoDePagamentoCreateOrUpdate);
}

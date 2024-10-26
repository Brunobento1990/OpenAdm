using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDeFreteRepository
{
    Task<ConfiguracaoDeFrete?> GetAsync();
    Task AddAsync(ConfiguracaoDeFrete configuracaoDeFrete);
    Task UpdateAsync(ConfiguracaoDeFrete configuracaoDeFrete);
}

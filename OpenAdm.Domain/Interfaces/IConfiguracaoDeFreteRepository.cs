using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDeFreteRepository : IGenericBaseRepository<ConfiguracaoDeFrete>
{
    Task<ConfiguracaoDeFrete?> ObterDoParceiroAsync(Guid parceiroId);
}
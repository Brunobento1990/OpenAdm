using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDoSistemaRepository : IGenericBaseRepository<ConfiguracaoDoSistema>
{
    Task<ConfiguracaoDoSistema?> GetConfiguracaoDoSistemaAsync(Guid parceiroId);
}

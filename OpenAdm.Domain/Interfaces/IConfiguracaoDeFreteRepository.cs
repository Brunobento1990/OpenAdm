using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDeFreteRepository : IGenericRepository<ConfiguracaoDeFrete>
{
    Task<ConfiguracaoDeFrete?> GetConfiguracaoAsync();
}

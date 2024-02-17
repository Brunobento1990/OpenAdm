using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDeEmailRepository : IGenericRepository<ConfiguracaoDeEmail>
{
    Task<ConfiguracaoDeEmail?> GetConfiguracaoDeEmailAtivaAsync();
}

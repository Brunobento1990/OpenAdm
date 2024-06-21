using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<string> GetConexaoDbByDominioAsync();
    Task<ConfiguracaoParceiro?> GetConexaoDbByXApiAsync(Guid xApi);
}

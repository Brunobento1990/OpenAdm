using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio);
    Task<ConfiguracaoParceiro?> GetParceiroByXApiAsync(Guid xApi);
}

using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio);
}

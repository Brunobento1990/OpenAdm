using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILoginRepresentanteRepository
{
    Task<Representante?> ObterPorEmailAsync(string email);
}

using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IRepresentanteRepository : IGenericRepository<Representante>
{
    Task<Representante?> ObterPorIdAsync(Guid id, bool tracking = false);
    Task<(bool CpfDuplicado, bool EmailDuplicado)> ObterDuplicidadesAsync(
        string? cpf, string? email, Guid? ignorarId = null);
}

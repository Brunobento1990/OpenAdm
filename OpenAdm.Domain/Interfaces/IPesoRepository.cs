using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IPesoRepository
{
    Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids);
}

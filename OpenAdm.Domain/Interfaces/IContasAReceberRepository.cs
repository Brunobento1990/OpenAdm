using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IContasAReceberRepository : IGenericRepository<ContasAReceber>
{
    Task<ContasAReceber?> GetByIdAsync(Guid id);
}

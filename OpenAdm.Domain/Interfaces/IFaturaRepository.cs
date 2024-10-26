using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IFaturaRepository : IGenericRepository<Fatura>
{
    Task<Fatura?> GetByIdAsync(Guid id);
}

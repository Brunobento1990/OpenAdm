using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILojasParceirasRepository : IGenericBaseRepository<LojaParceira>
{
    Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id);
    Task<IList<LojaParceira>> GetLojasParceirasAsync(Guid parceiroId);
    Task<IList<string?>> GetFotosLojasParceirasAsync(Guid parceiroId);
}

using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ILojasParceirasRepository : IGenericRepository<LojaParceira>
{
    Task<PaginacaoViewModel<LojaParceira>> GetPaginacaoLojasParceirasAsync(FilterModel<LojaParceira> filterModel);
    Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id);
    Task<IList<string?>> GetFotosLojasParceirasAsync();
}

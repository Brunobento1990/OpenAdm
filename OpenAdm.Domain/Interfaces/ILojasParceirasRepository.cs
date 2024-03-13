using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ILojasParceirasRepository : IGenericRepository<LojasParceiras>
{
    Task<PaginacaoViewModel<LojasParceiras>> GetPaginacaoLojasParceirasAsync(FilterModel<LojasParceiras> filterModel);
    Task<LojasParceiras?> GetLojaParceiraByIdAsync(Guid id);
    Task<IList<string?>> GetFotosLojasParceirasAsync();
}

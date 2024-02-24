using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhoRepository : IGenericRepository<Tamanho>
{
    Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids);
    Task<PaginacaoViewModel<Tamanho>> GetPaginacaoTamanhoAsync(FilterModel<Tamanho> filterModel);
    Task<Tamanho?> GetTamanhoByIdAsync(Guid id);
}

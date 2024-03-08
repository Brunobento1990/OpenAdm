using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IPesoRepository : IGenericRepository<Peso>
{
    Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids);
    Task<IList<Peso>> GetPesosAsync();
    Task<PaginacaoViewModel<Peso>> GetPaginacaoPesoAsync(FilterModel<Peso> filterModel);
    Task<Peso?> GetPesoByIdAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids);
}

using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IPesoRepository : IGenericRepository<Peso>
{
    Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro);
    Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids);
    Task<IList<Peso>> GetPesosAsync();
    Task<Peso?> GetPesoByIdAsync(Guid id);
    Task<Peso?> GetPesoByIdAsNoTrackingAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids);
    Task<IDictionary<Guid, Peso>> GetDictionaryPesosByIdsAsync(IList<Guid> ids);
}

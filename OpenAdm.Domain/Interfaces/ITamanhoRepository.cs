using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhoRepository : IGenericRepository<Tamanho>
{
    Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro);
    Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids);
    Task<IList<Tamanho>> GetTamanhosAsync();
    Task<Tamanho?> GetTamanhoByIdAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids);
    Task<IDictionary<Guid, Tamanho>> GetDictionaryTamanhosAsync(IList<Guid> ids);
}

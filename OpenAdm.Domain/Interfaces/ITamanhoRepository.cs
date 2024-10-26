using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhoRepository : IGenericRepository<Tamanho>
{
    Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids);
    Task<IList<Tamanho>> GetTamanhosAsync();
    Task<Tamanho?> GetTamanhoByIdAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids);
    Task<IDictionary<Guid, Tamanho>> GetDictionaryTamanhosAsync(IList<Guid> ids);
}

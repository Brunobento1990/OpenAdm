using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhoRepository
{
    Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids);
}

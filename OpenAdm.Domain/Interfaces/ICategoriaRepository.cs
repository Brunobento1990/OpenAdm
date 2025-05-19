using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IList<Categoria>> GetCategoriasAsync();
    Task<IList<Categoria>> GetCategoriasDropDownAsync();
    Task<Categoria?> GetCategoriaAsync(Guid id);
}

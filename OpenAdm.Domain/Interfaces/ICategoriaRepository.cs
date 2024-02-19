using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IList<Categoria>> GetCategoriasAsync();
}

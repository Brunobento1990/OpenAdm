using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IList<Categoria>> GetCategoriasAsync();
    Task<Categoria?> GetCategoriaAsync(Guid id);
    Task<PaginacaoViewModel<Categoria>> GetPaginacaoCategoriaAsync(FilterModel<Categoria> filterModel);
}

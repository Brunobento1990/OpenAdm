using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IList<Categoria>> GetCategoriasAsync();
    Task<PaginacaoViewModel<Categoria>> GetPaginacaoCategoriaAsync(PaginacaoCategoriaDto paginacaoCategoriaDto);
}

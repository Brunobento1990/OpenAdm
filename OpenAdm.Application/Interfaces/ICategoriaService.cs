using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
    Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(PaginacaoCategoriaDto paginacaoCategoriaDto);
}

using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.PaginateDto;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
    Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(PaginacaoCategoriaDto paginacaoCategoriaDto);
}

using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
}

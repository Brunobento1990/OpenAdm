using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
    Task<CategoriaViewModel> GetCategoriaAsync(Guid id);
    Task DeleteCategoriaAsync(Guid id);
    Task<CategoriaViewModel> UpdateCategoriaAsync(UpdateCategoriaDto updateCategoriaDto);
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
    Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(PaginacaoCategoriaDto paginacaoCategoriaDto);
}

using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<CategoriaViewModel> GetCategoriaAsync(Guid id);
    Task DeleteCategoriaAsync(Guid id);
    Task InativarAtivarEcommerceAsync(Guid id);
    Task<CategoriaViewModel> UpdateCategoriaAsync(UpdateCategoriaDto updateCategoriaDto);
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
    Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(FilterModel<Categoria> paginacaoCategoriaDto);
}

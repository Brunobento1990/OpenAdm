﻿using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ICategoriaService
{
    Task<IList<CategoriaViewModel>> GetCategoriasAsync();
    Task<IList<CategoriaViewModel>> GetCategoriasDropDownAsync();
    Task<CategoriaViewModel> GetCategoriaAsync(Guid id);
    Task DeleteCategoriaAsync(Guid id);
    Task InativarAtivarEcommerceAsync(Guid id);
    Task<CategoriaViewModel> UpdateCategoriaAsync(UpdateCategoriaDto updateCategoriaDto);
    Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto);
    Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(FilterModel<Categoria> paginacaoCategoriaDto);
}

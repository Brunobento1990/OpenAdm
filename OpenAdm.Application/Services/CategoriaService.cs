using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto)
    {
        var categoria = categoriaCreateDto.ToEntity();

        await _categoriaRepository.AddAsync(categoria);

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task<IList<CategoriaViewModel>> GetCategoriasAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasAsync();

        return categorias.Select(x => new CategoriaViewModel().ToModel(x)).ToList();
    }

    public async Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        var paginacao = await _categoriaRepository.GetPaginacaoCategoriaAsync(paginacaoCategoriaDto);

        return new PaginacaoViewModel<CategoriaViewModel>
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new CategoriaViewModel().ToModel(x)).ToList()
        };
    }
}

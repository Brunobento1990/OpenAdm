using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class CategoriaCached(CategoriaRepository categoriaRepository,
    ICachedService<Categoria> cachedService) : ICategoriaRepository
{
    private readonly CategoriaRepository _categoriaRepository = categoriaRepository;
    private readonly ICachedService<Categoria> _cachedService = cachedService;
    private const string _keyList = "categorias";

    public async Task<IList<Categoria>> GetCategoriasAsync()
    {
        var categorias = await _cachedService.GetListItemAsync(_keyList);

        if(categorias == null)
        {
            categorias = await _categoriaRepository.GetCategoriasAsync();

            if(categorias.Count > 0)
            {
                await _cachedService.SetListItemAsync(_keyList, categorias);
            }
        }

        return categorias;
    }

    public async Task<Categoria> AddAsync(Categoria entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _categoriaRepository.AddAsync(entity);
    }

    public async Task<Categoria> UpdateAsync(Categoria entity)
    {
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _categoriaRepository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Categoria entity)
    {
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _categoriaRepository.DeleteAsync(entity);
    }

    public async Task<PaginacaoViewModel<Categoria>> GetPaginacaoCategoriaAsync(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        return await _categoriaRepository.GetPaginacaoCategoriaAsync(paginacaoCategoriaDto);
    }
}

using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class CategoriaCached(
    ParceiroContext parceiroContext,
    CategoriaRepository categoriaRepository,
    ICachedService<Categoria> cachedService) : GenericRepository<Categoria>(parceiroContext), ICategoriaRepository
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
}

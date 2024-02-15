using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class CategoriaRepository(ParceiroContext parceiroContext) 
    : GenericRepository<Categoria>(parceiroContext), ICategoriaRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<IList<Categoria>> GetCategoriasAsync()
    {
        return await _parceiroContext
            .Categorias
            .AsNoTracking()
            .ToListAsync();
    }
}

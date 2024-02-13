using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class GenericRepository<T>(ParceiroContext parceiroContext)
    : IGenericRepository<T> where T : class
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<T> AddAsync(T entity)
    {
        await _parceiroContext.AddAsync(entity);
        await _parceiroContext.SaveChangesAsync();
        return entity;
    }
}

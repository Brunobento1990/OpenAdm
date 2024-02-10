using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class GenericRepository<T>(IParceiroContextFactory parceiroContextFactory)
    : IGenericRepository<T> where T : class
{
    private readonly IParceiroContextFactory _parceiroContextFactory = parceiroContextFactory;

    public async Task<T> AddAsync(T entity)
    {
        var context = await _parceiroContextFactory.CreateParceiroContextAsync();
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}

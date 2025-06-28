using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class GenericBaseRepository<T> : IGenericBaseRepository<T> where T : class
{
    protected readonly AppDbContext AppDbContext;

    public GenericBaseRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    public async Task AddAsync(T entity)
    {
        await AppDbContext.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await AppDbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        AppDbContext.Update(entity);
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class GenericBaseRepository<T> : IGenericBaseRepository<T> where T : BaseEntityParceiro
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

    public void Delete(T entity)
    {
        AppDbContext.Remove(entity);
    }

    public async Task<long> ProximoNumeroAsync(Guid parceiroId)
    {
        try
        {
            return await AppDbContext
                .Set<T>()
                .Where(x => x.ParceiroId == parceiroId)
                .MaxAsync(x => x.Numero) + 1;
        }
        catch (Exception)
        {
            return 1;
        }
    }

    public async Task SaveChangesAsync()
    {
        await AppDbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        AppDbContext.Update(entity);
    }

    public virtual async Task<PaginacaoViewModel<T>> PaginacaoAsync(FilterModel<T> filterModel)
    {
        var include = filterModel.IncludeCustom();
        var select = filterModel.SelectCustom();

        var query = AppDbContext
            .Set<T>()
            .AsNoTracking()
            .Where(x => x.ParceiroId == filterModel.ParceiroId)
            .WhereIsNotNull(filterModel.GetWhereBySearch());

        if (include != null)
        {
            query = query.Include(include);
        }
        ;

        if (select != null)
        {
            query = query.Select(select);
        }

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel);

        var totalDeRegistros = await AppDbContext.Set<T>().Where(x => x.ParceiroId == filterModel.ParceiroId).WhereIsNotNull(filterModel.GetWhereBySearch()).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
    }
}

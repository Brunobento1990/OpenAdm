using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.Extensions.IQueryable;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Repositories;

public class GenericRepository<T>(ParceiroContext parceiroContext)
    : IGenericRepository<T> where T : class
{
    protected readonly ParceiroContext ParceiroContext = parceiroContext;

    public virtual async Task<T> AddAsync(T entity)
    {
        await ParceiroContext.AddAsync(entity);
        await ParceiroContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> AdicionarAsync(T entity)
    {
        await ParceiroContext.AddAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            ParceiroContext.Remove(entity);
            return await ParceiroContext.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {

            if (ex.InnerException != null && ex.InnerException.Message.Contains("violates foreign key constraint"))
            {
                throw new ExceptionApi("Este registro contém dependências, e não pode ser excluido!");
            }

            throw;
        }
    }

    public virtual async Task<PaginacaoViewModel<T>> PaginacaoAsync(FilterModel<T> filterModel)
    {
        var include = filterModel.IncludeCustom();
        var select = filterModel.SelectCustom();

        var query = ParceiroContext
            .Set<T>()
            .AsNoTracking()
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

        var totalDeRegistros = await ParceiroContext.Set<T>().WhereIsNotNull(filterModel.GetWhereBySearch()).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
    }

    public async Task<IList<T>> PaginacaoDropDownAsync(PaginacaoDropDown<T> paginacaoDropDown)
    {
        return await ParceiroContext
            .Set<T>()
            .AsNoTracking()
            .OrderBy(x => EF.Property<T>(x, paginacaoDropDown.OrderBy))
            .WhereIsNotNull(paginacaoDropDown.Where())
            .Skip(0)
            .Take(50)
            .ToListAsync();
    }

    public Task<int> SaveChangesAsync()
        => ParceiroContext.SaveChangesAsync();

    public void Update(T entity)
    {
        ParceiroContext.Update(entity);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        ParceiroContext.Attach(entity);
        ParceiroContext.Update(entity);
        await ParceiroContext.SaveChangesAsync();
        return entity;
    }
}

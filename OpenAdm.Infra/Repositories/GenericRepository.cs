using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using OpenAdm.Domain.PaginateDto;
using System.Linq.Expressions;

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
        var includes = filterModel.IncludeCustomList();
        var select = filterModel.SelectCustom();
        var where = filterModel.Where();

        var queryBase = ParceiroContext
            .Set<T>()
            .AsNoTracking()
            .WhereIsNotNull(filterModel.GetWhereBySearch())
            .WhereIsNotNull(where);

        var totalDeRegistros = await queryBase.CountAsync();
        var query = queryBase.AsSplitQuery();

        if (include != null)
        {
            query = query.Include(include);
        }

        if (includes?.Count > 0)
        {
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
        }

        if (select != null)
        {
            query = query.Select(select);
        }

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel, totalDeRegistros);

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

    protected async Task<IList<TResult>> BuscarDropDownAsync<TResult>(
        DropDownFiltro filtro,
        Expression<Func<T, bool>>? where,
        Expression<Func<T, string>> orderBy,
        Expression<Func<T, Guid>> thenBy,
        Expression<Func<T, TResult>> select)
    {
        return await ParceiroContext
            .Set<T>()
            .AsNoTracking()
            .WhereIsNotNull(where)
            .OrderBy(orderBy)
            .ThenBy(thenBy)
            .Skip(filtro.Skip)
            .Take(filtro.Take)
            .Select(select)
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

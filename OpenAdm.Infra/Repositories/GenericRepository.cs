using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class GenericRepository<T>(ParceiroContext parceiroContext)
    : IGenericRepository<T> where T : class
{
    protected readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<T> AddAsync(T entity)
    {
        await _parceiroContext.AddAsync(entity);
        await _parceiroContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _parceiroContext.Remove(entity);
            return await _parceiroContext.SaveChangesAsync() > 0;
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

        var query = _parceiroContext
            .Set<T>()
            .AsNoTracking()
            .WhereIsNotNull(filterModel.GetWhereBySearch());

        if (include != null)
        {
            query = query.Include(include);
        };

        if (select != null)
        {
            query = query.Select(select);
        }

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel);

        var totalDeRegistros = await _parceiroContext.Set<T>().WhereIsNotNull(filterModel.GetWhereBySearch()).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _parceiroContext.Attach(entity);
        _parceiroContext.Update(entity);
        await _parceiroContext.SaveChangesAsync();
        return entity;
    }
}

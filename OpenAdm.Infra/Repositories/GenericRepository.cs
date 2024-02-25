using Domain.Pkg.Exceptions;
using Npgsql;
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

    public async Task<T> UpdateAsync(T entity)
    {
        _parceiroContext.Attach(entity);
        _parceiroContext.Update(entity);
        await _parceiroContext.SaveChangesAsync();
        return entity;
    }
}

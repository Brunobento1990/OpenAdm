using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<PaginacaoViewModel<T>> PaginacaoAsync(FilterModel<T> filterModel);
}

using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> AdicionarAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<PaginacaoViewModel<T>> PaginacaoAsync(FilterModel<T> filterModel);
    Task<IList<T>> PaginacaoDropDownAsync(PaginacaoDropDown<T> paginacaoDropDown);
    Task<int> SaveChangesAsync();
}

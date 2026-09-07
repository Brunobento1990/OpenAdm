using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IGenericBaseRepository<T> where T : BaseEntityParceiro
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<long> ProximoNumeroAsync(Guid parceiroId);
    Task SaveChangesAsync();
    Task<PaginacaoViewModel<T>> PaginacaoAsync(FilterModel<T> filterModel);
}

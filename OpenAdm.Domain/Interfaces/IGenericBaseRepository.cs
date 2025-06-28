namespace OpenAdm.Domain.Interfaces;

public interface IGenericBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    void Update(T entity);
    Task SaveChangesAsync();
}

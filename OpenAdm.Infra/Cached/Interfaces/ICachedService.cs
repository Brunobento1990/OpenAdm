namespace OpenAdm.Infra.Cached.Interfaces;

public interface ICachedService<T> where T : class
{
    Task<T?> GetItemAsync(string key);
    Task<IList<T>?> GetListItemAsync(string key);
    Task SetListItemAsync(string key, IList<T> itens);
    Task SetItemAsync(string key, T item);
    Task RemoveCachedAsync(string key);
}

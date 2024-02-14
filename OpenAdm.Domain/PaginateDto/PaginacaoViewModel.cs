namespace OpenAdm.Domain.Model;

public class PaginacaoViewModel<T> where T : class
{
    public IList<T> Values { get; set; } = new List<T>();
    public int TotalPage { get; set; }
}

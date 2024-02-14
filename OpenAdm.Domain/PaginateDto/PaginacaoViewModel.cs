namespace OpenAdm.Domain.Model;

public class PaginacaoViewModel<T> where T : class
{
    public IEnumerable<T> Values { get; set; }
    public int TotalPage { get; set; }
}

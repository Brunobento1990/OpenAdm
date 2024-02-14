namespace OpenAdm.Domain.Model;

public class PaginacaoViewModel<T> where T : class
{
    public List<T> Values { get; set; } = new();
    public int TotalPage { get; set; }
}

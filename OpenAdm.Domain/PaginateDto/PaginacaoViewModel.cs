namespace OpenAdm.Domain.Model;

public class PaginacaoViewModel<T> where T : class
{
    public IList<T> Values { get; set; } = [];
    public int TotalPaginas { get; set; }
    public int TotalDeRegistros { get; set; }
}

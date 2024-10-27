using System.Linq.Expressions;

namespace OpenAdm.Domain.PaginateDto;

public abstract class PaginacaoDropDown<T> where T : class
{
    public string? Search { get; set; }
    public string OrderBy { get; set; } = "DataDeAtualizacao";

    public abstract Expression<Func<T, bool>>? Where();
}

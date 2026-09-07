using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.PaginateDto;

public class DropDownFiltro
{
    public string? Search { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 50;
}

namespace OpenAdm.Domain.Model;

public abstract class FilterModel
{
    public string? Search { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public string OrderBy { get; set; } = "DataDeCriacao";
    public bool Asc { get; set; } = false;
}

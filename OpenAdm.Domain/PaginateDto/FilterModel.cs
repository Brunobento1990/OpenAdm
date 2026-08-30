using System.Linq.Expressions;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Model;

public abstract class FilterModel<T>
{
    public const int TakeMaximo = 100;

    public bool ListarInativo { get; set; } = false;
    public string? Search { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public string OrderBy { get; set; } = "Numero";
    public bool Asc { get; set; } = false;
    public Guid ParceiroId { get; set; }
    public abstract Expression<Func<T, bool>>? GetWhereBySearch();

    public virtual Expression<Func<T, object>>? IncludeCustom()
    {
        return null;
    }

    public virtual IList<Expression<Func<T, object>>>? IncludeCustomList()
    {
        return null;
    }

    public virtual Expression<Func<T, bool>>? Where()
    {
        return null;
    }

    public virtual Expression<Func<T, T>>? SelectCustom()
    {
        return null;
    }

    public string ValidarEObterPropriedadeDeOrdenacao()
    {
        if (Skip < 0)
            throw new ExceptionApi("Skip deve ser maior ou igual a zero.");

        if (Take is <= 0 or > TakeMaximo)
            throw new ExceptionApi($"Take deve estar entre 1 e {TakeMaximo}.");

        if (string.IsNullOrWhiteSpace(OrderBy))
            throw new ExceptionApi("Informe a propriedade de ordenação.");

        var propriedade = typeof(T).GetProperties()
            .FirstOrDefault(x => string.Equals(x.Name, OrderBy.Trim(), StringComparison.OrdinalIgnoreCase));

        if (propriedade == null)
            throw new ExceptionApi($"A propriedade '{OrderBy}' não é válida para ordenação.");

        return propriedade.Name;
    }
}

﻿using System.Linq.Expressions;

namespace OpenAdm.Domain.Model;

public abstract class FilterModel<T>
{
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
    public virtual Expression<Func<T, T>>? SelectCustom()
    {
        return null;
    }
}

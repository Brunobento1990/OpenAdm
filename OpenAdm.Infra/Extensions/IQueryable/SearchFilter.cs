using System.Linq.Expressions;

namespace OpenAdm.Infra.Extensions.IQueryable;

public static class SearchFilter
{
    public static IQueryable<TEntity> WhereIsNotNull<TEntity>(this IQueryable<TEntity> querable, Expression<Func<TEntity, bool>>? where)
    {
        if (where == null)
            return querable;

        return querable.Where(where);
    }
}

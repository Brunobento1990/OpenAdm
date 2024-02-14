namespace OpenAdm.Infra.Extensions.IQueryable;

public static class PaginateFilter
{
    public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> querable, int skip, int take)
    {
        return querable
            .Skip(skip * take)
            .Take(take);
    }
}

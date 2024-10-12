namespace OpenAdm.Infra.Extensions.IQueryable;

public static class PaginateFilter
{
    public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> querable, int skip, int take)
    {
        var newSkip = skip - 1;
        if (newSkip < 0)
        {
            newSkip = 0;
        }
        return querable
            .Skip(newSkip * take)
            .Take(take);
    }
}

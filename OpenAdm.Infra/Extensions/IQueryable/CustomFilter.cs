using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;

namespace OpenAdm.Infra.Extensions.IQueryable;

public static class CustomFilter
{
    public static async Task<(int TotalPages, IQueryable<TEntity> Values)> CustomFilterAsync<TEntity>(this IQueryable<TEntity> querable, FilterModel filterModel)
    {
        var total = await querable
            .CountCustomAsync(filterModel.Take);

        var values = querable
            .Paginate(filterModel.Skip, filterModel.Take).AsQueryable();

        return (total, values);
    }

    private static async Task<int> CountCustomAsync<TEntity>(this IQueryable<TEntity> querable, int take)
    {
        var count = await querable
            .CountAsync();

        return (int)Math.Ceiling((decimal)count / take);
    }
}

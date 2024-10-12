using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;

namespace OpenAdm.Infra.Extensions.IQueryable;

public static class CustomFilter
{
    public static async Task<(int TotalPaginas, IList<TEntity> Values)> CustomFilterAsync<TEntity>(this IQueryable<TEntity> querable, FilterModel<TEntity> filterModel)
    {
        var total = await querable
            .CountCustomAsync(filterModel.Take);

        var coluna = filterModel.OrderBy[..1].ToUpper() + filterModel.OrderBy[1..];

        querable = filterModel.Asc ? querable.OrderBy(x => EF.Property<TEntity>(x!, coluna))
            : querable.OrderByDescending(x => EF.Property<TEntity>(x!, coluna));

        var values = await querable
            .Paginate(filterModel.Skip, filterModel.Take)
            .ToListAsync();

        return (total, values);
    }

    private static async Task<int> CountCustomAsync<TEntity>(this IQueryable<TEntity> querable, int take)
    {
        var count = await querable
            .CountAsync();

        return (int)Math.Ceiling((decimal)count / take);
    }
}

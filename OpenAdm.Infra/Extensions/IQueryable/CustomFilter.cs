using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;

namespace OpenAdm.Infra.Extensions.IQueryable;

public static class CustomFilter
{
    public static async Task<(int TotalPaginas, IList<TEntity> Values)> CustomFilterAsync<TEntity>(this IQueryable<TEntity> querable, FilterModel<TEntity> filterModel)
    {
        var totalDeRegistros = await querable.CountAsync();
        return await querable.CustomFilterAsync(filterModel, totalDeRegistros);
    }

    public static async Task<(int TotalPaginas, IList<TEntity> Values)> CustomFilterAsync<TEntity>(
        this IQueryable<TEntity> querable,
        FilterModel<TEntity> filterModel,
        int totalDeRegistros)
    {
        var coluna = filterModel.ValidarEObterPropriedadeDeOrdenacao();
        var totalPaginas = CalcularTotalDePaginas(totalDeRegistros, filterModel.Take);

        querable = filterModel.Asc ? querable.OrderBy(x => EF.Property<TEntity>(x!, coluna))
            : querable.OrderByDescending(x => EF.Property<TEntity>(x!, coluna));

        var values = await querable
            .Paginate(filterModel.Skip, filterModel.Take)
            .ToListAsync();

        return (totalPaginas, values);
    }

    private static int CalcularTotalDePaginas(int totalDeRegistros, int take)
        => (int)Math.Ceiling((decimal)totalDeRegistros / take);

    public static async Task<int> CountCustomAsync<TEntity>(this IQueryable<TEntity> querable, int take)
    {
        if (take <= 0)
            throw new ArgumentOutOfRangeException(nameof(take), "Take deve ser maior que zero.");

        var count = await querable.CountAsync();
        return CalcularTotalDePaginas(count, take);
    }
}

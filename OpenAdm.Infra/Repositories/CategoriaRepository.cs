using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class CategoriaRepository(ParceiroContext parceiroContext)
    : GenericRepository<Categoria>(parceiroContext), ICategoriaRepository
{
    public async Task<Categoria?> GetCategoriaAsync(Guid id)
    {
        return await _parceiroContext
            .Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Categoria>> GetCategoriasAsync()
    {
        var categorias = await _parceiroContext
            .Categorias
            .AsQueryable()
            .AsNoTracking()
            .OrderByDescending(c => c.Numero)
            .Include(x => x.Produtos)
            .ToListAsync();

        foreach (var categoria in categorias)
        {
            categoria.Produtos = categoria
                .Produtos
                .Select(x =>
                    new Produto(
                        x.Id,
                        x.DataDeCriacao,
                        x.DataDeAtualizacao,
                        x.Numero,
                        x.Descricao,
                        x.EspecificacaoTecnica,
                        x.CategoriaId,
                        x.Referencia,
                        x.UrlFoto,
                        x.NomeFoto,
                        x.Peso))
                .OrderByDescending(x => x.Numero)
                .Take(3)
                .ToList();
        }

        return categorias;
    }

    public async Task<PaginacaoViewModel<Categoria>> GetPaginacaoCategoriaAsync(FilterModel<Categoria> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Categorias
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Categoria>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}

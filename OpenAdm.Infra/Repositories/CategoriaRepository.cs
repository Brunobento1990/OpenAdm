using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class CategoriaRepository(ParceiroContext parceiroContext) 
    : GenericRepository<Categoria>(parceiroContext), ICategoriaRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

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
                        x.Foto,
                        x.CategoriaId,
                        x.Referencia))
                .OrderByDescending(x => x.Numero)
                .Take(3)
                .ToList();
        }

        return categorias;
    }

    public async Task<PaginacaoViewModel<Categoria>> GetPaginacaoCategoriaAsync(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        var (total, values) = await _parceiroContext
                .Categorias
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Categoria>(x, paginacaoCategoriaDto.OrderBy))
                .WhereIsNotNull(paginacaoCategoriaDto.GetWhereBySearch())
                .CustomFilterAsync(paginacaoCategoriaDto);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}

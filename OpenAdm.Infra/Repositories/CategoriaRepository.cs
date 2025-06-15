using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class CategoriaRepository(ParceiroContext parceiroContext)
    : GenericRepository<Categoria>(parceiroContext), ICategoriaRepository
{
    public async Task<Categoria?> GetCategoriaAsync(Guid id)
    {
        return await ParceiroContext
            .Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Categoria>> GetCategoriasAsync()
    {
        var categorias = await ParceiroContext
            .Categorias
            .AsQueryable()
            .AsNoTracking()
            .OrderBy(c => c.Numero)
            .Include(x => x.Produtos.Where(x => !x.InativoEcommerce))
            .Where(x => !x.InativoEcommerce && x.Produtos.Where(x => !x.InativoEcommerce).Count() > 0)
            .ToListAsync();

        foreach (var categoria in categorias)
        {
            categoria.Produtos = [.. categoria
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
                        x.InativoEcommerce))
                .OrderBy(x => x.Numero)
                .Take(3)];
        }

        return categorias;
    }

    public async Task<IList<Categoria>> GetCategoriasDropDownAsync()
    {
        return await ParceiroContext
            .Categorias
            .AsNoTracking()
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Infra.Repositories;

public class ProdutoEcommerceRepository : IProdutoEcommerceRepository
{
    private const int Take = 10;
    private readonly ParceiroContext _context;

    public ProdutoEcommerceRepository(ParceiroContext context)
    {
        _context = context;
    }

    public async Task<PaginacaoViewModel<Produto>> ListarAsync(
        string? search,
        int page,
        ICollection<Guid>? categoriasIds)
    {
        var query = _context
            .Produtos
            .AsNoTracking()
            .Include(x => x.Tamanhos)
            .Include(x => x.Pesos)
            .Where(x => !x.InativoEcommerce)
            .AsQueryable();

        if (categoriasIds?.Count > 0)
        {
            query = query.Where(x => categoriasIds.Contains(x.CategoriaId));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.RemoverAcentos();

            query = query.Where(x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{search}%"));
        }

        var count = await query.CountAsync();
        var dados = await query
            .OrderBy(x => x.Referencia)
            .Skip((page - 1) * Take)
            .Take(Take)
            .ToListAsync();

        return new PaginacaoViewModel<Produto>()
        {
            Values = dados,
            TotalPaginas = (int)Math.Ceiling((decimal)count / Take)
        };
    }
}
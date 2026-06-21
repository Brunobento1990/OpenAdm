using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
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

    public async Task<ResultadoProdutoEcommerceModel> ListarAsync(
        string? search,
        int page,
        ICollection<Guid>? categoriasIds)
    {
        var query = _context
            .Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(x => x.Tamanhos)
            .Include(x => x.Pesos)
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
            .Select(x => new ProdutoEcommerceModel()
            {
                Categoria = x.Categoria.Descricao,
                Descricao = x.Descricao,
                Numero = x.Numero,
                Id = x.Id,
                Foto = x.UrlFoto!,
                Pesos = x.Pesos.Select(y => new PesoTamanhoEcommerceModel()
                {
                    Descricao = y.Descricao,
                    Id = y.Id
                }).ToList(),
                Tamanhos = x.Tamanhos.Select(y => new PesoTamanhoEcommerceModel()
                {
                    Descricao = y.Descricao,
                    Id = y.Id
                }).ToList(),
            })
            .Skip((page - 1) * Take)
            .Take(Take)
            .ToListAsync();

        return new ResultadoProdutoEcommerceModel()
        {
            Produtos = dados,
            QuantidadeDePagina = (int)Math.Ceiling((decimal)count / Take)
        };
    }
}
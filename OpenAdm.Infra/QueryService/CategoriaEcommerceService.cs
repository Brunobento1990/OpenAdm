using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Application.Queries;
using OpenAdm.Data.Context;

namespace OpenAdm.Infra.QueryService;

public class CategoriaEcommerceService : ICategoriaEcommerceService
{
    private readonly ParceiroContext _context;

    public CategoriaEcommerceService(ParceiroContext context)
    {
        _context = context;
    }

    public async Task<ICollection<CategoriaEcommerceQuery>> ListarTodasAsync()
    {
        return await _context
            .Categorias
            .AsNoTracking()
            .Where(x => !x.InativoEcommerce)
            .Select(x => new CategoriaEcommerceQuery()
            {
                Descricao = x.Descricao,
                Id = x.Id
            }).ToListAsync();
    }

    public async Task<ICollection<CategoriaEcommerceHomeQuery>> ListarHomeAsync()
    {
        return await _context
            .Categorias
            .AsNoTracking()
            .Where(x => !x.InativoEcommerce)
            .Select(x => new CategoriaEcommerceHomeQuery()
            {
                Descricao = x.Descricao,
                Id = x.Id,
                Produtos = x.Produtos
                    .OrderBy(y => y.DataDeCriacao)
                    .Take(3)
                    .Select(y => new ProdutoCategoriaEcommerceHomeQuery()
                    {
                        Foto = y.UrlFoto!,
                        Descricao = y.Descricao,
                        Id = y.Id
                    })
                    .ToList()
            })
            .ToListAsync();
    }
}
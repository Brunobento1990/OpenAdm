using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using Domain.Pkg.Entities;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Repositories;

public class ProdutoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Produto>(parceiroContext), IProdutoRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;
    private const int _take = 5;

    public async Task<PaginacaoViewModel<Produto>> GetProdutosAsync(int page, Guid? categoriaId)
    {
        var newPage = page == 0 ? page : page - 1;

        Expression<Func<Produto, bool>>? where =
            categoriaId == null && categoriaId != Guid.Empty ? null :
            x => x.CategoriaId == categoriaId.Value;

        var totalPages = await _parceiroContext
            .Produtos
            .AsQueryable()
            .TotalPage(_take);

        var produtos = await _parceiroContext
            .Produtos
            .AsNoTracking()
            .AsQueryable()
            .OrderBy(x => x.Numero)
            .Include(x => x.Categoria)
            .WhereIsNotNull(where)
            .Skip(newPage * _take)
            .Take(_take)
            .ToListAsync();

        var produtosIds = produtos.Select(x => x.Id).ToList();
        var tamanhos = await _parceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .Include(x => x.Tamanho)
            .ToListAsync();

        var pesos = await _parceiroContext
            .PesosProdutos
            .AsNoTracking()
            .Include(x => x.Peso)
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();

        if (produtos.Count > 0)
        {
            produtos.ForEach(produto =>
            {
                produto.Categoria.Produtos = new();
                produto.Tamanhos = tamanhos
                    .Where(x => x.ProdutoId == produto.Id)
                    .Select(tm => new Tamanho(tm.Tamanho.Id, tm.Tamanho.DataDeCriacao, tm.Tamanho.DataDeAtualizacao, tm.Tamanho.Numero, tm.Tamanho.Descricao))
                    .ToList();

                produto.Pesos = pesos
                    .Where(x => x.ProdutoId == produto.Id)
                    .Select(tm =>
                        new Peso(tm.Peso.Id, tm.Peso.DataDeCriacao, tm.Peso.DataDeAtualizacao, tm.Peso.Numero, tm.Peso.Descricao)
                     )
                    .ToList();
            });
        }

        return new PaginacaoViewModel<Produto>()
        {
            TotalPage = totalPages,
            Values = produtos
        };
    }

    public async Task<IList<Produto>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _parceiroContext
            .ProdutosMaisVendidos
            .AsNoTracking()
            .Select(x =>
                    new ProdutosMaisVendidos(
                        x.Id,
                        x.DataDeCriacao,
                        x.DataDeAtualizacao,
                        x.Numero,
                        x.QuantidadeProdutos / x.QuantidadePedidos,
                        x.QuantidadePedidos,
                        x.ProdutoId))
            .ToListAsync();

        var produtosIds = produtosMaisVendidos
            .OrderByDescending(x => x.QuantidadeProdutos)
            .Select(x => x.ProdutoId)
            .Take(3)
            .ToList();

        return await _parceiroContext
            .Produtos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<int> GetTotalPageProdutosAsync()
    {
        return await _parceiroContext
            .Produtos
            .AsQueryable()
            .TotalPage(_take);
    }

    public async Task<IList<Produto>> GetProdutosByCategoriaIdAsync(Guid categoriaId)
    {
        var produtos = await _parceiroContext
            .Produtos
            .AsNoTracking()
            .AsQueryable()
            .OrderBy(x => x.Numero)
            .Include(x => x.Categoria)
            .Where(x => x.CategoriaId == categoriaId)
            .ToListAsync();

        var produtosIds = produtos.Select(x => x.Id).ToList();

        var tamanhos = await _parceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Include(x => x.Tamanho)
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();

        var pesos = await _parceiroContext
            .PesosProdutos
            .AsNoTracking()
            .Include(x => x.Peso)
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();

        if (produtos.Count > 0)
        {
            produtos.ForEach(produto =>
            {
                produto.Categoria.Produtos = new();
                produto.Tamanhos = tamanhos
                    .Where(x => x.ProdutoId == produto.Id)
                    .Select(tm =>
                        new Tamanho(tm.Tamanho.Id, tm.Tamanho.DataDeCriacao, tm.Tamanho.DataDeAtualizacao, tm.Tamanho.Numero, tm.Tamanho.Descricao)
                     )
                    .ToList();

                produto.Pesos = pesos
                    .Where(x => x.ProdutoId == produto.Id)
                    .Select(tm =>
                        new Peso(tm.Peso.Id, tm.Peso.DataDeCriacao, tm.Peso.DataDeAtualizacao, tm.Peso.Numero, tm.Peso.Descricao)
                     )
                    .ToList();
            });
        }

        return produtos;
    }

    public async Task<IList<Produto>> GetProdutosByListIdAsync(List<Guid> ids)
    {
        var produtos = await _parceiroContext
            .Produtos
            .AsQueryable()
            .Include(x => x.Categoria)
            .Include(x => x.Pesos)
            .Include(x => x.Tamanhos)
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync();

        produtos.ForEach(produto =>
        {
            produto.Categoria.Produtos = new();
            produto.Pesos.ForEach(peso =>
            {
                peso.Produtos = new();
            });
            produto.Tamanhos.ForEach(tamanho =>
            {
                tamanho.Produtos = new();
            });
        });

        return produtos;
    }

    public async Task<PaginacaoViewModel<Produto>> GetPaginacaoProdutoAsync(FilterModel<Produto> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Produtos
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Produto>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }

    public async Task<Produto?> GetProdutoByIdAsync(Guid id)
    {
        var produto = await _parceiroContext
            .Produtos
            .AsNoTracking()
            .Include(x => x.Categoria)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (produto != null)
        {
            var tamanhos = await _parceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Include(x => x.Tamanho)
            .Where(x => x.ProdutoId == produto.Id)
            .ToListAsync();

            var pesos = await _parceiroContext
                .PesosProdutos
                .AsNoTracking()
                .Include(x => x.Peso)
                .Where(x => x.ProdutoId == produto.Id)
                .ToListAsync();


            produto.Categoria.Produtos = new();
            produto.Tamanhos = tamanhos
                .Where(x => x.ProdutoId == produto.Id)
                .Select(tm =>
                    new Tamanho(tm.Tamanho.Id, tm.Tamanho.DataDeCriacao, tm.Tamanho.DataDeAtualizacao, tm.Tamanho.Numero, tm.Tamanho.Descricao)
                 )
                .ToList();

            produto.Pesos = pesos
                .Where(x => x.ProdutoId == produto.Id)
                .Select(tm =>
                    new Peso(tm.Peso.Id, tm.Peso.DataDeCriacao, tm.Peso.DataDeAtualizacao, tm.Peso.Numero, tm.Peso.Descricao)
                 )
                .ToList();
        }

        return produto;
    }

    public async Task<IList<Produto>> GetAllProdutosAsync()
    {
        return await _parceiroContext
            .Produtos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoDeProdutosAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Produtos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }
}

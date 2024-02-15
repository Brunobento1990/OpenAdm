using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class ProdutoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Produto>(parceiroContext), IProdutoRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;
    private const int _take = 5;

    public async Task<PaginacaoViewModel<Produto>> GetProdutosAsync(int page)
    {
        var newPage = page == 0 ? page : page - 1;

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
            .Skip(newPage * _take)
            .Take(_take)
            .ToListAsync();

        var tamanhos = await _parceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Include(x => x.Tamanho)
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

        var tamanhos = await _parceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Include(x => x.Tamanho)
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
            });
        }

        return produtos;
    }
}

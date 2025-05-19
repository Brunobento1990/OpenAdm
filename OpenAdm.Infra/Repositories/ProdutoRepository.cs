using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using System.Linq.Expressions;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class ProdutoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Produto>(parceiroContext), IProdutoRepository
{
    private const int _take = 6;

    public async Task<PaginacaoViewModel<Produto>> GetProdutosAsync(PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto)
    {
        var newPage = paginacaoProdutoEcommerceDto.Page == 0 ? paginacaoProdutoEcommerceDto.Page : paginacaoProdutoEcommerceDto.Page - 1;

        Expression<Func<Produto, bool>>? where =
            paginacaoProdutoEcommerceDto.CategoriaId == null || paginacaoProdutoEcommerceDto.CategoriaId == Guid.Empty ? null :
            x => x.CategoriaId == paginacaoProdutoEcommerceDto.CategoriaId.Value;

        Expression<Func<Produto, bool>>? wherePesos =
            paginacaoProdutoEcommerceDto.PesoId == null || paginacaoProdutoEcommerceDto.PesoId == Guid.Empty ? null :
            x => x.Pesos.Any(p => p.Id == paginacaoProdutoEcommerceDto.PesoId.Value);

        Expression<Func<Produto, bool>>? whereTamanhos =
            paginacaoProdutoEcommerceDto.TamanhoId == null || paginacaoProdutoEcommerceDto.TamanhoId == Guid.Empty ? null :
            x => x.Tamanhos.Any(p => p.Id == paginacaoProdutoEcommerceDto.TamanhoId.Value);

        var produtos = await ParceiroContext
            .Produtos
            .AsNoTracking()
            .AsQueryable()
            .OrderBy(x => x.Numero)
            .Include(x => x.Categoria)
            .Include(x => x.Tamanhos)
            .Include(x => x.Pesos)
            .Where(x => !x.InativoEcommerce)
            .WhereIsNotNull(where)
            .WhereIsNotNull(wherePesos)
            .WhereIsNotNull(whereTamanhos)
            .Skip(newPage * _take)
            .Take(_take)
            .ToListAsync();

        if (produtos.Count > 0)
        {
            produtos.ForEach(produto =>
            {
                if (paginacaoProdutoEcommerceDto.PesoId != null && paginacaoProdutoEcommerceDto.PesoId != Guid.Empty)
                {
                    produto.Pesos = produto.Pesos.Where(peso => peso.Id == paginacaoProdutoEcommerceDto.PesoId.Value).ToList();
                }

                if (paginacaoProdutoEcommerceDto.TamanhoId != null && paginacaoProdutoEcommerceDto.TamanhoId != Guid.Empty)
                {
                    produto.Tamanhos = produto.Tamanhos.Where(tamanho => tamanho.Id == paginacaoProdutoEcommerceDto.TamanhoId.Value).ToList();
                }

                produto.Categoria.Produtos = new();
                produto.Tamanhos.ForEach(tamanho =>
                {
                    tamanho.Produtos = new();
                });

                produto.Pesos.ForEach(peso =>
                {
                    peso.Produtos = new();
                });
            });
        }

        var totalPages = await ParceiroContext
            .Produtos
            .AsQueryable()
            .WhereIsNotNull(where)
            .WhereIsNotNull(wherePesos)
            .WhereIsNotNull(whereTamanhos)
            .TotalPage(_take);

        return new PaginacaoViewModel<Produto>()
        {
            TotalPaginas = totalPages,
            Values = produtos,
        };
    }

    public async Task<IList<Produto>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await ParceiroContext
            .ProdutosMaisVendidos
            .AsNoTracking()
            .Select(x =>
                    new ProdutoMaisVendido(
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

        return await ParceiroContext
            .Produtos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<int> GetTotalPageProdutosAsync()
    {
        return await ParceiroContext
            .Produtos
            .AsQueryable()
            .TotalPage(_take);
    }

    public async Task<IList<Produto>> GetProdutosByCategoriaIdAsync(Guid categoriaId)
    {
        var produtos = await ParceiroContext
            .Produtos
            .AsNoTracking()
            .AsQueryable()
            .OrderBy(x => x.Numero)
            .Include(x => x.Categoria)
            .Where(x => x.CategoriaId == categoriaId)
            .ToListAsync();

        var produtosIds = produtos.Select(x => x.Id).ToList();

        var tamanhos = await ParceiroContext
            .TamanhosProdutos
            .AsNoTracking()
            .Include(x => x.Tamanho)
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();

        var pesos = await ParceiroContext
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
                        new Tamanho(tm.Tamanho.Id, tm.Tamanho.DataDeCriacao, tm.Tamanho.DataDeAtualizacao, tm.Tamanho.Numero, tm.Tamanho.Descricao, tm.Tamanho.PesoReal)
                     )
                    .ToList();

                produto.Pesos = pesos
                    .Where(x => x.ProdutoId == produto.Id)
                    .Select(tm =>
                        new Peso(tm.Peso.Id, tm.Peso.DataDeCriacao, tm.Peso.DataDeAtualizacao, tm.Peso.Numero, tm.Peso.Descricao, tm.Peso.PesoReal)
                     )
                    .ToList();
            });
        }

        return produtos;
    }

    public async Task<IList<Produto>> GetProdutosByListIdAsync(List<Guid> ids)
    {
        var produtos = await ParceiroContext
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

    public async Task<Produto?> GetProdutoByIdAsync(Guid id)
    {
        var produto = await ParceiroContext
            .Produtos
            .Include(x => x.Categoria)
            .Include(x => x.Tamanhos)
            .Include(x => x.Pesos)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (produto != null)
        {
            var tamanhos = await ParceiroContext
            .TamanhosProdutos
            .Include(x => x.Tamanho)
            .Where(x => x.ProdutoId == produto.Id)
            .ToListAsync();

            var pesos = await ParceiroContext
                .PesosProdutos
                .Include(x => x.Peso)
                .Where(x => x.ProdutoId == produto.Id)
                .ToListAsync();


            produto.Categoria.Produtos = [];
            produto.Tamanhos = tamanhos
                .Where(x => x.ProdutoId == produto.Id)
                .Select(tm =>
                    new Tamanho(tm.Tamanho.Id, tm.Tamanho.DataDeCriacao, tm.Tamanho.DataDeAtualizacao, tm.Tamanho.Numero, tm.Tamanho.Descricao, tm.Tamanho.PesoReal)
                 )
                .ToList();

            produto.Pesos = pesos
                .Where(x => x.ProdutoId == produto.Id)
                .Select(tm =>
                    new Peso(tm.Peso.Id, tm.Peso.DataDeCriacao, tm.Peso.DataDeAtualizacao, tm.Peso.Numero, tm.Peso.Descricao, tm.Peso.PesoReal)
                 )
                .ToList();
        }

        return produto;
    }

    public async Task<IList<Produto>> GetAllProdutosAsync()
    {
        return await ParceiroContext
            .Produtos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoDeProdutosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Produtos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<IDictionary<Guid, Produto>> GetDictionaryProdutosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Produtos
            .AsNoTracking()
            .Include(x => x.Categoria)
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
    }

    public async Task<Produto?> GetProdutoByIdParaEditarAsync(Guid id)
    {
        return await ParceiroContext
            .Produtos
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Produto produto)
    {
        ParceiroContext.Produtos.Update(produto);
    }
}

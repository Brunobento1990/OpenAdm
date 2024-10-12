using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TamanhosProdutoRepository : GenericRepository<TamanhoProduto>, ITamanhosProdutoRepository
{
    public TamanhosProdutoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<TamanhoProduto>> AddRangeAsync(IList<TamanhoProduto> tamanhosProdutos)
    {
        await _parceiroContext.AddRangeAsync(tamanhosProdutos);
        return tamanhosProdutos;
    }

    public async Task<bool> DeleteRangeAsync(Guid produtoId)
    {
        try
        {
            var tamanhosProdutos = await _parceiroContext
                .TamanhosProdutos
                .Where(x => x.ProdutoId == produtoId)
                .ToListAsync();

            _parceiroContext.RemoveRange(tamanhosProdutos);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

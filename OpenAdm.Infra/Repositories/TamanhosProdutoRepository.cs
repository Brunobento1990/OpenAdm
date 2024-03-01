using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TamanhosProdutoRepository : GenericRepository<TamanhosProdutos>, ITamanhosProdutoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public TamanhosProdutoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IList<TamanhosProdutos>> AddRangeAsync(IList<TamanhosProdutos> tamanhosProdutos)
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

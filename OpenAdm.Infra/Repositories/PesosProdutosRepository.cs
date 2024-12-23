using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class PesosProdutosRepository(ParceiroContext parceiroContext) 
    : GenericRepository<PesoProduto>(parceiroContext), IPesosProdutosRepository
{
    public async Task<IList<PesoProduto>> AddRangeAsync(IList<PesoProduto> pesosProdutos)
    {
        await ParceiroContext.AddRangeAsync(pesosProdutos);
        return pesosProdutos;
    }

    public async Task<bool> DeleteRangeAsync(Guid produtoId)
    {
        try
        {
            var pesosProdutos = await ParceiroContext
                .PesosProdutos
                .Where(x => x.ProdutoId == produtoId)
                .ToListAsync();

            ParceiroContext.RemoveRange(pesosProdutos);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

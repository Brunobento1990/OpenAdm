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
        if (pesosProdutos.Count > 0)
        {
            await ParceiroContext.AddRangeAsync(pesosProdutos);
        }
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

            if (pesosProdutos.Count > 0)
            {
                ParceiroContext.RemoveRange(pesosProdutos);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

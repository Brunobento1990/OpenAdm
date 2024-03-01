﻿using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class PesosProdutosRepository(ParceiroContext parceiroContext) 
    : GenericRepository<PesosProdutos>(parceiroContext), IPesosProdutosRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<IList<PesosProdutos>> AddRangeAsync(IList<PesosProdutos> pesosProdutos)
    {
        await _parceiroContext.AddRangeAsync(pesosProdutos);
        return pesosProdutos;
    }

    public async Task<bool> DeleteRangeAsync(Guid produtoId)
    {
        try
        {
            var pesosProdutos = await _parceiroContext
                .PesosProdutos
                .Where(x => x.ProdutoId == produtoId)
                .ToListAsync();

            _parceiroContext.RemoveRange(pesosProdutos);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

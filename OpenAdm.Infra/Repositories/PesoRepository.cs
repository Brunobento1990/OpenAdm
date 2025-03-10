﻿using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class PesoRepository : GenericRepository<Peso>, IPesoRepository
{
    public PesoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<IDictionary<Guid, Peso>> GetDictionaryPesosByIdsAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
    }

    public async Task<Peso?> GetPesoByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Peso>> GetPesosAsync()
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}

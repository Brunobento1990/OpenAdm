﻿using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class TabelaDePrecoRepository : GenericRepository<TabelaDePreco>, ITabelaDePrecoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public TabelaDePrecoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<int> GetCountTabelaDePrecoAsync()
    {
        return await _parceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .CountAsync();
    }

    public async Task<PaginacaoViewModel<TabelaDePreco>> GetPaginacaoAsync(FilterModel<TabelaDePreco> filterModel)
    {
        var (total, values) = await _parceiroContext
                .TabelaDePreco
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<TabelaDePreco>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync()
    {
        return await _parceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .Include(x => x.ItensTabelaDePreco)
            .FirstOrDefaultAsync(x => x.AtivaEcommerce);
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .Include(x => x.ItensTabelaDePreco)
                .ThenInclude(x => x.Produto)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

﻿using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface IFaturaContasAReceberRepository : IGenericRepository<FaturaContasAReceber>
{
    Task<FaturaContasAReceber?> GetByIdAsync(Guid id);
    Task<IList<FaturaContasAReceber>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum);
}
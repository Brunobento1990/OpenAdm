﻿using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILojasParceirasRepository : IGenericRepository<LojaParceira>
{
    Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id);
    Task<IList<LojaParceira>> GetLojasParceirasAsync();
    Task<IList<string?>> GetFotosLojasParceirasAsync();
}

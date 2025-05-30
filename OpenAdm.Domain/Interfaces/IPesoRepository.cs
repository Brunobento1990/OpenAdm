﻿using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IPesoRepository : IGenericRepository<Peso>
{
    Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids);
    Task<IList<Peso>> GetPesosAsync();
    Task<Peso?> GetPesoByIdAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids);
    Task<IDictionary<Guid, Peso>> GetDictionaryPesosByIdsAsync(IList<Guid> ids);
}

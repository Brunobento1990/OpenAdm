﻿using OpenAdm.Domain.Entities;
using System.Linq.Expressions;

namespace OpenAdm.Domain.Interfaces;

public interface IEstoqueRepository : IGenericRepository<Estoque>
{
    Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where);
}

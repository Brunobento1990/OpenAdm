﻿using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    Task<IList<Produto>> GetProdutosMaisVendidosAsync();
    Task<IList<Produto>> GetProdutosByCategoriaIdAsync(Guid categoriaId);
    Task<IList<Produto>> GetProdutosByListIdAsync(List<Guid> ids);
    Task<PaginacaoViewModel<Produto>> GetProdutosAsync(int page);
}
﻿using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItemTabelaDePrecoRepository : IGenericRepository<ItemTabelaDePreco>
{
    Task<ItemTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id);
    Task<IList<ItemTabelaDePreco>> GetItensTabelaDePrecoByIdAsync(Guid tabelaDePrecoId);
    Task<IList<ItemTabelaDePreco>> ObterPorPesoIdAsync(Guid pesoId);
    Task<IList<ItemTabelaDePreco>> ObterPorTamanhoIdAsync(Guid tamanhoId);
    Task<IList<ItemTabelaDePreco>> GetItensTabelaDePrecoByIdProdutosAsync(IList<Guid> produtosIds);
    Task AddRangeAsync(IList<ItemTabelaDePreco> itensTabelaDePrecos);
    Task DeleteItensTabelaDePrecoByProdutoIdAsync(Guid produtoId);
}

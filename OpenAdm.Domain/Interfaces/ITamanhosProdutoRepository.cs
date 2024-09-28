using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhosProdutoRepository : IGenericRepository<TamanhoProduto>
{
    Task<IList<TamanhoProduto>> AddRangeAsync(IList<TamanhoProduto> tamanhosProdutos);
    Task<bool> DeleteRangeAsync(Guid produtoId);
}

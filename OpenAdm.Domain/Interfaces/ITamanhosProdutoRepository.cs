using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITamanhosProdutoRepository : IGenericRepository<TamanhosProdutos>
{
    Task<IList<TamanhosProdutos>> AddRangeAsync(IList<TamanhosProdutos> tamanhosProdutos);
    Task<bool> RemoveRangeAsync(Guid produtoId);
}

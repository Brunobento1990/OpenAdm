using OpenAdm.Domain.Entities;
using System.Linq.Expressions;

namespace OpenAdm.Domain.Interfaces;

public interface IEstoqueRepository : IGenericRepository<Estoque>
{
    Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where);
    Task<IList<Estoque>> GetPosicaoEstoqueAsync();
    Task<IDictionary<Guid, Estoque>> GetPosicaoEstoqueAsync(ICollection<Guid> ids);
    Task<IList<Estoque>> PosicaoEstoqueDoProdutoAsync(Guid produtoId);

    Task<IList<Estoque>> GetPosicaoEstoqueRelatorioAsync(
        ICollection<Guid>? produtos,
        ICollection<Guid>? pesos,
        ICollection<Guid>? tamanhos,
        ICollection<Guid>? categorias);

    Task<IList<Estoque>> GetPosicaoEstoqueDosProdutosAsync(IList<Guid> produtosIds);
    Task AddRangeAsync(IList<Estoque> entities);
    void UpdateRange(IList<Estoque> entities);
}
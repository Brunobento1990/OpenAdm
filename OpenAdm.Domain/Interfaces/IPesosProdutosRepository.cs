using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IPesosProdutosRepository : IGenericRepository<PesosProdutos>
{
    Task<bool> DeleteRangeAsync(Guid produtoId);
    Task<IList<PesosProdutos>> AddRangeAsync(IList<PesosProdutos> pesosProdutos);
}

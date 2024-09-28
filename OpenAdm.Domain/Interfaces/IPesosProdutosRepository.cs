using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IPesosProdutosRepository : IGenericRepository<PesoProduto>
{
    Task<bool> DeleteRangeAsync(Guid produtoId);
    Task<IList<PesoProduto>> AddRangeAsync(IList<PesoProduto> pesosProdutos);
}

using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IEstoqueRepository : IGenericRepository<Estoque>
{
    Task<Estoque?> GetEstoqueByProdutoIdAsync(Guid produtoId);
    Task<PaginacaoViewModel<Estoque>> GetPaginacaoEstoqueAsync(FilterModel<Estoque> filterModel);
}

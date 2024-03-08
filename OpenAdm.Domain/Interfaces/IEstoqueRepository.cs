using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Domain.Interfaces;

public interface IEstoqueRepository : IGenericRepository<Estoque>
{
    Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where);
    Task<PaginacaoViewModel<Estoque>> GetPaginacaoEstoqueAsync(FilterModel<Estoque> filterModel);
}

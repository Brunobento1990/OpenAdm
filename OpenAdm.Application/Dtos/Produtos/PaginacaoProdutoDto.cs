using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Application.PaginateDto;

public class PaginacaoProdutoDto : FilterModel<Produto>
{
    public override Expression<Func<Produto, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().Contains(Search.ToLower())
            || x.Descricao.Contains(Search.ToLower());
    }
}

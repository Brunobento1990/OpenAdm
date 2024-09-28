using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoProdutoDto : FilterModel<Produto>
{
    public override Expression<Func<Produto, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
    }
}

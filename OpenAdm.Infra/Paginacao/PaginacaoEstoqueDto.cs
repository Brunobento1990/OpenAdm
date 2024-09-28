using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoEstoqueDto : FilterModel<Estoque>
{
    public override Expression<Func<Estoque, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Numero.ToString()), $"%{Search}%");
    }
}

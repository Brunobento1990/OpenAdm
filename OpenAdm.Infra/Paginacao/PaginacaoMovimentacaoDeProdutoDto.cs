using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoMovimentacaoDeProdutoDto : FilterModel<MovimentacaoDeProduto>
{
    public override Expression<Func<MovimentacaoDeProduto, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Numero.ToString()), $"%{Search}%");
    }
}

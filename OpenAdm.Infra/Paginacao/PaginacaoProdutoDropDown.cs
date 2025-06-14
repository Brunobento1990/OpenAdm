using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.PaginateDto;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoProdutoDropDown : PaginacaoDropDown<Produto>
{
    public override Expression<Func<Produto, bool>>? Where()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return null;
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
    }
}

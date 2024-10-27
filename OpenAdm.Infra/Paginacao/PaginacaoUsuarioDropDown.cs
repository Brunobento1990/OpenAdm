using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.PaginateDto;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoUsuarioDropDown : PaginacaoDropDown<Usuario>
{
    public override Expression<Func<Usuario, bool>>? Where()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return null;
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Nome), $"%{Search}%");
    }
}

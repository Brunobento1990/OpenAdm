using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoPesoDto : FilterModel<Peso>
{
    public override Expression<Func<Peso, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
    }
}

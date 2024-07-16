using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoUsuarioDto : FilterModel<Usuario>
{
    public override Expression<Func<Usuario, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Cpf!), $"%{Search}%") ||
        EF.Functions.ILike(EF.Functions.Unaccent(x.Cnpj!), $"%{Search}%") ||
        EF.Functions.ILike(EF.Functions.Unaccent(x.Nome), $"%{Search}%");
    }
}

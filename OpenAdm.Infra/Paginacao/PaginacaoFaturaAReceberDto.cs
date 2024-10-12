using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoFaturaAReceberDto : FilterModel<FaturaContasAReceber>
{
    public override Expression<Func<FaturaContasAReceber, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return null;
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Numero.ToString()), $"%{Search}%") || EF.Functions.ILike(EF.Functions.Unaccent(x.ContasAReceber.Usuario.Nome), $"%{Search}%");
    }

    public override Expression<Func<FaturaContasAReceber, object>>? IncludeCustom()
    {
        return x => x.ContasAReceber.Usuario;
    }
}

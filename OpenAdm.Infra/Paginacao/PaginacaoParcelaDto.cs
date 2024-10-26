using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoParcelaDto : FilterModel<Parcela>
{
    public TipoFaturaEnum Tipo { get; set; }
    public override Expression<Func<Parcela, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return x => x.Fatura.Tipo == Tipo;
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Numero.ToString()), $"%{Search}%") || EF.Functions.ILike(EF.Functions.Unaccent(x.Fatura.Usuario.Nome), $"%{Search}%") && x.Fatura.Tipo == Tipo;
    }

    public override Expression<Func<Parcela, object>>? IncludeCustom()
    {
        return x => x.Fatura.Usuario;
    }
}

using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public sealed class PaginacaoFaturaBonificadaDto : FilterModel<Fatura>
{
    public override Expression<Func<Fatura, bool>> GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            return x => x.Tipo == TipoFaturaEnum.Bonificado;
        }

        var search = Search.RemoverAcentos();

        return x => x.Tipo == TipoFaturaEnum.Bonificado &&
            (EF.Functions.ILike(EF.Functions.Unaccent(x.Pedido!.Numero.ToString()), $"%{search}%") ||
             EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Nome), $"%{search}%"));
    }

    public override IList<Expression<Func<Fatura, object>>> IncludeCustomList()
    {
        return [x => x.Usuario, x => x.Pedido!];
    }
}

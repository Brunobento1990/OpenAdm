using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.Extensions;

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
        
        var search = Search.RemoverAcentos();

        return x =>
             (EF.Functions.ILike(EF.Functions.Unaccent(x.Fatura.Pedido!.Numero.ToString()), $"%{search}%") ||
             EF.Functions.ILike(EF.Functions.Unaccent(x.Fatura.Usuario.Nome), $"%{search}%"))
            && x.Fatura.Tipo == Tipo;
    }

    public override IList<Expression<Func<Parcela, object>>>? IncludeCustomList()
    {
        return [x => x.Fatura.Usuario, x => x.Fatura.Pedido!];
    }
}

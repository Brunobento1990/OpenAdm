using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using System.Linq.Expressions;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoParcelaDto : FilterModel<Parcela>
{
    public TipoFaturaEnum Tipo { get; set; }
    public DateTime? DataVencimentoInicial { get; set; }
    public DateTime? DataVencimentoFinal { get; set; }
    public bool? Quitada { get; set; }
    public Guid? PedidoId { get; set; }

    public override Expression<Func<Parcela, object>> OrderByCustom()
    {
        switch (OrderBy.ToLower())
        {
            case "numerofatura":
                return x => x.Fatura.Numero;
            case "numeroparcela":
                return x => x.NumeroDaParcela;
            default:
                return x => x.DataDeCriacao;
        }
    }

    public override Expression<Func<Parcela, bool>> GetWhereBySearch()
    {
        var dataVencimentoInicial = DataVencimentoInicial?.Date;
        var dataVencimentoFinalExclusiva = DataVencimentoFinal?.Date.AddDays(1);

        if (string.IsNullOrWhiteSpace(Search))
        {
            return x => x.Fatura.Tipo == Tipo &&
                        (!PedidoId.HasValue || x.Fatura.PedidoId == PedidoId.Value) &&
                        (!Quitada.HasValue || x.Quitada == Quitada.Value) &&
                        (!dataVencimentoInicial.HasValue || x.DataDeVencimento >= dataVencimentoInicial.Value) &&
                        (!dataVencimentoFinalExclusiva.HasValue ||
                         x.DataDeVencimento < dataVencimentoFinalExclusiva.Value);
        }

        var search = Search.RemoverAcentos();

        return x =>
            (EF.Functions.ILike(EF.Functions.Unaccent(x.Fatura.Pedido!.Numero.ToString()), $"%{search}%") ||
             EF.Functions.ILike(EF.Functions.Unaccent(x.Fatura.Usuario.Nome), $"%{search}%"))
            && x.Fatura.Tipo == Tipo
            && (!PedidoId.HasValue || x.Fatura.PedidoId == PedidoId.Value)
            && (!Quitada.HasValue || x.Quitada == Quitada.Value)
            && (!dataVencimentoInicial.HasValue || x.DataDeVencimento >= dataVencimentoInicial.Value)
            && (!dataVencimentoFinalExclusiva.HasValue || x.DataDeVencimento < dataVencimentoFinalExclusiva.Value);
    }

    public override IList<Expression<Func<Parcela, object>>> IncludeCustomList()
    {
        return [x => x.Fatura.Usuario, x => x.Fatura.Pedido!, x => x.Transacoes!];
    }
}
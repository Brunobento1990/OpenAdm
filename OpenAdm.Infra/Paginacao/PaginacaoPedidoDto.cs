using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoPedidoDto : FilterModel<Pedido>
{
    public int? StatusPedido { get; set; }
    public override Expression<Func<Pedido, object>>? IncludeCustom()
    {
        return x => x.ItensPedido;
    }
    public override Expression<Func<Pedido, bool>>? GetWhereBySearch()
    {

        if (string.IsNullOrWhiteSpace(Search) && StatusPedido == null)
            return null;

        if (!string.IsNullOrWhiteSpace(Search) && StatusPedido != null)
            return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Email), $"%{Search}%")
            || x.StatusPedido == (StatusPedido)StatusPedido;

        if (!string.IsNullOrWhiteSpace(Search) && StatusPedido == null)
            return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Email), $"%{Search}%") ||
                EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Nome), $"%{Search}%");

        if (StatusPedido != null && string.IsNullOrWhiteSpace(Search))
            return x => x.StatusPedido == (StatusPedido)StatusPedido;

        return null;
    }
}

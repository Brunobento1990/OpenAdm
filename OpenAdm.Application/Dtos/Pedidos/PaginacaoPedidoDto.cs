using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Application.PaginateDto;

public class PaginacaoPedidoDto : FilterModel<Pedido>
{
    public int? StatusPedido { get; set; }

    public override Expression<Func<Pedido, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search) && StatusPedido == null)
            return null;

        if (!string.IsNullOrWhiteSpace(Search) && StatusPedido != null)
            return x => x.Usuario.Nome.ToLower().Contains(Search.ToLower())
            || x.StatusPedido == (StatusPedido)StatusPedido;

        if (!string.IsNullOrWhiteSpace(Search) && StatusPedido == null)
            return x => x.Usuario.Nome.ToLower().Contains(Search.ToLower());

        if (StatusPedido != null && string.IsNullOrWhiteSpace(Search))
            return x => x.StatusPedido == (StatusPedido)StatusPedido;

        return null;
    }
}

using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoPedidoDto : FilterModel<Pedido>
{
    public int? StatusPedido { get; set; }
    public Guid? RepresentanteId { get; set; }
    public override Expression<Func<Pedido, object>>? IncludeCustom()
    {
        return x => x.ItensPedido;
    }
    public override Expression<Func<Pedido, bool>>? GetWhereBySearch()
    {

        if (string.IsNullOrWhiteSpace(Search) && StatusPedido == null && RepresentanteId == null)
            return null;

        return x =>
            (!RepresentanteId.HasValue || x.RepresentanteId == RepresentanteId) &&
            (!StatusPedido.HasValue || x.StatusPedido == (StatusPedido)StatusPedido) &&
            (string.IsNullOrWhiteSpace(Search) ||
             EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Email), $"%{Search}%") ||
             EF.Functions.ILike(EF.Functions.Unaccent(x.Usuario.Nome), $"%{Search}%"));
    }
}

using Domain.Pkg.Entities;
using System.Linq.Expressions;

namespace OpenAdm.Domain.Model.Pedidos;

public class RelatorioPedidoDto
{
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public Guid? UsuarioId { get; set; }

    public Expression<Func<Pedido, bool>>? WhereUsuarioId()
    {
        if (UsuarioId is null || UsuarioId.Value == Guid.Empty)
        {
            return null;
        }

        return x => x.UsuarioId == UsuarioId;
    }
}

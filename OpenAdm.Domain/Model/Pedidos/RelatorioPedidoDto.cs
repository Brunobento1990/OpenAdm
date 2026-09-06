using OpenAdm.Domain.Entities;
using System.Linq.Expressions;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Model.Pedidos;

public class RelatorioPedidoDto
{
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public Guid? UsuarioId { get; set; }

    public void Validar()
    {
        var usuarioNaoInformado = UsuarioId is null || UsuarioId == Guid.Empty;

        if (usuarioNaoInformado && (!DataInicial.HasValue || !DataFinal.HasValue))
            throw new ExceptionApi("Informe o período quando o cliente não for informado.");

        if (DataInicial.HasValue && DataFinal.HasValue)
        {
            if (DataFinal.Value.Date < DataInicial.Value.Date)
                throw new ExceptionApi("A data final deve ser maior ou igual à data inicial.");

            if (usuarioNaoInformado && (DataFinal.Value.Date - DataInicial.Value.Date).TotalDays > 90)
                throw new ExceptionApi("O período máximo permitido sem informar um cliente é de 90 dias.");
        }
    }

    public Expression<Func<Pedido, bool>>? WhereUsuarioId()
    {
        if (UsuarioId is null || UsuarioId.Value == Guid.Empty)
        {
            return null;
        }

        return x => x.UsuarioId == UsuarioId;
    }
}

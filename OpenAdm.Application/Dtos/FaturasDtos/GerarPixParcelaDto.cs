using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class GerarPixParcelaDto
{
    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }

    public void Validar()
    {
        if (Valor <= 0)
        {
            throw new ExceptionApi("O valor deve ser maior que zero");
        }

        if (PedidoId == Guid.Empty)
        {
            throw new ExceptionApi("Pedido inválido");
        }
    }
}

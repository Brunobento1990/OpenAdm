using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoCreateDto
{
    public EnderecoDto EnderecoEntrega { get; set; } = null!;
    public IList<ItemPedidoModel> Itens { get; set; } = [];

    public void Validar()
    {
        if (EnderecoEntrega == null)
        {
            throw new ExceptionApi("Informe o endereço de entrega");
        }

        EnderecoEntrega.Validar();

        if (Itens.Count == 0)
        {
            throw new ExceptionApi("Informe os itens do pedido");
        }
    }
}

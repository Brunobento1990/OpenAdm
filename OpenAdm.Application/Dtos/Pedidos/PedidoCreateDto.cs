using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoCreateDto : ValidarBaseDTO
{
    public EnderecoDto EnderecoEntrega { get; set; } = null!;
    public IList<ItemPedidoModel> Itens { get; set; } = [];
    public int? FreteId { get; set; }
    public decimal? ValorFrete { get; set; }

    public override string? Validar()
    {
        if (EnderecoEntrega == null)
        {
            return "Informe o endereço de entrega";
        }

        if (Itens.Count == 0)
        {
            return "Informe os itens do pedido";
        }

        return EnderecoEntrega.Validar();
    }
}
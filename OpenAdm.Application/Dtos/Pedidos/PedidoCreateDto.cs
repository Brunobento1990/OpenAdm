using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoCreateDto : ValidarBaseDTO
{
    public EnderecoDto EnderecoEntrega { get; set; } = null!;
    public IList<ItemPedidoModel> Itens { get; set; } = [];
    public IEnumerable<ItemPedidoModel> ItensQuantidadesValidas => Itens.Where(x => x.Quantidade > 0);
    public int? FreteId { get; set; }
    public decimal? ValorFrete { get; set; }

    public override string? Validar()
    {
        if (EnderecoEntrega == null)
        {
            return "Informe o endereço de entrega";
        }

        if (!ItensQuantidadesValidas.Any())
        {
            return "Informe os itens do pedido";
        }

        return EnderecoEntrega.Validar();
    }
}
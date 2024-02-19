using Domain.Pkg.Model;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoCreateDto
{
    public IList<PedidoPorPesoModel> PedidosPorPeso { get; set; } = new List<PedidoPorPesoModel>();
    public IList<PedidoPorTamanhoModel> PedidosPorTamanho { get; set; } = new List<PedidoPorTamanhoModel>();
}

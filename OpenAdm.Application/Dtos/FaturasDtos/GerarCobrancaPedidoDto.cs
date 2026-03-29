using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class GerarCobrancaPedidoDto
{
    public Guid PedidoId { get; set; }
    public MeioDePagamentoEnum MeioDePagamento { get; set; }
}

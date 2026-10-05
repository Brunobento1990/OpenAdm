using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities.OpenAdm;

public class CobrancaPedidoEcommerce : BaseEntity
{
    public CobrancaPedidoEcommerce(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid pedidoId, bool ativo, decimal total, StatusCobrancaPedidoEcommerceEnum status)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        PedidoId = pedidoId;
        Ativo = ativo;
        Total = total;
        Status = status;
    }

    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; set; } = null!;
    public bool Ativo { get; private set; }
    public decimal Total { get; private set; }
    public StatusCobrancaPedidoEcommerceEnum Status { get; private set; }

    public static CobrancaPedidoEcommerce Novo(Guid pedidoId, decimal total)
    {
        return new CobrancaPedidoEcommerce(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.UtcNow,
            dataDeAtualizacao: DateTime.UtcNow,
            numero: 0,
            pedidoId: pedidoId,
            ativo: true,
            total: total,
            status: StatusCobrancaPedidoEcommerceEnum.ACobrar);
    }

}

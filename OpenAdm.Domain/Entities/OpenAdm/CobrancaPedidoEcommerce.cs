using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities.OpenAdm;

public class CobrancaPedidoEcommerce : BaseEntityParceiro
{
    public CobrancaPedidoEcommerce(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid parceiroId, Guid pedidoId, bool ativo, decimal total, StatusCobrancaPedidoEcommerceEnum status)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        PedidoId = pedidoId;
        Ativo = ativo;
        Total = total;
        Status = status;
    }

    public Guid PedidoId { get; private set; }
    public bool Ativo { get; private set; }
    public decimal Total { get; private set; }
    public StatusCobrancaPedidoEcommerceEnum Status { get; private set; }

    public static CobrancaPedidoEcommerce Novo(Guid pedidoId, decimal total, long numero, Guid parceiroId)
    {
        return new CobrancaPedidoEcommerce(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: numero,
            parceiroId: parceiroId,
            pedidoId: pedidoId,
            ativo: true,
            total: total,
            status: StatusCobrancaPedidoEcommerceEnum.ACobrar);
    }
}
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface ICobrancaPedidoEcommerceRepository : IGenericBaseRepository<CobrancaPedidoEcommerce>
{
    Task<CobrancaPedidoEcommerce?> GetByPedidoIdAsync(Guid pedidoId, Guid parceiroId);
    Task AtualizarStatusAsync(Guid id, Guid parceiroId, StatusCobrancaPedidoEcommerceEnum status);
    Task<decimal> TotalACobrarAposAsync(DateTime data, Guid parceiroId);
    Task<int> QuantidadeACobrarAsync(Guid parceiroId);
    Task<decimal> TotalACobrarAsync(Guid parceiroId);
    Task<ICollection<CobrancaPedidoEcommerce>> CobrancasMaisAntigasAsync(Guid parceiroId);
}

using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface ICobrancaPedidoEcommerceRepository : IGenericBaseRepository<CobrancaPedidoEcommerce>
{
    Task<decimal> TotalACobrarAposAsync(DateTime data, Guid parceiroId);
    Task<int> QuantidadeACobrarAsync(Guid parceiroId);
    Task<decimal> TotalACobrarAsync(Guid parceiroId);
    Task<ICollection<CobrancaPedidoEcommerce>> CobrancasMaisAntigasAsync(Guid parceiroId);
}
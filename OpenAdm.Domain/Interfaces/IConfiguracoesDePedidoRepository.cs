using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracoesDePedidoRepository : IGenericBaseRepository<ConfiguracoesDePedido>
{
    Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync(Guid parceiroId);
}

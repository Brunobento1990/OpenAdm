using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracoesDePedidoRepository : IGenericRepository<ConfiguracoesDePedido>
{
    Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync();
}

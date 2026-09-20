using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface IResumoMensalPedidoRepository
{
    Task<ResumoMensalPedidoModel> ObterAsync(DateTime inicio, DateTime fim);
}

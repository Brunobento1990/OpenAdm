namespace OpenAdm.Application.Interfaces;

public interface IProcessarPedidoService
{
    Task ProcessarCreateAsync(Guid pedidoId);
}

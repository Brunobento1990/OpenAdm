using OpenAdm.Domain.Entities;
namespace OpenAdm.Application.Interfaces;

public interface ITopUsuarioService
{
    Task AddOrUpdateTopUsuarioAsync(Pedido pedido);
}

using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class TopUsuarioService : ITopUsuarioService
{
    private readonly ITopUsuariosRepository _topUsuariosRepository;

    public TopUsuarioService(ITopUsuariosRepository topUsuariosRepository)
    {
        _topUsuariosRepository = topUsuariosRepository;
    }
    public async Task AddOrUpdateTopUsuarioAsync(Pedido pedido)
    {
        var topUsuario = await _topUsuariosRepository.GetByUsuarioIdAsync(pedido.UsuarioId);

        if (topUsuario != null)
        {
            topUsuario.Update(pedido.ValorTotal, 1);
            await _topUsuariosRepository.UpdateAsync(topUsuario);
            return;
        }

        var date = DateTime.Now;

        topUsuario = new TopUsuario(
            Guid.NewGuid(),
            date,
            date,
            0,
            pedido.ValorTotal,
        1,
        pedido.UsuarioId,
        pedido.Usuario.Nome);

        await _topUsuariosRepository.AddAsync(topUsuario);
    }
}

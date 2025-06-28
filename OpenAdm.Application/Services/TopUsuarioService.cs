using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class TopUsuarioService : ITopUsuarioService
{
    private readonly ITopUsuariosRepository _topUsuariosRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public TopUsuarioService(ITopUsuariosRepository topUsuariosRepository, IUsuarioAutenticado usuarioAutenticado)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }
    public async Task AddOrUpdateTopUsuarioAsync(Pedido pedido)
    {
        var topUsuario = await _topUsuariosRepository.GetByUsuarioIdAsync(pedido.UsuarioId, _usuarioAutenticado.ParceiroId);

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
        pedido.Usuario.Nome,
        _usuarioAutenticado.ParceiroId);

        await _topUsuariosRepository.AddAsync(topUsuario);
    }
}

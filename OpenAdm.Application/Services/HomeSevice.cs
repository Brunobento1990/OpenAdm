using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.TopUsuario;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly ITopUsuariosRepository _topUsuariosRepository;
    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository)
    {
        _topUsuariosRepository = topUsuariosRepository;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();

        return new HomeAdmViewModel()
        {
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x).ToList(),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x).ToList()
        };
    }
}

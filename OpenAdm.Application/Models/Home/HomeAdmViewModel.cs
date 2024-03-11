using OpenAdm.Application.Models.TopUsuario;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public IList<TopUsuariosViewModel> TopUsuariosTotalCompra { get; set; } = new List<TopUsuariosViewModel>();
    public IList<TopUsuariosViewModel> TopUsuariosTotalPedido { get; set; } = new List<TopUsuariosViewModel>();
}

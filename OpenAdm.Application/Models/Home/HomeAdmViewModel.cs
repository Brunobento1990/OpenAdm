using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Application.Models.TopUsuarios;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public IList<TopUsuariosViewModel> TopUsuariosTotalCompra { get; set; } = [];
    public IList<TopUsuariosViewModel> TopUsuariosTotalPedido { get; set; } = [];
    public IList<MovimentoDeProdutoDashBoardModel> Movimentos { get; set; } = [];
}

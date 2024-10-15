using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Application.Models.TopUsuarios;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public IList<TopUsuariosViewModel> TopUsuariosTotalCompra { get; set; } = [];
    public IList<TopUsuariosViewModel> TopUsuariosTotalPedido { get; set; } = [];
    public IList<MovimentoDeProdutoDashBoardModel> Movimentos { get; set; } = [];
    public IList<FaturaPagaDashBoardModel> Faturas { get; set; } = [];
    public decimal TotalAReceber { get; set; }
    public int PedidosEmAberto { get; set; }
}

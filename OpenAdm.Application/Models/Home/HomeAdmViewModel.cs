using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public VariacaoMensalPedidoModel VariacaoMensalPedido { get; set; } = null!;
    public IEnumerable<UsuarioViewModel> UsuarioSemPedidoCpf { get; set; } = [];
    public IEnumerable<UsuarioViewModel> UsuarioSemPedidoCnpj { get; set; } = [];
    public IEnumerable<MovimentoDeProdutoDashBoardModel> Movimentos { get; set; } = [];
    public decimal TotalAReceber { get; set; }
    public IEnumerable<StatusPedidoHomeModel> StatusPedido { get; set; } = [];
    public long QuantidadeDeAcessoEcommerce { get; set; }
    public long QuantidadeDeUsuarioCnpj { get; set; }
    public long QuantidadeDeUsuarioCpf { get; set; }
    public decimal TotalProdutoEstoque { get; set; }
    public decimal TotalProdutoEstoqueReservado { get; set; }
    public int TotalDePedidos { get; set; }
    public IEnumerable<PedidoPorDiaModel> PedidosPorDia { get; set; } = [];
    public IEnumerable<ProdutoMaisVendidoModel> ProdutosMaisVendidos { get; set; } = [];
    public IEnumerable<ProdutoMaisVendidoModel> ProdutosMenosVendidos { get; set; } = [];
}

public class PedidoPorDiaModel
{
    public DateTime Data { get; set; }
    public int Total { get; set; }
    public string DiaSemana => Data.DescricaoDiaSemana();
}

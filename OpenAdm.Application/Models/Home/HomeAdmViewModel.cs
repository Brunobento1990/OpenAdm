using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Models.Home;

public class HomeAdmViewModel
{
    public ResumoMensalHomeViewModel ResumoMensal { get; set; } = new();
    public IEnumerable<MovimentoDeProdutoDashBoardModel> Movimentos { get; set; } = [];
    public IEnumerable<StatusPedidoHomeModel> StatusPedido { get; set; } = [];
    public long QuantidadeDeAcessoEcommerce { get; set; }
    public long QuantidadeDeUsuarioCnpj { get; set; }
    public long QuantidadeDeUsuarioCpf { get; set; }
    public decimal TotalProdutoEstoque { get; set; }
    public decimal TotalProdutoEstoqueReservado { get; set; }
    public decimal QuantidadeProdutoDisponivel => TotalProdutoEstoque - TotalProdutoEstoqueReservado;
    public int TotalDePedidos { get; set; }
    public CobrancaHomeAdmViewModel Cobranca { get; set; } = new();
    public TotalParcelasHomeAdmViewModel Parcelas { get; set; } = new();
    public IEnumerable<PedidoPorDiaModel> PedidosPorDia { get; set; } = [];
    public IEnumerable<ProdutoMaisVendidoModel> ProdutosMaisVendidos { get; set; } = [];
    public IEnumerable<ProdutoMaisVendidoModel> ProdutosMenosVendidos { get; set; } = [];
}

public sealed class TotalParcelasHomeAdmViewModel
{
    public decimal AReceberHoje { get; set; }
    public decimal AReceberSemana { get; set; }
    public decimal APagarHoje { get; set; }
    public decimal APagarSemana { get; set; }
}

public class CobrancaHomeAdmViewModel
{
    public decimal TotalHoje { get; set; }
    public decimal TotalCobranca { get; set; }
    public int QuantidadeACobrar { get; set; }
    public decimal TotalSemana { get; set; }
    public ICollection<ItemCobrancaHomeAdmViewModel> CobrancasMaisAntigas { get; set; } = [];
}

public class ItemCobrancaHomeAdmViewModel
{
    public Guid PedidoId { get; set; }
    public long NumeroPedido { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public int ADias => (DateTime.UtcNow - Data).Days;
}

public class PedidoPorDiaModel
{
    public DateTime Data { get; set; }
    public int Total { get; set; }
    public string DiaSemana => Data.DescricaoDiaSemana();
}

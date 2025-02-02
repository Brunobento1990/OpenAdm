using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.TopUsuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly ITopUsuariosRepository _topUsuariosRepository;
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;
    private readonly IParcelaService _faturaContasAReceberService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IAcessoEcommerceService _acessoEcommerceService;
    private readonly IUsuarioRepository _usuarioRepository;

    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IParcelaService faturaContasAReceberService,
        IPedidoRepository pedidoRepository,
        IAcessoEcommerceService acessoEcommerceService,
        IUsuarioRepository usuarioRepository)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _faturaContasAReceberService = faturaContasAReceberService;
        _pedidoRepository = pedidoRepository;
        _acessoEcommerceService = acessoEcommerceService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();
        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
        var faturas = await _faturaContasAReceberService.FaturasDashBoardAsync();
        var totalAReceber = await _faturaContasAReceberService.GetSumAReceberAsync();
        var pedidosEmAberto = await _pedidoRepository.GetCountPedidosEmAbertoAsync();
        var quantidadeDeAcessoEcommerce = await _acessoEcommerceService.QuantidadeDeAcessoAsync();
        var quantidadeDeUsuario = await _usuarioRepository.GetCountAsync();

        return new HomeAdmViewModel()
        {
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x).ToList(),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x).ToList(),
            Movimentos = movimentos,
            Faturas = faturas,
            TotalAReceber = totalAReceber,
            PedidosEmAberto = pedidosEmAberto,
            QuantidadeDeAcessoEcommerce = quantidadeDeAcessoEcommerce,
            QuantidadeDeUsuario
            = quantidadeDeUsuario
        };
    }
}

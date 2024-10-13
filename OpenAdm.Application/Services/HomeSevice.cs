using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.TopUsuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly ITopUsuariosRepository _topUsuariosRepository;
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;
    private readonly IFaturaContasAReceberService _faturaContasAReceberService;
    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IFaturaContasAReceberService faturaContasAReceberService)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _faturaContasAReceberService = faturaContasAReceberService;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();
        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
        var faturas = await _faturaContasAReceberService.FaturasDashBoardAsync();
        return new HomeAdmViewModel()
        {
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x).ToList(),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x).ToList(),
            Movimentos = movimentos,
            Faturas = faturas
        };
    }
}

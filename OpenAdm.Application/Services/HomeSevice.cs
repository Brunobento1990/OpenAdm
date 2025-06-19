using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.TopUsuarios;
using OpenAdm.Domain.Extensions;
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
    private readonly IEstoqueService _estoqueService;

    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IParcelaService faturaContasAReceberService,
        IPedidoRepository pedidoRepository,
        IAcessoEcommerceService acessoEcommerceService,
        IUsuarioRepository usuarioRepository,
        IEstoqueService estoqueService)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _faturaContasAReceberService = faturaContasAReceberService;
        _pedidoRepository = pedidoRepository;
        _acessoEcommerceService = acessoEcommerceService;
        _usuarioRepository = usuarioRepository;
        _estoqueService = estoqueService;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();
        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
        //var faturas = await _faturaContasAReceberService.FaturasDashBoardAsync();
        var totalAReceber = await _faturaContasAReceberService.GetSumAReceberAsync();
        var pedidosEmAberto = await _pedidoRepository.GetCountStatusPedidosAsync();
        var quantidadeDeAcessoEcommerce = await _acessoEcommerceService.QuantidadeDeAcessoAsync();
        var quantidadeDeUsuarioCpf = await _usuarioRepository.GetCountCpfAsync();
        var quantidadeDeUsuarioCnpj = await _usuarioRepository.GetCountCnpjAsync();
        var estoques = await _estoqueService.GetPosicaoDeEstoqueAsync();
        var variacaoPedido = await _pedidoRepository.ObterHomeAsync();
        var usuariosSemPedido = await _usuarioRepository.UsuariosSemPedidoAsync();

        return new HomeAdmViewModel()
        {
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x),
            Movimentos = movimentos,
            //Faturas = faturas,
            TotalAReceber = totalAReceber,
            StatusPedido = pedidosEmAberto,
            QuantidadeDeAcessoEcommerce = quantidadeDeAcessoEcommerce,
            QuantidadeDeUsuarioCnpj = quantidadeDeUsuarioCnpj,
            QuantidadeDeUsuarioCpf = quantidadeDeUsuarioCpf,
            PosicaoDeEstoques = estoques,
            VariacaoMensalPedido = new()
            {
                Mes = variacaoPedido.Mes.ConverterMesIntEmNome(),
                Porcentagem = variacaoPedido.Porcentagem,
                TotalAnoAtual = variacaoPedido.TotalAnoAtual,
                TotalAnoAnterior = variacaoPedido.TotalAnoAnterior,
                AnoAtual = variacaoPedido.AnoAtual,
                AnoAnterior = variacaoPedido.AnoAnterior
            },
            UsuarioSemPedido = usuariosSemPedido.Select(x => new Models.Usuarios.UsuarioViewModel()
            {
                Cnpj = x.Cnpj,
                Cpf = x.Cpf,
                Id = x.Id,
                Nome = x.Nome,
                Telefone = x.Telefone,
                Numero = x.Numero
            })
        };
    }
}

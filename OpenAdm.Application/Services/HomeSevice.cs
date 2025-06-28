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
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IAcessoEcommerceService _acessoEcommerceService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEstoqueService _estoqueService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IPedidoRepository pedidoRepository,
        IAcessoEcommerceService acessoEcommerceService,
        IUsuarioRepository usuarioRepository,
        IEstoqueService estoqueService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _pedidoRepository = pedidoRepository;
        _acessoEcommerceService = acessoEcommerceService;
        _usuarioRepository = usuarioRepository;
        _estoqueService = estoqueService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync(_usuarioAutenticado.ParceiroId);
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync(_usuarioAutenticado.ParceiroId);
        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
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
            TotalAReceber = 0,
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
            UsuarioSemPedidoCpf = usuariosSemPedido.Where(x => !x.IsAtacado).Select(x => new Models.Usuarios.UsuarioViewModel()
            {
                Cnpj = x.Cnpj,
                Cpf = x.Cpf,
                Id = x.Id,
                Nome = x.Nome,
                Telefone = x.Telefone,
                Numero = x.Numero
            }),
            UsuarioSemPedidoCnpj = usuariosSemPedido.Where(x => x.IsAtacado).Select(x => new Models.Usuarios.UsuarioViewModel()
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

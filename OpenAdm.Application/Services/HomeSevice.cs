using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.TopUsuarios;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

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
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IHomeRepository _homeRepository;

    public HomeSevice(
        ITopUsuariosRepository topUsuariosRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IParcelaService faturaContasAReceberService,
        IPedidoRepository pedidoRepository,
        IAcessoEcommerceService acessoEcommerceService,
        IUsuarioRepository usuarioRepository,
        IEstoqueService estoqueService,
        IUsuarioAutenticado usuarioAutenticado, IHomeRepository homeRepository)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _faturaContasAReceberService = faturaContasAReceberService;
        _pedidoRepository = pedidoRepository;
        _acessoEcommerceService = acessoEcommerceService;
        _usuarioRepository = usuarioRepository;
        _estoqueService = estoqueService;
        _usuarioAutenticado = usuarioAutenticado;
        _homeRepository = homeRepository;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra =
            await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync(_usuarioAutenticado.ParceiroId);
        var topUsuariosTotalPedido =
            await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync(_usuarioAutenticado.ParceiroId);
        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
        //var faturas = await _faturaContasAReceberService.FaturasDashBoardAsync();
        var totalAReceber = await _faturaContasAReceberService.GetSumAReceberAsync();
        var pedidosStatus = await _homeRepository.ObterStatusPedidosAsync();
        var totalPedidos = await _homeRepository.CountDePedidosAsync();
        var quantidadeDeAcessoEcommerce = await _acessoEcommerceService.QuantidadeDeAcessoAsync();
        var quantidadeDeUsuarioCpf = await _usuarioRepository.GetCountCpfAsync();
        var quantidadeDeUsuarioCnpj = await _usuarioRepository.GetCountCnpjAsync();
        var estoques = await _estoqueService.GetPosicaoDeEstoqueAsync();
        var variacaoPedido = await _pedidoRepository.ObterHomeAsync();
        var usuariosSemPedido = await _usuarioRepository.UsuariosSemPedidoAsync();
        
        var dataInicio = DateTime.Today.AddDays(-6);
        var pedidosPorDia = await _homeRepository.ContatorPedido7DiasAsync(dataInicio);

        return new HomeAdmViewModel()
        {
            PedidosPorDia = MontarCountDiario(pedidosPorDia, dataInicio),
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x),
            Movimentos = movimentos,
            //Faturas = faturas,
            TotalDePedidos = totalPedidos,
            TotalAReceber = totalAReceber,
            StatusPedido = pedidosStatus.Select(x => new StatusPedidoHomeModel()
            {
                Status = x.Status,
                Quantidade = x.Quantidade,
                Porcentagem = Math.Round((decimal)x.Quantidade * 100 / totalPedidos, 2),
            }),
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
            UsuarioSemPedidoCpf = usuariosSemPedido.Where(x => !x.IsAtacado).Select(x =>
                new Models.Usuarios.UsuarioViewModel()
                {
                    Cnpj = x.Cnpj,
                    Cpf = x.Cpf,
                    Id = x.Id,
                    Nome = x.Nome,
                    Telefone = x.Telefone,
                    Numero = x.Numero
                }),
            UsuarioSemPedidoCnpj = usuariosSemPedido.Where(x => x.IsAtacado).Select(x =>
                new Models.Usuarios.UsuarioViewModel()
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

    private static IList<PedidoPorDiaModel> MontarCountDiario(IList<ContadorPedidoModel> registros, DateTime dataInicio)
    {
        var porData = registros.ToDictionary(r => r.Data.Date, r => r.Total);
        var resultado = new List<PedidoPorDiaModel>(7);

        for (var i = 0; i < 7; i++)
        {
            var data = dataInicio.AddDays(i);
            resultado.Add(new PedidoPorDiaModel
            {
                Data = data,
                Total = porData.TryGetValue(data, out var total) ? total : 0
            });
        }

        return resultado;
    }
}
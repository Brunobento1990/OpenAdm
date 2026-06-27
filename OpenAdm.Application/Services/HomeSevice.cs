using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IAcessoEcommerceService _acessoEcommerceService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHomeRepository _homeRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly ICachedService<HomeAdmViewModel> _cache;
    private readonly ICobrancaPedidoEcommerceRepository _cobrancaPedidoEcommerceRepository;

    public HomeSevice(
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IPedidoRepository pedidoRepository,
        IAcessoEcommerceService acessoEcommerceService,
        IUsuarioRepository usuarioRepository,
        IHomeRepository homeRepository, IUsuarioAutenticado usuarioAutenticado, ICachedService<HomeAdmViewModel> cache,
        ICobrancaPedidoEcommerceRepository cobrancaPedidoEcommerceRepository)
    {
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _pedidoRepository = pedidoRepository;
        _acessoEcommerceService = acessoEcommerceService;
        _usuarioRepository = usuarioRepository;
        _homeRepository = homeRepository;
        _usuarioAutenticado = usuarioAutenticado;
        _cache = cache;
        _cobrancaPedidoEcommerceRepository = cobrancaPedidoEcommerceRepository;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var key = $"home_{_usuarioAutenticado.ParceiroId}";

        var cache = await _cache.GetItemAsync(key);

        if (cache != null)
        {
            return cache;
        }

        var movimentos = await _movimentacaoDeProdutosService.MovimentoDashBoardAsync();
        var pedidosStatus = await _homeRepository.ObterStatusPedidosAsync();
        var totalPedidos = await _homeRepository.CountDePedidosAsync();
        var quantidadeDeAcessoEcommerce = await _acessoEcommerceService.QuantidadeDeAcessoAsync();
        var quantidadeDeUsuarioCpf = await _usuarioRepository.GetCountCpfAsync();
        var quantidadeDeUsuarioCnpj = await _usuarioRepository.GetCountCnpjAsync();
        var variacaoPedido = await _pedidoRepository.ObterHomeAsync();
        //var usuariosSemPedido = await _usuarioRepository.UsuariosSemPedidoAsync();
        var totalizadorProdutoEstoque = await _homeRepository.ObterTotalizadoProtudoEstoqueAsync();
        var dataInicio = DateTime.Today.AddDays(-6);
        var pedidosPorDia = await _homeRepository.ContatorPedido7DiasAsync(dataInicio);
        var produtosMaisVendidos = await _homeRepository.ProdutosMaisVendidosAsync(false);
        var produtosMenosVendidos = await _homeRepository.ProdutosMaisVendidosAsync(true);

        var totalCobrancaHoje =
            await _cobrancaPedidoEcommerceRepository.TotalACobrarAposAsync(DateTime.Now,
                _usuarioAutenticado.ParceiroId);

        var totalCobrancaSemana =
            await _cobrancaPedidoEcommerceRepository.TotalACobrarAposAsync(DateTime.Now.AddDays(-7),
                _usuarioAutenticado.ParceiroId);

        var totalCobranca = await _cobrancaPedidoEcommerceRepository.TotalACobrarAsync(_usuarioAutenticado.ParceiroId);
        var quantidadeTotalCobranca =
            await _cobrancaPedidoEcommerceRepository.QuantidadeACobrarAsync(_usuarioAutenticado.ParceiroId);
        var cobrancasMaisAntigas = await _cobrancaPedidoEcommerceRepository
            .CobrancasMaisAntigasAsync(_usuarioAutenticado.ParceiroId);

        var pedidosCobrancasMaisAntigas =
            await _pedidoRepository
                .ListarAsync(cobrancasMaisAntigas.Select(x => x.PedidoId));

        ICollection<ItemCobrancaHomeAdmViewModel> cobrancasMaisAntigasModel = [];

        foreach (var cobranca in cobrancasMaisAntigas)
        {
            var pedido = pedidosCobrancasMaisAntigas
                .FirstOrDefault(x => x.PedidoId == cobranca.PedidoId);

            if (pedido == null)
            {
                continue;
            }

            cobrancasMaisAntigasModel.Add(new()
            {
                PedidoId = pedido.PedidoId,
                Cliente = pedido.Cliente,
                NumeroPedido = pedido.NumeroPedido,
                Valor = cobranca.Total,
                Data = cobranca.DataDeCriacao
            });
        }

        cache = new HomeAdmViewModel()
        {
            Cobranca = new()
            {
                TotalHoje = totalCobrancaHoje,
                TotalSemana = totalCobrancaSemana,
                QuantidadeACobrar = quantidadeTotalCobranca,
                TotalCobranca = totalCobranca,
                CobrancasMaisAntigas = cobrancasMaisAntigasModel
            },
            PedidosPorDia = MontarCountDiario(pedidosPorDia, dataInicio),
            Movimentos = movimentos,
            TotalDePedidos = totalPedidos,
            ProdutosMaisVendidos = produtosMaisVendidos,
            ProdutosMenosVendidos = produtosMenosVendidos,
            TotalProdutoEstoque = totalizadorProdutoEstoque?.Quantidade ?? 0,
            TotalProdutoEstoqueReservado = totalizadorProdutoEstoque?.QuantidadeReservada ?? 0,
            StatusPedido = pedidosStatus.Select(x => new StatusPedidoHomeModel()
            {
                Status = x.Status,
                Quantidade = x.Quantidade,
                Porcentagem = Math.Round((decimal)x.Quantidade * 100 / totalPedidos, 2),
            }),
            QuantidadeDeAcessoEcommerce = quantidadeDeAcessoEcommerce,
            QuantidadeDeUsuarioCnpj = quantidadeDeUsuarioCnpj,
            QuantidadeDeUsuarioCpf = quantidadeDeUsuarioCpf,
            VariacaoMensalPedido = new()
            {
                Mes = variacaoPedido.Mes.ConverterMesIntEmNome(),
                Porcentagem = variacaoPedido.Porcentagem,
                TotalAnoAtual = variacaoPedido.TotalAnoAtual,
                TotalAnoAnterior = variacaoPedido.TotalAnoAnterior,
                AnoAtual = variacaoPedido.AnoAtual,
                AnoAnterior = variacaoPedido.AnoAnterior
            },
            // UsuarioSemPedidoCpf = usuariosSemPedido.Where(x => !x.IsAtacado).Select(x =>
            //     new Models.Usuarios.UsuarioViewModel()
            //     {
            //         Cnpj = x.Cnpj,
            //         Cpf = x.Cpf,
            //         Id = x.Id,
            //         Nome = x.Nome,
            //         Telefone = x.Telefone,
            //         Numero = x.Numero
            //     }),
            // UsuarioSemPedidoCnpj = usuariosSemPedido.Where(x => x.IsAtacado).Select(x =>
            //     new Models.Usuarios.UsuarioViewModel()
            //     {
            //         Cnpj = x.Cnpj,
            //         Cpf = x.Cpf,
            //         Id = x.Id,
            //         Nome = x.Nome,
            //         Telefone = x.Telefone,
            //         Numero = x.Numero
            //     })
        };

        await _cache.SetItemAsync(key, cache);

        return cache;
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
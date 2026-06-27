using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class CreatePedidoService : ICreatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;
    private readonly IItensPedidoRepository _itensPedidoRepository;
    private readonly ICobrancaPedidoEcommerceRepository _cobrancaPedidoEcommerceRepository;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarPedidoService processarPedidoService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        ICarrinhoRepository carrinhoRepository,
        IConfiguracoesDePedidoService configuracoesDePedidoService,
        IUsuarioAutenticado usuarioAutenticado,
        IEstoqueRepository estoqueRepository,
        IConfiguracaoDeFreteService configuracaoDeFreteService,
        IItensPedidoRepository itensPedidoRepository,
        ICobrancaPedidoEcommerceRepository cobrancaPedidoEcommerceRepository)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _carrinhoRepository = carrinhoRepository;
        _configuracoesDePedidoService = configuracoesDePedidoService;
        _usuarioAutenticado = usuarioAutenticado;
        _estoqueRepository = estoqueRepository;
        _configuracaoDeFreteService = configuracaoDeFreteService;
        _itensPedidoRepository = itensPedidoRepository;
        _cobrancaPedidoEcommerceRepository = cobrancaPedidoEcommerceRepository;
    }

    public async Task<ResultPartner<CriarPedidoViewModel>> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto)
    {
        var erro = pedidoCreateDto.Validar();
        if (!string.IsNullOrEmpty(erro))
        {
            return (ResultPartner<CriarPedidoViewModel>)erro;
        }

        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();

        if (string.IsNullOrWhiteSpace(usuario.Telefone))
        {
            return (ResultPartner<CriarPedidoViewModel>)
                "Seu cadastro esta incompleto, acesse sua conta e cadastre seu telefone!";
        }

        var cobraFrete = await _configuracaoDeFreteService.CobrarAsync();

        if (cobraFrete.Cobrar &&
            (!pedidoCreateDto.FreteId.HasValue || pedidoCreateDto.FreteId.Value == 0 ||
             !pedidoCreateDto.ValorFrete.HasValue || pedidoCreateDto.ValorFrete.Value == 0))
        {
            return (ResultPartner<CriarPedidoViewModel>)
                $"Seu pedido não foi selecionado um frete";
        }

        var configuracaoDePedido = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        var total = pedidoCreateDto.ItensQuantidadesValidas.Sum(x => x.Quantidade * x.ValorUnitario);

        var pedidoMinimo = usuario.IsAtacado
            ? configuracaoDePedido.PedidoMinimoAtacado
            : configuracaoDePedido.PedidoMinimoVarejo;

        if (pedidoMinimo > total)
        {
            return (ResultPartner<CriarPedidoViewModel>)
                $"Seu pedido não atingiu o mínimo configurado que é de: {pedidoMinimo} reais";
        }

        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, usuario.Id, null);

        var produtosIds = pedidoCreateDto.ItensQuantidadesValidas.Select(x => x.ProdutoId).Distinct().ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);

        var itensTabelaDePrecoDictionary = itensTabelaDePreco
            .ToDictionary(x => (x.ProdutoId, x.PesoId, x.TamanhoId));

        var estoques = await _estoqueRepository.GetPosicaoEstoqueDosProdutosAsync(produtosIds);
        var estoquesReservados = await _itensPedidoRepository.ObterEstoquesReservadosAsync(produtosIds);

        var reservadosDictionary = estoquesReservados
            .ToDictionary(x => (x.ProdutoId, x.PesoId, x.TamanhoId));

        var estoquesDictionary = estoques
            .ToDictionary(x => (x.ProdutoId, x.PesoId, x.TamanhoId));

        foreach (var item in pedidoCreateDto.ItensQuantidadesValidas)
        {
            if (!itensTabelaDePrecoDictionary.TryGetValue((item.ProdutoId, item.PesoId, item.TamanhoId),
                    out var itemTabela))
            {
                return (ResultPartner<CriarPedidoViewModel>)
                    $"Não foi possível localizar o preço do produto: {item.ProdutoId}";
            }

            item.ValorUnitario = usuario.IsAtacado ? itemTabela.ValorUnitarioAtacado : itemTabela.ValorUnitarioVarejo;

            if (configuracaoDePedido.VendaDeProdutoComEstoque || itemTabela.Produto.VendaSomenteComEstoqueDisponivel)
            {
                if (!estoquesDictionary.TryGetValue((item.ProdutoId, item.PesoId, item.TamanhoId), out var estoque))
                {
                    return (ResultPartner<CriarPedidoViewModel>)
                        $"Não foi possível localizar o estoque do produto: {itemTabela.Produto.Descricao}";
                }

                reservadosDictionary.TryGetValue((item.ProdutoId, item.PesoId, item.TamanhoId), out var reservado);

                var posicaoEstoque = new PosicaoEstoqueModel(estoque.Quantidade, reservado?.QuantidadeReservada ?? 0,
                    estoque.Produto.ExigeEstoqueDisponivel(configuracaoDePedido.VendaDeProdutoComEstoque));

                if (!posicaoEstoque.PossuiDisponivel(item.Quantidade))
                {
                    return (ResultPartner<CriarPedidoViewModel>)
                        $"Não há estoque disponível do produto: {itemTabela.Produto.Descricao}, a quantidade disponível é : {posicaoEstoque.Disponivel}";
                }
            }
        }

        pedido.ProcessarItensPedido(pedidoCreateDto.ItensQuantidadesValidas);

        pedido.EnderecoEntrega = new EnderecoEntregaPedido(
            cep: pedidoCreateDto.EnderecoEntrega.Cep,
            logradouro: pedidoCreateDto.EnderecoEntrega.Logradouro,
            bairro: pedidoCreateDto.EnderecoEntrega.Bairro,
            localidade: pedidoCreateDto.EnderecoEntrega.Localidade,
            complemento: pedidoCreateDto.EnderecoEntrega.Complemento ?? "",
            numero: pedidoCreateDto.EnderecoEntrega.Numero,
            uf: pedidoCreateDto.EnderecoEntrega.Uf,
            pedidoId: pedido.Id,
            valorFrete: pedidoCreateDto.ValorFrete ?? 0,
            tipoFrete: pedidoCreateDto.FreteId?.ToString() ?? "",
            id: Guid.NewGuid());

        await _pedidoRepository.AddAsync(pedido);
        await _carrinhoRepository.DeleteCarrinhoAsync(pedido.UsuarioId.ToString());
        await _processarPedidoService.ProcessarCreateAsync(pedido.Id, configuracaoDePedido);

        var resultado = new CriarPedidoViewModel()
        {
            Pedido = new PedidoViewModel().ForModel(pedido),
            Message = "Pedido cadastrado com sucesso!",
        };

        var proximoNumeroCobranca =
            await _cobrancaPedidoEcommerceRepository.ProximoNumeroAsync(_usuarioAutenticado.ParceiroId);

        var cobranca = CobrancaPedidoEcommerce.Novo(
            pedido.Id,
            pedido.ValorTotal,
            numero: proximoNumeroCobranca,
            _usuarioAutenticado.ParceiroId);

        await _cobrancaPedidoEcommerceRepository.AddAsync(cobranca);
        await _cobrancaPedidoEcommerceRepository.SaveChangesAsync();

        resultado.Redirect = $"/pedido/cobranca/{pedido.Id}";

        return (ResultPartner<CriarPedidoViewModel>)resultado;
    }
}
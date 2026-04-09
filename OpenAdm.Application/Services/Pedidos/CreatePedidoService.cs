using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
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
    private readonly IFaturaService _contasAReceberService;
    private readonly IConfiguracaoDePagamentoService _configuracaoDePagamentoService;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarPedidoService processarPedidoService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        ICarrinhoRepository carrinhoRepository,
        IFaturaService contasAReceberService,
        IConfiguracaoDePagamentoService configuracaoDePagamentoService,
        IConfiguracoesDePedidoService configuracoesDePedidoService,
        IUsuarioAutenticado usuarioAutenticado,
        IEstoqueRepository estoqueRepository, IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _carrinhoRepository = carrinhoRepository;
        _contasAReceberService = contasAReceberService;
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
        _configuracoesDePedidoService = configuracoesDePedidoService;
        _usuarioAutenticado = usuarioAutenticado;
        _estoqueRepository = estoqueRepository;
        _configuracaoDeFreteService = configuracaoDeFreteService;
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

        var estoques = await _estoqueRepository
            .GetPosicaoEstoqueDosProdutosAsync(produtosIds);

        foreach (var item in pedidoCreateDto.ItensQuantidadesValidas)
        {
            var itemTabela = itensTabelaDePreco
                .FirstOrDefault(itemTabelaDePreco =>
                    itemTabelaDePreco.ProdutoId == item.ProdutoId &&
                    itemTabelaDePreco.PesoId == item.PesoId &&
                    itemTabelaDePreco.TamanhoId == item.TamanhoId);

            if (itemTabela == null)
            {
                return (ResultPartner<CriarPedidoViewModel>)
                    $"Não foi possível localizar o preço do produto: {item.ProdutoId}";
            }

            item.ValorUnitario = usuario.IsAtacado ? itemTabela.ValorUnitarioAtacado : itemTabela.ValorUnitarioVarejo;

            if (configuracaoDePedido.VendaDeProdutoComEstoque || itemTabela.Produto.VendaSomenteComEstoqueDisponivel)
            {
                var estoque = estoques.FirstOrDefault(x => x.ProdutoId == item.ProdutoId &&
                                                           x.PesoId == item.PesoId &&
                                                           x.TamanhoId == item.TamanhoId);
                if (estoque == null)
                {
                    return (ResultPartner<CriarPedidoViewModel>)
                        $"Não foi possível localizar o estoque do produto: {item.ProdutoId}";
                }

                if (estoque.QuantidadeDisponivel < item.Quantidade)
                {
                    return (ResultPartner<CriarPedidoViewModel>)
                        $"Não há estoque disponível do produto: {itemTabela.Produto.Descricao}, a quantidade disponível é : {estoque.Quantidade}";
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

        var configPagamento = await _configuracaoDePagamentoService.CobrarAsync();

        var resultado = new CriarPedidoViewModel()
        {
            Pedido = new PedidoViewModel().ForModel(pedido),
            Message = "Pedido cadastrado com sucesso!",
        };

        if (!configPagamento.Cobrar)
        {
            await _contasAReceberService.CriarContasAReceberAsync(new()
            {
                DataDoPrimeiroVencimento = DateTime.Now.AddMonths(1),
                Desconto = null,
                MeioDePagamento = null,
                Observacao = $"Pedido: {pedido.Numero}",
                PedidoId = pedido.Id,
                QuantidadeDeParcelas = 1,
                Total = pedido.ValorTotal,
                UsuarioId = pedido.UsuarioId,
                Tipo = TipoFaturaEnum.A_Receber
            });

            return (ResultPartner<CriarPedidoViewModel>)resultado;
        }

        resultado.Redirect = $"/pedido/cobranca/{pedido.Id}";

        return (ResultPartner<CriarPedidoViewModel>)resultado;
    }
}
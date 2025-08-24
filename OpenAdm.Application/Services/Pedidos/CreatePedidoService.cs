using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using System.Threading.Channels;

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
    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarPedidoService processarPedidoService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        ICarrinhoRepository carrinhoRepository,
        IFaturaService contasAReceberService,
        IConfiguracaoDePagamentoService configuracaoDePagamentoService,
        IConfiguracoesDePedidoService configuracoesDePedidoService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _carrinhoRepository = carrinhoRepository;
        _contasAReceberService = contasAReceberService;
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
        _configuracoesDePedidoService = configuracoesDePedidoService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto)
    {
        pedidoCreateDto.Validar();
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();

        if (string.IsNullOrWhiteSpace(usuario.Telefone))
        {
            throw new ExceptionApi("Seu cadastro esta incompleto, acesse sua conta e cadastre seu telefone!");
        }

        var pedidoMinimo = await _configuracoesDePedidoService.GetPedidoMinimoAsync();
        var total = pedidoCreateDto.Itens.Sum(x => x.Quantidade * x.ValorUnitario);

        if (pedidoMinimo.PedidoMinimo > total)
        {
            throw new ExceptionApi("Seu pedido não atingiu o mínimo de valor!");
        }

        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, usuario.Id, null);

        var produtosIds = pedidoCreateDto.Itens.Select(x => x.ProdutoId).ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);

        foreach (var item in pedidoCreateDto.Itens)
        {
            var itemTabela = itensTabelaDePreco
                .FirstOrDefault(itemTabelaDePreco =>
                    itemTabelaDePreco.ProdutoId == item.ProdutoId &&
                    itemTabelaDePreco.PesoId == item.PesoId &&
                    itemTabelaDePreco.TamanhoId == item.TamanhoId)
                ?? throw new Exception($"Não foi possível localizar o preço do produto: {item.ProdutoId}");

            item.ValorUnitario = usuario.IsAtacado ? itemTabela.ValorUnitarioAtacado : itemTabela.ValorUnitarioVarejo;
        }

        pedido.ProcessarItensPedido(pedidoCreateDto.Itens);
        pedido.EnderecoEntrega = new EnderecoEntregaPedido(
            cep: pedidoCreateDto.EnderecoEntrega.Cep,
            logradouro: pedidoCreateDto.EnderecoEntrega.Logradouro,
            bairro: pedidoCreateDto.EnderecoEntrega.Bairro,
            localidade: pedidoCreateDto.EnderecoEntrega.Localidade,
            complemento: pedidoCreateDto.EnderecoEntrega.Complemento ?? "",
            numero: pedidoCreateDto.EnderecoEntrega.Numero,
            uf: pedidoCreateDto.EnderecoEntrega.Uf,
            pedidoId: pedido.Id,
            valorFrete: 0,
            tipoFrete: "",
            id: Guid.NewGuid());

        await _pedidoRepository.AddAsync(pedido);
        await _carrinhoRepository.DeleteCarrinhoAsync(pedido.UsuarioId.ToString());

        //if (!_channel.Channel.Writer.TryWrite(pedido))
        //{
        //    await _processarPedidoService.ProcessarCreateAsync(pedido.Id);
        //}

        await _processarPedidoService.ProcessarCreateAsync(pedido.Id);

        var configPagamento = await _configuracaoDePagamentoService.CobrarAsync();

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
        }

        return new PedidoViewModel().ForModel(pedido);
    }
}

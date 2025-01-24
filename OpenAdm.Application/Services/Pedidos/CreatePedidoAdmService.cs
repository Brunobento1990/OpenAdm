using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class CreatePedidoAdmService : ICreatePedidoAdmService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly IFaturaService _faturaService;
    private readonly IUsuarioService _usuarioService;

    public CreatePedidoAdmService(
        IPedidoRepository pedidoRepository,
        IProcessarPedidoService processarPedidoService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        IFaturaService faturaService,
        IUsuarioService usuarioService)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _faturaService = faturaService;
        _usuarioService = usuarioService;
    }

    public async Task<bool> CreateAsync(PedidoAdmCreateDto pedidoAdmCreateDto)
    {
        if (pedidoAdmCreateDto.Itens.Count == 0)
        {
            throw new ExceptionApi("Informe os itens do pedido!");
        }
        var usuario = await _usuarioService.GetUsuarioByIdValidacaoAsync(id: pedidoAdmCreateDto.UsuarioId);
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, usuario.Id, null);

        var produtosIds = pedidoAdmCreateDto.Itens.Select(x => x.ProdutoId).ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);

        pedido.ProcessarItensPedido(pedidoAdmCreateDto.Itens);

        await _pedidoRepository.AddAsync(pedido);
        await _processarPedidoService.ProcessarCreateAsync(pedido.Id);


        await _faturaService.CriarContasAReceberAsync(new()
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

        return true;
    }
}

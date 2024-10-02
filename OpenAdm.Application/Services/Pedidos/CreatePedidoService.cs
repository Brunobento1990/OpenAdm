using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class CreatePedidoService : ICreatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly ICarrinhoRepository _carrinhoRepository;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarPedidoService processarPedidoService,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        ICarrinhoRepository carrinhoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _carrinhoRepository = carrinhoRepository;
    }

    public async Task<PedidoViewModel> CreatePedidoAsync(IList<ItemPedidoModel> itensPedidoModels, Usuario usuario)
    {
        if (itensPedidoModels.Count == 0)
        {
            throw new ExceptionApi("Informe os itens do pedido!");
        }
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, usuario.Id);

        var produtosIds = itensPedidoModels.Select(x => x.ProdutoId).ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);

        foreach (var itemTabelaDePreco in itensTabelaDePreco)
        {
            var preco = itensPedidoModels
                .FirstOrDefault(itemPedido =>
                    itemPedido.ProdutoId == itemTabelaDePreco.ProdutoId &&
                    itemPedido.PesoId == itemTabelaDePreco.PesoId &&
                    itemPedido.TamanhoId == itemTabelaDePreco.TamanhoId);
        
            if (preco != null)
            {
                if (usuario.IsAtacado && preco.ValorUnitario != itemTabelaDePreco.ValorUnitarioAtacado)
                {
                    throw new Exception($"Os valores unitários do pedido estão incorretos: usuarioId: {usuario.Id}");
                }
                throw new Exception($"Os valores unitários do pedido estão incorretos: usuarioId: {usuario.Id}");
            }
        }

        pedido.ProcessarItensPedido(itensPedidoModels);

        await _pedidoRepository.AddAsync(pedido);
        await _carrinhoRepository.DeleteCarrinhoAsync(pedido.UsuarioId.ToString());
        await _processarPedidoService.ProcessarCreateAsync(pedido.Id);

        return new PedidoViewModel().ForModel(pedido);
    }
}

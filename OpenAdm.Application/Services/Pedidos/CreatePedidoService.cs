using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Domain.Pkg.Exceptions;
using Domain.Pkg.Model;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class CreatePedidoService : ICreatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository, 
        IProcessarPedidoService processarPedidoService, 
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
    }

    public async Task<PedidoViewModel> CreatePedidoAsync(IList<ItensPedidoModel> itensPedidoModels, Usuario usuario)
    {
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
            }
        }

        pedido.ProcessarItensPedido(itensPedidoModels);

        await _pedidoRepository.AddAsync(pedido);
        await _processarPedidoService.ProcessarCreateAsync(pedido.Id);

        return new PedidoViewModel().ForModel(pedido);
    }
}

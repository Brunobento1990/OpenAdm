using OpenAdm.Application.Dtos.Bases;
using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using OpenAdm.Application.Models.EnderecosEntregaPedidos;

namespace OpenAdm.Application.Models.Pedidos;

public class PedidoViewModel : BaseViewModel
{
    public StatusPedido StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public IList<ItensPedidoViewModel> ItensPedido { get; set; } = [];
    public EnderecoEntregaPedidoViewModel? EnderecoEntrega { get; set; }
    public PedidoViewModel ForModel(Pedido entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        Numero = entity.Numero;
        ValorTotal = entity.ValorTotal;
        StatusPedido = entity.StatusPedido;
        ItensPedido = entity.ItensPedido.Select(x => 
            new ItensPedidoViewModel().ToModel(x)
        ).ToList();

        if (entity.Usuario != null)
            Usuario = entity.Usuario.Nome;

        return this;
    }
}

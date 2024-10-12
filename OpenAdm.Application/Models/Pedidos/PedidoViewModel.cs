using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Pedidos;

public class PedidoViewModel : BaseViewModel
{
    public StatusPedido StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TotalItens { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public IList<ItensPedidoViewModel> ItensPedido { get; set; } = [];

    public PedidoViewModel ForModelPedidoEmAberto(Pedido entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        Numero = entity.Numero;
        ValorTotal = entity.ValorTotal;
        StatusPedido = entity.StatusPedido;
        TotalItens = entity.ItensPedido.Sum(x => x.Quantidade);
        DataDeAtualizacao = entity.DataDeAtualizacao;
        if (entity.Usuario != null)
            Usuario = entity.Usuario.Nome;

        return this;
    }

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

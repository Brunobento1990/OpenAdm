using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enums;

namespace OpenAdm.Application.Models.Pedidos;

public class PedidoViewModel : BaseViewModel
{
    public StatusPedido StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public PedidoViewModel ForModel(Pedido entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        Numero = entity.Numero;
        ValorTotal = entity.ValorTotal;
        StatusPedido = entity.StatusPedido;

        if (entity.Usuario != null)
            Usuario = entity.Usuario.Nome;

        return this;
    }
}

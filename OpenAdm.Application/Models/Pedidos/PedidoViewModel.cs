using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Pedidos;

public class PedidoViewModel : BaseViewModel
{
    public StatusPedido StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TotalItens { get; set; }
    public decimal TotalAReceber { get; set; }
    public decimal PorcentagemEstoqueDisponivel
    {
        get
        {
            if (ItensPedido == null || !ItensPedido.Any())
                return 0;

            var porcentagens = ItensPedido.Select(x =>
            {
                if (x.Quantidade == 0) return 100m;
                return Math.Min(100m, (x.EstoqueAtual * 100m) / x.Quantidade);
            });

            return porcentagens.Min();
        }
    }
    public string Usuario { get; set; } = string.Empty;
    public bool TemEstoqueDisponivel => ItensPedido.Where(x => x.TemEstoqueDisponivel).Count() == ItensPedido.Count;
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

        TotalAReceber = entity.Fatura?.ValorAPagarAReceber ?? 0;

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

        TotalAReceber = entity.Fatura?.ValorAPagarAReceber ?? 0;

        return this;
    }
}

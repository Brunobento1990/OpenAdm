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
    public decimal PorcentagemEstoqueAtendido
    {
        get
        {
            if (ItensPedido == null || !ItensPedido.Any())
                return 0;

            var totalPedido = ItensPedido.Sum(x => x.Quantidade);
            var totalAtendido = ItensPedido.Sum(x => x.EstoqueDisponivel);

            if (totalPedido == 0) return 0;

            var percentualAtendido = (totalAtendido / (decimal)totalPedido) * 100m;
            return Math.Round(percentualAtendido, 0);
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

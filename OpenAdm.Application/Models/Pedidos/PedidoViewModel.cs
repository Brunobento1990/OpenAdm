using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.EnderecosEntregasPedido;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Pedidos;

public class PedidoViewModel : BaseViewModel
{
    public StatusPedido StatusPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TotalItens { get; set; }
    public decimal TotalAReceber { get; set; }
    public EnderecoEntregaPedidoViewModel? EnderecoEntrega { get; set; }

    public decimal PorcentagemEstoqueAtendido { get; set; }

    public string Usuario { get; set; } = string.Empty;
    public bool TemEstoqueDisponivel { get; set; }

    public void SetarPorcentagemEstoqueAtendido(ICollection<ItensPedidoViewModel> itens)
    {
        if (itens == null || !itens.Any())
        {
            return;
        }

        var totalPedido = itens.Sum(x => x.Quantidade);
        var totalAtendido = itens.Sum(x => x.EstoqueDisponivel);

        if (totalPedido == 0)
        {
            return;
        }

        var percentualAtendido = (totalAtendido / (decimal)totalPedido) * 100m;
        PorcentagemEstoqueAtendido = Math.Round(percentualAtendido, 0);
    }

    public void SetarTemEstoqueDisponivel(ICollection<ItensPedidoViewModel> itens)
    {
        TemEstoqueDisponivel = itens.Where(x => x.TemEstoqueDisponivel).Count() == itens.Count;
    }

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

        EnderecoEntrega = entity.EnderecoEntrega == null
            ? null
            : (EnderecoEntregaPedidoViewModel)entity.EnderecoEntrega;
        TotalAReceber = entity.Fatura?.ValorAPagarAReceber ?? 0;

        return this;
    }
}
using OpenAdm.Domain.Enums;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Domain.Validations;

namespace OpenAdm.Domain.Entities;

public sealed class Pedido : BaseEntity
{
    public Pedido(
        Guid id, 
        DateTime dataDeCriacao, 
        DateTime dataDeAtualizacao, 
        long numero, 
        StatusPedido statusPedido, 
        Guid usuarioId) 
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ValidationGuid.ValidGuidNullAndEmpty(usuarioId);
        StatusPedido = statusPedido;
        UsuarioId = usuarioId;
    }

    public StatusPedido StatusPedido { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal ValorTotal { get { return ItensPedido.Sum(x => x.ValorTotal); } }
    public IList<ItensPedido> ItensPedido { get; set; } = new List<ItensPedido>();

    public void UpdateStatus(StatusPedido statusPedido)
    {
        if (StatusPedido == StatusPedido.Entregue)
            throw new ExceptionApi(CodigoErrors.StatusDoPedidoEntregue);

        StatusPedido = statusPedido;
    }

    public void ProcessarItensPedido(
        IList<PedidoPorPesoModel> pedidoPorPesoModels, 
        IList<PedidoPorTamanhoModel> pedidoPorTamanhoModels, 
        TabelaDePreco tabelaDePreco)
    {
        if (pedidoPorPesoModels.Count == 0 && pedidoPorTamanhoModels.Count == 0)
            throw new ExceptionApi(CodigoErrors.PedidoSemItens);

        if (tabelaDePreco == null || tabelaDePreco.ItensTabelaDePreco.Count == 0)
            throw new Exception(CodigoErrors.PedidoSemItens);

        foreach (var pedidoPorTamanhoModel in pedidoPorTamanhoModels)
        {
            var valorUnitario = tabelaDePreco
                .ItensTabelaDePreco
                .FirstOrDefault(item => item.ProdutoId == pedidoPorTamanhoModel.ProdutoId && item.TamanhoId == pedidoPorTamanhoModel.TamanhoId)?.ValorUnitario ?? 0;

            ItensPedido.Add(new ItensPedido(
                id: Guid.NewGuid(),
                dataDeCriacao: DataDeCriacao,
                dataDeAtualizacao: DataDeAtualizacao,
                numero: 0,
                pesoId: null,
                tamanhoId: pedidoPorTamanhoModel.TamanhoId,
                produtoId: pedidoPorTamanhoModel.ProdutoId,
                pedidoId: Id,
                valorUnitario:
                valorUnitario,
                quantidade: pedidoPorTamanhoModel.Quantidade));
        };

        foreach (var pedidoPorPesoModel in pedidoPorPesoModels)
        {
            var valorUnitario = tabelaDePreco
                .ItensTabelaDePreco
                .FirstOrDefault(item => item.ProdutoId == pedidoPorPesoModel.ProdutoId && item.PesoId == pedidoPorPesoModel.PesoId)?.ValorUnitario ?? 0;

            ItensPedido.Add(new ItensPedido(
                id: Guid.NewGuid(),
                dataDeCriacao: DataDeCriacao,
                dataDeAtualizacao: DataDeAtualizacao,
                numero: 0,
                pesoId: pedidoPorPesoModel.PesoId,
                tamanhoId: null,
                produtoId: pedidoPorPesoModel.ProdutoId,
                pedidoId: Id,
                valorUnitario:
                valorUnitario,
                quantidade: pedidoPorPesoModel.Quantidade));
        };
    }
}
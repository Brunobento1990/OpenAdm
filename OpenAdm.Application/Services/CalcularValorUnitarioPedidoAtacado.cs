using Domain.Pkg.Entities;
using Domain.Pkg.Interfaces;

namespace OpenAdm.Application.Services;

public class CalcularValorUnitarioPedidoAtacado : ICalcularValorUnitarioPedido
{
    private readonly IList<ItensTabelaDePreco> _itensTabelaDePreco;

    public CalcularValorUnitarioPedidoAtacado(IList<ItensTabelaDePreco> itensTabelaDePreco)
    {
        _itensTabelaDePreco = itensTabelaDePreco;
    }

    public decimal GetValorUnitarioByPesoId(Guid produtoId, Guid? pesoId)
    {
        var itemTabelaDePreco = _itensTabelaDePreco
        .FirstOrDefault(x => x.ProdutoId == produtoId && x.PesoId == pesoId);

        itemTabelaDePreco ??= _itensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId);

        return itemTabelaDePreco?.ValorUnitarioAtacado ?? 0;
    }

    public decimal GetValorUnitarioByTamanhoId(Guid produtoId, Guid? tamanhoId)
    {
        var itemTabelaDePreco = _itensTabelaDePreco
        .FirstOrDefault(x => x.ProdutoId == produtoId && x.TamanhoId == tamanhoId);

        itemTabelaDePreco ??= _itensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId);

        return itemTabelaDePreco?.ValorUnitarioAtacado ?? 0;
    }
}

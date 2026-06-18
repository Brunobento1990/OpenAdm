using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Helpers;

public static class ProdutoEcommerceHelper
{
    public static decimal ValorUnitarioProduto(
        string? cnpj,
        decimal? valorUnitarioAtacado,
        decimal? valorUnitarioVarejo)
    {
        var isAtacado = !string.IsNullOrWhiteSpace(cnpj);

        if (isAtacado)
        {
            return valorUnitarioAtacado ?? 0;
        }

        return valorUnitarioVarejo ?? 0;
    }

    public static PosicaoEstoqueModel ObterPosicao(
        Produto produto,
        Estoque? estoque,
        ConfiguracoesDePedido config,
        decimal reservado)
    {
        return new PosicaoEstoqueModel(
            estoque?.Quantidade ?? 0,
            reservado,
            produto.ExigeEstoqueDisponivel(config.VendaDeProdutoComEstoque));
    }

    public static void AplicarEstoque(
        Produto produto,
        ConfiguracoesDePedido config,
        Estoque? estoque,
        EstoqueReservadoModel? estoqueReservado,
        out decimal quantidadeDisponivel,
        out bool temEstoqueDisponivel)
    {
        var posicao = ObterPosicao(
            produto,
            estoque,
            config,
            estoqueReservado?.QuantidadeReservada ?? 0);

        quantidadeDisponivel = posicao.Disponivel;
        temEstoqueDisponivel = posicao.TemEstoqueDisponivel;
    }
}
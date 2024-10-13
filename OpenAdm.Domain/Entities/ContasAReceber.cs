using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class ContasAReceber : BaseEntity
{
    public ContasAReceber(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusContasAPagarEnum status,
        Guid usuarioId,
        Guid? pedidoId,
        DateTime? dataDeFechamento)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Status = status;
        UsuarioId = usuarioId;
        PedidoId = pedidoId;
        DataDeFechamento = dataDeFechamento;
    }

    public StatusContasAPagarEnum Status { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public Guid? PedidoId { get; private set; }
    public Pedido? Pedido { get; set; }
    public DateTime? DataDeFechamento { get; private set; }
    public IList<FaturaContasAReceber> Faturas { get; set; } = [];
    public decimal Total { get { return Faturas.Sum(x => x.Valor); } }

    public void Fechar()
    {
        DataDeFechamento = DateTime.Now;
        Status = StatusContasAPagarEnum.Paga;
        DataDeAtualizacao = DateTime.Now;
    }

    public void PagaParcialmente()
    {
        Status = StatusContasAPagarEnum.Paga_Parcialmente;
        DataDeAtualizacao = DateTime.Now;
    }

    public static ContasAReceber NovaContasAReceber(
        Guid usuarioId,
        Guid? pedidoId,
        decimal total,
        int quantidadeDeParcelas,
        DateTime primeiroVencimento,
        MeioDePagamentoEnum? meioDePagamento,
        decimal? desconto,
        string? observacao)
    {
        var contasAReceber = new ContasAReceber(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusContasAPagarEnum.Aberta,
            usuarioId: usuarioId,
            pedidoId: pedidoId,
            dataDeFechamento: null);

        var valorDaParcela = total / quantidadeDeParcelas;

        for (int i = 1; i <= quantidadeDeParcelas; i++)
        {
            var novoVencimento = i == 1 ? primeiroVencimento : primeiroVencimento.AddMonths(i);
            contasAReceber.Faturas.Add(FaturaContasAReceber.NovaFatura(
                dataDeVencimento: novoVencimento,
                numeroDaFatura: i,
                meioDePagamento: null,
                valor: valorDaParcela,
                desconto: desconto,
                observacao: observacao,
                contasAReceberId: contasAReceber.Id));
        }

        var totalValidacao = contasAReceber.Faturas.Sum(x => x.Valor);

        if (totalValidacao > total)
        {
            var diferenca = totalValidacao - total;
            var primeiraParcela = contasAReceber.Faturas.FirstOrDefault();
            primeiraParcela?.TirarDiferenca(diferenca);
        }
        else if (totalValidacao < total)
        {
            var diferenca = total - totalValidacao;
            var primeiraParcela = contasAReceber.Faturas.FirstOrDefault();
            primeiraParcela?.AdicionarDiferenca(diferenca);
        }

        return contasAReceber;
    }
}

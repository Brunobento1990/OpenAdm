using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Entities;

public sealed class FaturaContasAReceber : BaseEntity
{
    public FaturaContasAReceber(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusFaturaContasAReceberEnum status,
        DateTime dataDeVencimento,
        int numeroDaFatura,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao,
        Guid contasAReceberId,
        DateTime? dataDePagamento)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Status = status;
        DataDeVencimento = dataDeVencimento;
        NumeroDaFatura = numeroDaFatura;
        MeioDePagamento = meioDePagamento;
        Valor = valor;
        Desconto = desconto;
        Observacao = observacao;
        ContasAReceberId = contasAReceberId;
        DataDePagamento = dataDePagamento;
    }

    public StatusFaturaContasAReceberEnum Status { get; private set; }
    public DateTime DataDeVencimento { get; private set; }
    public DateTime? DataDePagamento { get; private set; }
    public int NumeroDaFatura { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public decimal Valor { get; private set; }
    public decimal? Desconto { get; private set; }
    public string? Observacao { get; private set; }
    public Guid ContasAReceberId { get; private set; }
    public ContasAReceber ContasAReceber { get; set; } = null!;
    public void TirarDiferenca(decimal diferenca)
    {
        Valor -= diferenca;
    }
    public void AdicionarDiferenca(decimal diferenca)
    {
        Valor += diferenca;
    }

    public void Pagar(
        decimal? desconto,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao)
    {
        if (Status == StatusFaturaContasAReceberEnum.Pago)
        {
            throw new ExceptionApi($"A fatura: {Numero} já se encontra paga!");
        }

        Desconto = desconto;
        Observacao = observacao;
        MeioDePagamento = meioDePagamento;
        Status = StatusFaturaContasAReceberEnum.Pago;
        DataDePagamento = DateTime.Now;
    }

    public static FaturaContasAReceber NovaFatura(
        DateTime dataDeVencimento,
        int numeroDaFatura,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao,
        Guid contasAReceberId
        )
    {
        return new FaturaContasAReceber(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusFaturaContasAReceberEnum.Pendente,
            dataDeVencimento: dataDeVencimento,
            numeroDaFatura: numeroDaFatura,
            meioDePagamento: meioDePagamento,
            valor: valor,
            desconto: desconto,
            observacao: observacao,
            contasAReceberId: contasAReceberId,
            dataDePagamento: null);
    }
}

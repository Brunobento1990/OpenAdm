using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Entities;

public sealed class Parcela : BaseEntity
{
    public Parcela(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusParcelaEnum status,
        DateTime dataDeVencimento,
        int numeroDaFatura,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao,
        Guid faturaId,
        DateTime? dataDePagamento,
        string? idExterno)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Status = status;
        DataDeVencimento = dataDeVencimento;
        NumeroDaFatura = numeroDaFatura;
        MeioDePagamento = meioDePagamento;
        Valor = valor;
        Desconto = desconto;
        Observacao = observacao;
        FaturaId = faturaId;
        DataDePagamento = dataDePagamento;
        IdExterno = idExterno;
    }

    public StatusParcelaEnum Status { get; private set; }
    public DateTime DataDeVencimento { get; private set; }
    public DateTime? DataDePagamento { get; private set; }
    public int NumeroDaFatura { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public decimal Valor { get; private set; }
    public decimal? Desconto { get; private set; }
    public string? Observacao { get; private set; }
    public string? IdExterno { get; private set; }
    public Guid FaturaId { get; private set; }
    public Fatura Fatura { get; set; } = null!;

    public void Edit(
        StatusParcelaEnum status,
        DateTime dataDeVencimento,
        DateTime? dataDePagamento,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao)
    {
        Status = status;
        DataDeVencimento = dataDeVencimento;
        DataDePagamento = dataDePagamento;
        MeioDePagamento = meioDePagamento;
        Valor = valor;
        Desconto = desconto;
        Observacao = observacao;
    }
    public void TirarDiferenca(decimal diferenca)
    {
        Valor -= diferenca;
    }
    public void AdicionarDiferenca(decimal diferenca)
    {
        Valor += diferenca;
    }

    public void PagarWebHook()
    {
        Status = StatusParcelaEnum.Pago;
        DataDePagamento = DateTime.Now;
    }

    public void Pagar(
        decimal? desconto,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao)
    {
        if (Status == StatusParcelaEnum.Pago)
        {
            throw new ExceptionApi($"A fatura: {Numero} já se encontra paga!");
        }

        Desconto = desconto;
        Observacao = observacao;
        MeioDePagamento = meioDePagamento;
        Status = StatusParcelaEnum.Pago;
        DataDePagamento = DateTime.Now;
    }

    public static Parcela NovaFatura(
        DateTime dataDeVencimento,
        int numeroDaFatura,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao,
        Guid faturaId,
        string? idExterno
        )
    {
        return new Parcela(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusParcelaEnum.Pendente,
            dataDeVencimento: dataDeVencimento,
            numeroDaFatura: numeroDaFatura,
            meioDePagamento: meioDePagamento,
            valor: valor,
            desconto: desconto,
            observacao: observacao,
            faturaId: faturaId,
            dataDePagamento: null,
            idExterno: idExterno);
    }
}

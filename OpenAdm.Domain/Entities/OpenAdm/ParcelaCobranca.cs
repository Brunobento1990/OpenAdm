using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities.OpenAdm;

public class ParcelaCobranca
{
    public ParcelaCobranca(Guid id, DateTime dataDeCadastro, DateTime dataDeVencimento, DateTime? dataDePagamento,
        int numero, int mesCobranca, int anoCobranca, decimal valor, decimal valorPago,
        TipoParcelaCobrancaEnum tipoParcelaCobranca, Guid empresaOpenAdmId, string? idExterno)
    {
        Id = id;
        DataDeCadastro = dataDeCadastro;
        DataDeVencimento = dataDeVencimento;
        DataDePagamento = dataDePagamento;
        Numero = numero;
        MesCobranca = mesCobranca;
        AnoCobranca = anoCobranca;
        Valor = valor;
        ValorPago = valorPago;
        TipoParcelaCobranca = tipoParcelaCobranca;
        EmpresaOpenAdmId = empresaOpenAdmId;
        IdExterno = idExterno;
    }

    public Guid Id { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    public DateTime DataDeVencimento { get; private set; }
    public DateTime? DataDePagamento { get; private set; }

    public int Numero { get; private set; }
    public int MesCobranca { get; private set; }
    public int AnoCobranca { get; private set; }

    public decimal Valor { get; private set; }
    public decimal ValorPago { get; private set; }
    public string? IdExterno { get; private set; }
    public TipoParcelaCobrancaEnum TipoParcelaCobranca { get; private set; }
    public Guid EmpresaOpenAdmId { get; private set; }
    public EmpresaOpenAdm EmpresaOpenAdm { get; set; } = null!;
    public bool Pago => ValorPago >= Valor;
    public bool Vencido => !Pago && DateTime.UtcNow.Date > DataDeVencimento.Date;

    public static ParcelaCobranca NovaCobranca(
        Guid empresaOpenAdmId,
        int numero,
        int mesCobranca,
        int anoCobranca,
        decimal valor,
        DateTime dataDeVencimento,
        TipoParcelaCobrancaEnum tipoParcelaCobranca)
    {
        return new ParcelaCobranca(
            id: Guid.NewGuid(),
            dataDeCadastro: DateTime.Now,
            dataDeVencimento: dataDeVencimento,
            dataDePagamento: null,
            numero: numero,
            mesCobranca: mesCobranca,
            anoCobranca: anoCobranca,
            valor: valor,
            valorPago: 0,
            tipoParcelaCobranca: tipoParcelaCobranca,
            empresaOpenAdmId: empresaOpenAdmId,
            idExterno: null);
    }

    public void Pagar(decimal valor)
    {
        ValorPago += valor;
        DataDePagamento = DateTime.Now;
    }
}
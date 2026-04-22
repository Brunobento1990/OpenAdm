using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Application.Models;

public class ParcelaCobrancaViewModel : BaseViewModel
{
    public DateTime DataDeVencimento { get; set; }
    public DateTime? DataDePagamento { get; set; }
    public int MesCobranca { get; set; }
    public int AnoCobranca { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorPago { get; set; }
    public bool Pago { get; set; }
    public bool Vencido { get; set; }
    public PixViewModel? Pix { get; set; }

    public static explicit operator ParcelaCobrancaViewModel(ParcelaCobranca parcelaCobranca)
    {
        return new ParcelaCobrancaViewModel()
        {
            DataDeCriacao = parcelaCobranca.DataDeCadastro,
            Id = parcelaCobranca.Id,
            Numero = parcelaCobranca.Numero,
            AnoCobranca = parcelaCobranca.AnoCobranca,
            DataDePagamento = parcelaCobranca.DataDePagamento,
            MesCobranca = parcelaCobranca.MesCobranca,
            Valor = parcelaCobranca.Valor,
            ValorPago = parcelaCobranca.ValorPago,
            Pago = parcelaCobranca.Pago,
            Vencido = parcelaCobranca.Vencido,
            DataDeVencimento = parcelaCobranca.DataDeVencimento
        };
    }
}

public class PixViewModel
{
    public string QrCode { get; set; } = string.Empty;
    public string CopiaECola { get; set; } = string.Empty;
}

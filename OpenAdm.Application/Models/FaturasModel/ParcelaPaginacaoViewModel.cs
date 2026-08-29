using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.FaturasModel;

public sealed class ParcelaPaginacaoViewModel
{
    public Guid Id { get; set; }
    public long NumeroFatura { get; set; }
    public int NumeroDaParcela { get; set; }
    public long? NumeroPedido { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public StatusParcelaEnum Status { get; set; }
    public decimal ValorPagoRecebido { get; set; }
    public decimal ValorPagoRecebidoLiquido { get; set; }
    public decimal DescontoConcedido { get; set; }
    public decimal ValorAPagarAReceber { get; set; }
    public DateTime Vencimento { get; set; }
    public bool Quitada { get; set; }

    public static explicit operator ParcelaPaginacaoViewModel(Parcela parcela)
    {
        return new ParcelaPaginacaoViewModel
        {
            Id = parcela.Id,
            NumeroFatura = parcela.Fatura.Numero,
            NumeroDaParcela = parcela.NumeroDaParcela,
            NomeUsuario = parcela.Fatura.Usuario.Nome,
            Valor = parcela.Valor,
            ValorPagoRecebido = parcela.ValorPagoRecebido,
            ValorPagoRecebidoLiquido = parcela.ValorPagoRecebidoLiquido,
            DescontoConcedido = parcela.DescontoConcedido,
            ValorAPagarAReceber = parcela.ValorAPagarAReceber,
            Vencimento = parcela.DataDeVencimento,
            Quitada = parcela.Quitada,
            NumeroPedido = parcela.Fatura.Pedido?.Numero,
            Status = parcela.Status,
        };
    }
}

using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.FaturasModel;

public sealed class FaturaBonificadaPaginacaoViewModel
{
    public Guid Id { get; set; }
    public long NumeroFatura { get; set; }
    public long? NumeroPedido { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public StatusFaturaEnum Status { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public DateTime? DataDeFechamento { get; set; }
    public bool Quitada { get; set; }

    public static explicit operator FaturaBonificadaPaginacaoViewModel(Fatura fatura)
    {
        return new FaturaBonificadaPaginacaoViewModel
        {
            Id = fatura.Id,
            NumeroFatura = fatura.Numero,
            NumeroPedido = fatura.Pedido?.Numero,
            NomeUsuario = fatura.Usuario.Nome,
            Total = fatura.Total,
            Status = fatura.Status,
            DataDeCriacao = fatura.DataDeCriacao,
            DataDeFechamento = fatura.DataDeFechamento,
            Quitada = fatura.Quitada
        };
    }
}

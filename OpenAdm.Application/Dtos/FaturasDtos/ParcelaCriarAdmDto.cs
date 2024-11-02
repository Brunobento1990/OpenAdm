using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class ParcelaCriarAdmDto
{
    public DateTime DataDeVencimento { get; set; }
    public int NumeroDaFatura { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
}

public class ParcelaCriarDto
{
    public Guid FaturaId { get; set; }
    public DateTime? DataDeVencimento { get; set; }
    public int NumeroDaFatura { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }

    public void Validar()
    {
        if (Valor == 0)
        {
            throw new ExceptionApi("Informe o valor da fatura!");
        }

        if (Desconto.HasValue && Desconto.Value > Valor)
        {
            throw new ExceptionApi("O desconto da fatura não pode ser maior que o desconto!");
        }
    }
}

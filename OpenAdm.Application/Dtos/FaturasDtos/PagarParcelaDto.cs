using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class PagarParcelaDto
{
    public Guid Id { get; set; }
    public decimal? Desconto { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataDePagamento { get; set; }

    public void Validar()
    {
        if (Valor <= 0)
        {
            throw new ExceptionApi("Valor para pagamento inválido!");
        }
    }
}

using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public sealed class RenegociarFaturaDto
{
    public Guid FaturaId { get; set; }
    public IList<ParcelaRenegociacaoDto> Parcelas { get; set; } = [];

    public string? Validar()
    {
        if (FaturaId == Guid.Empty)
            return "Informe a fatura!";

        if (Parcelas == null || Parcelas.Count == 0)
            return "Informe ao menos uma parcela!";

        if (Parcelas.Any(x => x is null || x.NumeroDaParcela <= 0 || x.Valor <= 0 || x.DataDeVencimento == default))
            return "Os dados das parcelas são inválidos!";

        if (Parcelas.Select(x => x.NumeroDaParcela).Distinct().Count() != Parcelas.Count)
            return "Os números das parcelas não podem ser repetidos!";

        return null;
    }

    public string? ValidarTotal(decimal total, IEnumerable<ParcelaRenegociacaoDto>? parcelasParaCriar = null)
    {
        if ((parcelasParaCriar ?? Parcelas).Sum(x => x.Valor.ArredondarCentavos()) != total.ArredondarCentavos())
            return "A soma das parcelas deve ser igual ao total da fatura!";

        return null;
    }
}

public sealed class ParcelaRenegociacaoDto
{
    public DateTime DataDeVencimento { get; set; }
    public int NumeroDaParcela { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
}

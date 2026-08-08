using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public sealed class NegociarCobrancaPedidoDto
{
    public Guid PedidoId { get; set; }
    public IList<ParcelaNegociacaoDto> Parcelas { get; set; } = [];

    public string? Validar(decimal totalCobranca)
    {
        if (Parcelas.Count == 0)
        {
            return "Informe ao menos uma parcela!";
        }

        if (Parcelas.Any(x => x.NumeroDaParcela <= 0 || x.Valor <= 0))
        {
            return "Os dados das parcelas são inválidos!";
        }

        if (Parcelas.Select(x => x.NumeroDaParcela).Distinct().Count() != Parcelas.Count)
        {
            return "Os números das parcelas não podem ser repetidos!";
        }

        var totalParcelas = Parcelas.Sum(x =>
            decimal.Round(x.Valor, 2, MidpointRounding.AwayFromZero));
        var totalCobrancaArredondado = decimal.Round(
            totalCobranca,
            2,
            MidpointRounding.AwayFromZero);

        if (totalParcelas != totalCobrancaArredondado)
        {
            return "A soma das parcelas deve ser igual ao total da cobrança!";
        }

        return null;
    }
}

public sealed class ParcelaNegociacaoDto
{
    public DateTime DataDeVencimento { get; set; }
    public int NumeroDaParcela { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public bool AVista { get; set; }
}

namespace OpenAdm.Application.Dtos;

public class RelatorioVendaDeProdutoDTO
{
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public int Skip { get; set; }
    public bool Asc { get; set; }
    public TipoRelatorioVendaDeProdutoEnum? Tipo { get; set; }

    public DateTime? ObterDataInicial()
    {
        if (!Tipo.HasValue)
        {
            return DataInicial;
        }

        switch (Tipo.Value)
        {
            case TipoRelatorioVendaDeProdutoEnum.UltimosSeteDias:
                return DateTime.Today.AddDays(-6);
            case TipoRelatorioVendaDeProdutoEnum.UltimosTrintaDias:
                return DateTime.Today.AddDays(-29);
            case TipoRelatorioVendaDeProdutoEnum.UltimosNoventaDias:
                return DateTime.Today.AddDays(-89);
            default:
                return DataInicial;
        }
    }
}

public enum TipoRelatorioVendaDeProdutoEnum
{
    UltimosSeteDias = 1,
    UltimosTrintaDias = 2,
    UltimosNoventaDias = 3,
}
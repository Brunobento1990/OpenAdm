namespace OpenAdm.Application.Models.MovimentacaoDeProdutos;

public class MovimentacaoDeProdutoRelatorio
{
    public string? Referencia { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string PesoTamanho { get; set; } = string.Empty;
    public string TipoMovimento { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public DateTime DataDaMovimentacao { get; set; }
}

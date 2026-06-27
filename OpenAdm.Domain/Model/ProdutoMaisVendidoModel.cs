namespace OpenAdm.Domain.Model;

public class ProdutoMaisVendidoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? Foto { get; set; }
    public string? Peso { get; set; }
    public string? Tamanho { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal => ValorUnitario * Quantidade;
}
namespace OpenAdm.Application.Models.Pedidos;

public class ItemPedidoProducaoViewModel
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public string? Referencia { get; set; }
    public string Produto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Peso { get; set; } = string.Empty;
    public string Tamanho { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
}

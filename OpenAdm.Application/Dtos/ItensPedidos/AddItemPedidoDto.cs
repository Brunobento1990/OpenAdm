namespace OpenAdm.Application.Dtos.ItensPedidos;

public sealed class AddItemPedidoDto
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

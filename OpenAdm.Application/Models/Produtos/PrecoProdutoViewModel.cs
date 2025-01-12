namespace OpenAdm.Application.Models.Produtos;

public class PrecoProdutoViewModel
{
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public decimal ValorUnitario { get; set; }
}

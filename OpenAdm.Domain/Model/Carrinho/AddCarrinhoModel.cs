namespace OpenAdm.Domain.Model.Carrinho;

public class AddCarrinhoModel
{
    public Guid ProdutoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid? PesoId { get; set; }
    public decimal Quantidade { get; set; }
}

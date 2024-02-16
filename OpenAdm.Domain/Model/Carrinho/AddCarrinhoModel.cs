namespace OpenAdm.Domain.Model.Carrinho;

public class AddCarrinhoModel
{
    public Guid ProdutoId { get; set; }
    public List<AddTamanhoCarrinho> Tamanhos { get; set; } = new();
    public List<AddPesoCarrinho> Pesos { get; set; } = new();
}

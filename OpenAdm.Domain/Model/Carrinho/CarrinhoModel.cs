namespace OpenAdm.Domain.Model.Carrinho;

public class CarrinhoModel
{
    public Guid UsuarioId { get; set; }
    public List<AddCarrinhoModel> Produtos { get; set; } = new();
}

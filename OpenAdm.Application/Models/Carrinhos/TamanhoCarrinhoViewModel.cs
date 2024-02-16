namespace OpenAdm.Application.Models.Carrinhos;

public class TamanhoCarrinhoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public QuantidadeProdutoCarrinhoViewModel PrecoProduto { get; set; } = new();
}

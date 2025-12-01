namespace OpenAdm.Application.Models.Carrinhos;

public class PesoCarrinhoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public decimal? Quantidade { get; set; }
    public bool TemEstoqueDisponivel { get; set; } = true;
    public QuantidadeProdutoCarrinhoViewModel PrecoProduto { get; set; } = new();
}

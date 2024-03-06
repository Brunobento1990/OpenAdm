namespace OpenAdm.Application.Models.Carrinhos;

public class QuantidadeProdutoCarrinhoViewModel
{
    public Guid Id { get; set; }
    public decimal Quantidade { get; set; }
    public decimal? ValorUnitarioAtacado { get; set; }
    public decimal? ValorUnitarioVarejo { get; set; }
}

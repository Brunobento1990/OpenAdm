namespace OpenAdm.Application.Queries;

public class CategoriaEcommerceHomeQuery : CategoriaEcommerceQuery
{
    public IList<ProdutoCategoriaEcommerceHomeQuery> Produtos { get; set; } = [];
}

public class ProdutoCategoriaEcommerceHomeQuery
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Foto { get; set; } = string.Empty;
}
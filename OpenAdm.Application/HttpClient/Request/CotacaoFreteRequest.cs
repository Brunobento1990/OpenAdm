namespace OpenAdm.Application.HttpClient.Request;

public class CotacaoFreteRequest
{
    public CepCotacaoFreteRequest From { get; set; } = new();
    public CepCotacaoFreteRequest To { get; set; } = new();
    public ICollection<ProdutoCotacaoFreteRequest> Products { get; set; } = [];
}

public class CepCotacaoFreteRequest
{
    public string Postal_code { get; set; } = string.Empty;
}

public class ProdutoCotacaoFreteRequest
{
    public string Id { get; set; } = string.Empty;
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Length { get; set; }
    public decimal Weight { get; set; }
    public decimal Insurance_value { get; set; }
    public int Quantity { get; set; }
}
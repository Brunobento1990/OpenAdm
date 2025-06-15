namespace OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;

public class EnderecoEntregaPedidoCreateDto
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public decimal? ValorFrete { get; set; }
    public string? TipoFrete { get; set; }
    public Guid PedidoId { get; set; }
}

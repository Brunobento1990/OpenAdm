namespace OpenAdm.Infra.Model;

public class ConsultaCepResponse
{
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string? Complemento { get; set; }
}

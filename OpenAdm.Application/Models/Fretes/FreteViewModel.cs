namespace OpenAdm.Application.Models.Fretes;

public class FreteViewModel
{
    public decimal ValorPac { get; set; }
    public decimal ValorSedex { get; set; }
    public EnderecoViewModel Endereco { get; set; } = new();
}

public class EnderecoViewModel
{
    public string Numero { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}

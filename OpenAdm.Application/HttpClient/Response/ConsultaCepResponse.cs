﻿namespace OpenAdm.Application.HttpClient.Response;

public class ConsultaCepResponse
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string? Complemento { get; set; }
}

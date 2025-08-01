﻿namespace OpenAdm.Application.Models.Emails;

public class ToEnvioEmailApiModel
{
    public string Email { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string? Html { get; set; }
    public string Assunto { get; set; } = string.Empty;
    public string? NomeDoArquivo { get; set; }
    public string? TipoDoArquivo { get; set; }
    public byte[]? Arquivo { get; set; }
}

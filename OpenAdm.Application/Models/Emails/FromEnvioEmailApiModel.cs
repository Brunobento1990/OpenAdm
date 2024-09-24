namespace OpenAdm.Application.Models.Emails;

public class FromEnvioEmailApiModel
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Servidor { get; set; } = string.Empty;
    public int Porta { get; set; }
    public bool EnableSsl { get; set; }
}

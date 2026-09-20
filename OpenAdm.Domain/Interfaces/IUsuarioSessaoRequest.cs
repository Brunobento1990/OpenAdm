namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioSessaoRequest
{
    string? EnderecoIp { get; set; }
    string? UserAgent { get; set; }
    string? SistemaOperacional { get; set; }
    string? Navegador { get; set; }
    string? Dispositivo { get; set; }
}
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Models;

public class UsuarioSessaoRequest : IUsuarioSessaoRequest
{
    public string? EnderecoIp { get ; set ; }
    public string? UserAgent { get ; set ; }
    public string? SistemaOperacional { get ; set ; }
    public string? Navegador { get ; set ; }
    public string? Dispositivo { get ; set ; }
}
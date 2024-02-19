using System.ComponentModel.DataAnnotations;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Dtos.Usuarios;

public class UpdateSenhaUsuarioDto
{
    [Required]
    [MaxLength(1000)]
    public string Senha { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    [Compare("Senha", ErrorMessage = "As senha não conferem!")]
    public string ReSenha { get; set; } = string.Empty;

    public string HashSenha()
    {
        return HashPassword(Senha, 10);
    }
}

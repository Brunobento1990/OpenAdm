using OpenAdm.Application.Adapters;
using System.ComponentModel.DataAnnotations;

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
        return PasswordAdapter.GenerateHash(Senha);
    }
}

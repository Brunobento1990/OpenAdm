using Domain.Pkg.Entities;
using OpenAdm.Application.Models;
using System.ComponentModel.DataAnnotations;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Dtos.Usuarios;

public class CreateUsuarioDto : BaseModel
{
    [Required]
    [MaxLength(255)]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Senha { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    [Compare("Senha", ErrorMessage = "As senhas não conferem!")]
    public string ReSenha { get; set; } = string.Empty;
    [MaxLength(15)]
    public string? Telefone { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Cnpj { get; set; } = string.Empty;

    public Usuario ToEntity()
    {
        var senha = HashPassword(Senha, 10);

        var date = DateTime.Now;
        return new Usuario(
            Guid.NewGuid(),
            date,
            date,
            0,
            Email,
            senha,
            Nome,
            Telefone,
            Cnpj);
    }
}

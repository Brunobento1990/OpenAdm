using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Usuarios;

public class UpdateUsuarioDto
{
    [Required]
    [MaxLength(255)]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(15)]
    public string? Telefone { get; set; }
    [MaxLength(20)]
    public string? Cnpj { get; set; }
}

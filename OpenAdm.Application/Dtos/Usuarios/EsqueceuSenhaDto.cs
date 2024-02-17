using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Usuarios;

public class EsqueceuSenhaDto
{
    [Required]
    [MaxLength(255)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}

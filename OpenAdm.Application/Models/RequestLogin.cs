using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Models;

public class RequestLogin
{
    [Required]
    [MaxLength(255)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}

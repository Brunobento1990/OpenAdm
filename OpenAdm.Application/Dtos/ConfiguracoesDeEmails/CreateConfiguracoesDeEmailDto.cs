using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.ConfiguracoesDeEmails;

public class CreateConfiguracoesDeEmailDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Servidor { get; set; } = string.Empty;
    [Required]
    public int Porta { get; set; }
    public bool? Ativo { get; set; } = true;

    public ConfiguracaoDeEmail ToEntity()
    {
        var date = DateTime.Now;

        return new ConfiguracaoDeEmail(
                Guid.NewGuid(),
                date,
                date,
                0,
                Email,
                Servidor,
                Criptografia.Encrypt(Senha),
                Porta,
                Ativo.HasValue);
    }
}

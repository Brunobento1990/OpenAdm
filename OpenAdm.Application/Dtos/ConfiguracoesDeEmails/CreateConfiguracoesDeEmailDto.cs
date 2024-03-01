using Domain.Pkg.Cryptography;
using Domain.Pkg.Entities;
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
    public bool? Ativo { get; set; }

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
                CryptographyGeneric.Encrypt(Senha),
                Porta,
                Ativo.HasValue);
    }
}

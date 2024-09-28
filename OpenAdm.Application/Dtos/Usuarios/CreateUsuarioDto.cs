using OpenAdm.Domain.Entities;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Models;
using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Exceptions;

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
    public string ReSenha { get; set; } = string.Empty;
    [MaxLength(15)]
    [Required(ErrorMessage = "Informe o seu telefone.")]
    public string Telefone { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Cnpj { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Cpf { get; set; } = string.Empty;

    public Usuario ToEntity()
    {
        if((string.IsNullOrWhiteSpace(Cnpj) && string.IsNullOrWhiteSpace(Cpf))
            || (!string.IsNullOrWhiteSpace(Cnpj) && !string.IsNullOrWhiteSpace(Cpf)))
        {
            throw new ExceptionApi("Informe o CNPJ ou o CPF");
        }

        var senha = PasswordAdapter.GenerateHash(Senha);

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
            Cnpj,
            Cpf,
            true);
    }
}

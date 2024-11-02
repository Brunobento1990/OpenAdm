using OpenAdm.Domain.Entities;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Models;
using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Application.Dtos.Usuarios;

public class CreateUsuarioDto : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ReSenha { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Cnpj { get; set; } = string.Empty;
    public string? Cpf { get; set; } = string.Empty;

    public void Validar()
    {
        if ((string.IsNullOrWhiteSpace(Cpf) && string.IsNullOrWhiteSpace(Cnpj)) ||
            !string.IsNullOrWhiteSpace(Cpf) && !string.IsNullOrWhiteSpace(Cnpj))
        {
            throw new ExceptionApi("Informe o CPF ou o CNPJ");
        }

        if (string.IsNullOrWhiteSpace(Nome))
        {
            throw new ExceptionApi("Informe o nome");
        }

        if (string.IsNullOrWhiteSpace(Telefone))
        {
            throw new ExceptionApi("Informe o telefone");
        }

        if (string.IsNullOrWhiteSpace(Senha) || string.IsNullOrWhiteSpace(ReSenha))
        {
            throw new ExceptionApi("Senhas inválidas");
        }

        if (!Senha.Equals(ReSenha))
        {
            throw new ExceptionApi("As senha não conferem!");
        }

        if (!string.IsNullOrWhiteSpace(Cpf) && !ValidarCnpjECpf.IsCpf(Cpf))
        {
            throw new ExceptionApi("CPF inválido!");
        }
    }

    public Usuario ToEntity()
    {
        if ((string.IsNullOrWhiteSpace(Cnpj) && string.IsNullOrWhiteSpace(Cpf))
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
            string.IsNullOrWhiteSpace(Cnpj) ? null : Cnpj,
            string.IsNullOrWhiteSpace(Cpf) ? null : Cpf,
            true);
    }
}

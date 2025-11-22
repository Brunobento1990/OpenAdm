using OpenAdm.Domain.Entities;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Models;
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
    public TipoPessoa TipoPessoa { get; set; }

    public void Validar()
    {
        if (TipoPessoa == TipoPessoa.Juridica && string.IsNullOrWhiteSpace(Cnpj))
        {
            throw new ExceptionApi("Informe o CNPJ");
        }

        if (TipoPessoa == TipoPessoa.Fisica && string.IsNullOrWhiteSpace(Cpf))
        {
            throw new ExceptionApi("Informe o CPF");
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

        if (!string.IsNullOrWhiteSpace(Cpf) && !ValidarCnpjECpf.IsCpf(Cpf) && TipoPessoa == TipoPessoa.Fisica)
        {
            throw new ExceptionApi("CPF inválido!");
        }

        Cpf = Cpf?.Replace(".", "")?.Replace("-", "");
        Cnpj = Cnpj?.Replace(".", "")?.Replace("-", "")?.Replace("/", "");
    }

    public Usuario ToEntity(bool ativo)
    {
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
            TipoPessoa == TipoPessoa.Juridica ? Cnpj : null,
            TipoPessoa == TipoPessoa.Fisica ? Cpf : null,
            ativo,
            null,
            null);
    }
}

public enum TipoPessoa
{
    Juridica = 1,
    Fisica = 2
}

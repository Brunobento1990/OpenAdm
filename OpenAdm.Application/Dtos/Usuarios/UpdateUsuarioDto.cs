using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Application.Dtos.Usuarios;

public class UpdateUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }

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

        if (!string.IsNullOrWhiteSpace(Cpf) && !ValidarCnpjECpf.IsCpf(Cpf))
        {
            throw new ExceptionApi("CPF inválido!");
        }
    }
}

﻿using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Models.Logins;

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

public class RequestLoginUsuario
{
    public string CpfCnpj { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(CpfCnpj))
        {
            throw new ExceptionApi("CPF/CNPJ inválido");
        }

        if (string.IsNullOrWhiteSpace(Senha))
        {
            throw new ExceptionApi("Senha inválida");
        }
    }
}

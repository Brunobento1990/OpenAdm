﻿using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.Usuarios;

public class UsuarioViewModel : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public int? QuantidadeDePedido { get; set; }

    public UsuarioViewModel ToModel(Usuario entity, int? quantidadeDePedido = null)
    {
        QuantidadeDePedido = quantidadeDePedido;
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Email = entity.Email;
        Numero = entity.Numero;
        Telefone = entity.Telefone;
        Nome = entity.Nome;
        Cnpj = entity.Cnpj;
        Cpf = entity.Cpf;

        return this;
    }
}

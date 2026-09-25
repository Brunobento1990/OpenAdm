using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Representante : BaseEntity
{
    public Representante(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero,
        string nome, string? cpf, string? email, string? telefone, string? senha, bool ativo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        Telefone = telefone;
        Senha = senha;
        Ativo = ativo;
    }

    public string Nome { get; private set; }
    public string? Cpf { get; private set; }
    public string? Email { get; private set; }
    public string? Telefone { get; private set; }
    public string? Senha { get; private set; }
    public bool Ativo { get; private set; }

    public void Editar(string nome, string? cpf, string? email, string? telefone)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        Telefone = telefone;
        DataDeAtualizacao = DateTime.UtcNow;
    }

    public void InativarAtivar(bool ativo)
    {
        Ativo = ativo;
        DataDeAtualizacao = DateTime.UtcNow;
    }
}

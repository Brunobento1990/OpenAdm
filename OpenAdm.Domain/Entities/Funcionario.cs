using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Funcionario : BaseEntity
{
    public Funcionario(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, string email, string senha, string nome, string? telefone, byte[]? avatar, bool ativo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Email = email;
        Senha = senha;
        Nome = nome;
        Telefone = telefone;
        Avatar = avatar;
        Ativo = ativo;
    }

    public string Email { get; private set; }
    public string Senha { get; private set; }
    public string Nome { get; private set; }
    public string? Telefone { get; private set; }
    public byte[]? Avatar { get; private set; }
    public bool Ativo { get; private set; }
}

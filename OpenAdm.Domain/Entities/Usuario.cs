using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Usuario : BaseEntity
{
    public Usuario(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string email,
        string senha,
        string nome,
        string? telefone,
        string? cnpj,
        string? cpf,
        bool ativo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Email = email;
        Senha = senha;
        Nome = nome;
        Telefone = telefone;
        Cnpj = cnpj;
        Cpf = cpf;
        Ativo = ativo;
    }

    public string Email { get; private set; }
    public string Senha { get; private set; }
    public string Nome { get; private set; }
    public string? Telefone { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Cpf { get; private set; }
    public bool Ativo { get; private set; }
    public bool IsAtacado => !string.IsNullOrWhiteSpace(Cnpj);
    public EnderecoUsuario? EnderecoUsuario { get; set; }

    public void Inativar()
    {
        Ativo = false;
    }

    public void UpdateSenha(string senha)
    {
        Senha = senha;
    }

    public void Update(string email, string nome, string? telefone, string? cnpj, string? cpf)
    {
        Cpf = cpf;
        Cnpj = cnpj;
        Email = email;
        Nome = nome;
        Telefone = telefone;
    }
}

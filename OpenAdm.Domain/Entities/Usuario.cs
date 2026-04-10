using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Extensions;

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
        bool ativo,
        Guid? tokenEsqueceuSenha,
        DateTime? dataExpiracaoTokenEsqueceuSenha, DateTime? forcarLogin)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Email = email;
        Senha = senha;
        Nome = nome;
        Telefone = telefone;
        Cnpj = cnpj;
        Cpf = cpf;
        Ativo = ativo;
        TokenEsqueceuSenha = tokenEsqueceuSenha;
        DataExpiracaoTokenEsqueceuSenha = dataExpiracaoTokenEsqueceuSenha;
        ForcarLogin = forcarLogin;
    }

    public string Email { get; private set; }
    public string Senha { get; private set; }
    public string Nome { get; private set; }
    public string? Telefone { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Cpf { get; private set; }

    public string? CpfOuCnpjFormatado => !string.IsNullOrWhiteSpace(Cnpj) ? Cnpj.FormatCnpj() :
        !string.IsNullOrWhiteSpace(Cpf) ? Cpf.FormatCpf() : null;

    public bool Ativo { get; private set; }
    public bool AcessoLiberadoEcommerce => Ativo && !string.IsNullOrWhiteSpace(Telefone);
    public Guid? TokenEsqueceuSenha { get; private set; }
    public DateTime? DataExpiracaoTokenEsqueceuSenha { get; private set; }
    public DateTime? ForcarLogin { get; private set; }
    public bool IsAtacado => !string.IsNullOrWhiteSpace(Cnpj);
    public EnderecoUsuario? EnderecoUsuario { get; set; }
    public IList<Pedido>? Pedidos { get; set; }

    public void EsqueceuSenha()
    {
        TokenEsqueceuSenha = Guid.NewGuid();
        DataExpiracaoTokenEsqueceuSenha = DateTime.Now;
    }

    public void UpdateSenha(string senha)
    {
        Senha = senha;
        TokenEsqueceuSenha = null;
        DataExpiracaoTokenEsqueceuSenha = null;
    }

    public void Update(string email, string nome, string? telefone, string? cnpj, string? cpf)
    {
        Cpf = string.IsNullOrWhiteSpace(cpf) ? null : cpf;
        Cnpj = string.IsNullOrWhiteSpace(cnpj) ? null : cnpj;
        Email = email;
        Nome = nome;
        Telefone = telefone;
    }

    public void AtivarBloquear()
    {
        Ativo = !Ativo;
    }
}
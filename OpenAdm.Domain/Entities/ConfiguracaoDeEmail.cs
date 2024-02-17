
namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoDeEmail : BaseEntity
{
    public ConfiguracaoDeEmail(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string email,
        string servidor,
        string senha,
        int porta,
        bool ativo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Email = email;
        Servidor = servidor;
        Senha = senha;
        Porta = porta;
        Ativo = ativo;
    }

    public string Email { get; private set; }
    public string Servidor { get; private set; }
    public string Senha { get; private set; }
    public int Porta { get; private set; }
    public bool Ativo { get; private set; }
}

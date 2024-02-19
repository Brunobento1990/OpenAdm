
namespace OpenAdm.Domain.Entities;

public class ConfiguracoesDePedido : BaseEntity
{
    public ConfiguracoesDePedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string emailDeEnvio,
        bool ativo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
    }

    public string EmailDeEnvio { get; private set; }
    public bool Ativo { get; private set; }
}

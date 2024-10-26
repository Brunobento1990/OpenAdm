using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoDePagamento : BaseEntity
{
    public ConfiguracaoDePagamento(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string publicKey,
        string accessToken,
        bool cobrarCpf,
        bool cobrarCnpj)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        PublicKey = publicKey;
        AccessToken = accessToken;
        CobrarCpf = cobrarCpf;
        CobrarCnpj = cobrarCnpj;
    }

    public void Update(string publicKey, string accessToken, bool cobrarCpf, bool cobrarCnpj)
    {
        CobrarCnpj = cobrarCnpj;
        CobrarCpf = cobrarCpf;
        AccessToken = accessToken;
        PublicKey = publicKey;
    }

    public string PublicKey { get; private set; }
    public string AccessToken { get; private set; }
    public bool CobrarCpf { get; private set; }
    public bool CobrarCnpj { get; private set; }
}

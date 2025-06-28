using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoDePagamento : BaseEntityParceiro
{
    public ConfiguracaoDePagamento(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string publicKey,
        string accessToken,
        bool cobrarCpf,
        bool cobrarCnpj,
        string? urlWebHook,
        Guid parceiroId)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        PublicKey = publicKey;
        AccessToken = accessToken;
        CobrarCpf = cobrarCpf;
        CobrarCnpj = cobrarCnpj;
        UrlWebHook = urlWebHook;
    }

    public void Update(string publicKey, string accessToken, bool cobrarCpf, bool cobrarCnpj, string? urlWebHook)
    {
        CobrarCnpj = cobrarCnpj;
        CobrarCpf = cobrarCpf;
        AccessToken = accessToken;
        PublicKey = publicKey;
        UrlWebHook = urlWebHook;
    }

    public string PublicKey { get; private set; }
    public string AccessToken { get; private set; }
    public string? UrlWebHook { get; private set; }
    public bool CobrarCpf { get; private set; }
    public bool CobrarCnpj { get; private set; }
}

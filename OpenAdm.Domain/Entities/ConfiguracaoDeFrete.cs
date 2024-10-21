namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoDeFrete
{
    public ConfiguracaoDeFrete(
        Guid id,
        string cepOrigem,
        string alturaEmbalagem,
        string larguraEmbalagem,
        string comprimentoEmbalagem,
        string chaveApi,
        int? peso,
        bool? cobrarCpf,
        bool? cobrarCnpj,
        bool? inativo)
    {
        Id = id;
        CepOrigem = cepOrigem;
        AlturaEmbalagem = alturaEmbalagem;
        LarguraEmbalagem = larguraEmbalagem;
        ComprimentoEmbalagem = comprimentoEmbalagem;
        ChaveApi = chaveApi;
        Peso = peso;
        CobrarCpf = cobrarCpf;
        CobrarCnpj = cobrarCnpj;
        Inativo = inativo;
    }
    public void Update(
        string cepOrigem,
        string alturaEmbalagem,
        string larguraEmbalagem,
        string comprimentoEmbalagem,
        string chaveApi,
        int? peso,
        bool? cobrarCpf,
        bool? cobrarCnpj,
        bool? inativo)
    {
        CepOrigem = cepOrigem;
        AlturaEmbalagem = alturaEmbalagem;
        LarguraEmbalagem = larguraEmbalagem;
        ComprimentoEmbalagem = comprimentoEmbalagem;
        ChaveApi = chaveApi;
        Peso = peso;
        CobrarCpf = cobrarCpf;
        CobrarCnpj = cobrarCnpj;
        Inativo = inativo;
    }
    public Guid Id { get; private set; }
    public string CepOrigem { get; private set; }
    public string AlturaEmbalagem { get; private set; }
    public string LarguraEmbalagem { get; private set; }
    public string ComprimentoEmbalagem { get; private set; }
    public string ChaveApi { get; private set; }
    public int? Peso { get; private set; }
    public bool? CobrarCpf { get; private set; }
    public bool? CobrarCnpj { get; private set; }
    public bool? Inativo { get; private set; }
}

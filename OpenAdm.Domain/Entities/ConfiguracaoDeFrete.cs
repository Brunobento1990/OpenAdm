using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public class ConfiguracaoDeFrete : BaseEntityParceiro
{
    public ConfiguracaoDeFrete(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid parceiroId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
    }

    public string Token { get; set; } = string.Empty;
    public string? CepOrigem { get; set; }
    public bool Ativo { get; set; }
    public bool CobrarCnpj { get; set; }
    public bool CobrarCpf { get; set; }
}
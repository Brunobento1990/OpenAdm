namespace OpenAdm.Domain.Entities;

public sealed class FaturaHistorico
{
    public FaturaHistorico(Guid id, Guid faturaId, DateTime dataDeCriacao, string descricao)
    {
        Id = id;
        FaturaId = faturaId;
        DataDeCriacao = dataDeCriacao;
        Descricao = descricao;
    }

    public Guid Id { get; private set; }
    public Guid FaturaId { get; private set; }
    public DateTime DataDeCriacao { get; private set; }
    public string Descricao { get; private set; }
    public Fatura Fatura { get; set; } = null!;

    public static FaturaHistorico Nova(Guid faturaId, string descricao)
        => new(Guid.NewGuid(), faturaId, DateTime.UtcNow, descricao);
}

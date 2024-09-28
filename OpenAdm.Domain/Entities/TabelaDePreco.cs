using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class TabelaDePreco : BaseEntity
{
    public TabelaDePreco(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao,
        bool ativaEcommerce)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        AtivaEcommerce = ativaEcommerce;
    }

    public string Descricao { get; private set; }
    public bool AtivaEcommerce { get; private set; }
    public List<ItemTabelaDePreco> ItensTabelaDePreco { get; set; } = [];

    public void Update(string descricao, bool ativaEcommerce)
    {
        Descricao = descricao;
        AtivaEcommerce = ativaEcommerce;
    }
}


using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Validations;

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
        ValidationString.ValidateWithLength(descricao, message: CodigoErrors.DescricaoTabelaDePrecoInvalida);
        Descricao = descricao;
        AtivaEcommerce = ativaEcommerce;
    }

    public string Descricao { get; private set; }
    public bool AtivaEcommerce { get; private set; }
    public List<ItensTabelaDePreco> ItensTabelaDePreco { get; set; } = new();
}

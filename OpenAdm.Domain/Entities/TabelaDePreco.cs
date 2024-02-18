
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

    public decimal GetValorUnitarioByTamanhoId(Guid produtoId, Guid? tamanhoId)
    {
        var itemTabelaDePreco = ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId && x.TamanhoId == tamanhoId);

        if(itemTabelaDePreco == null)
        {
            itemTabelaDePreco = ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId);
        }

        return itemTabelaDePreco?.ValorUnitario ?? 0;
    }

    public decimal GetValorUnitarioByPesoId(Guid produtoId, Guid? pesoId)
    {
        var itemTabelaDePreco = ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId && x.PesoId == pesoId);

        if (itemTabelaDePreco == null)
        {
            itemTabelaDePreco = ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == produtoId);
        }

        return itemTabelaDePreco?.ValorUnitario ?? 0;
    }
}

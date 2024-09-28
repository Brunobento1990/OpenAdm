using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ItemTabelaDePreco : BaseItem
{
    public ItemTabelaDePreco(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid produtoId, decimal valorUnitarioAtacado, decimal valorUnitarioVarejo, Guid tabelaDePrecoId, Guid? tamanhoId, Guid? pesoId) : base(id, dataDeCriacao, dataDeAtualizacao, numero, produtoId)
    {
        ValorUnitarioAtacado = valorUnitarioAtacado;
        ValorUnitarioVarejo = valorUnitarioVarejo;
        TabelaDePrecoId = tabelaDePrecoId;
        TamanhoId = tamanhoId;
        PesoId = pesoId;
    }

    public void Update(decimal valorUnitarioAtacado, decimal valorUnitarioVarejo, Guid? tamanhoId, Guid? pesoId, Guid produtoId)
    {
        ValorUnitarioVarejo = valorUnitarioVarejo;
        ValorUnitarioAtacado = valorUnitarioAtacado;
        TamanhoId = tamanhoId;
        PesoId = pesoId;
        ProdutoId = produtoId;
    }

    public decimal ValorUnitarioAtacado { get; private set; }
    public decimal ValorUnitarioVarejo { get; private set; }
    public Guid TabelaDePrecoId { get; private set; }
    public TabelaDePreco TabelaDePreco { get; set; } = null!;
    public Guid? TamanhoId { get; private set; }
    public Guid? PesoId { get; private set; }
}

using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Dtos.Produtos;

public class UpdateProdutoDto : CreateProdutoDto
{
    public Guid Id { get; set; }

    public IList<TamanhoProduto> ToTamanhosProdutos()
    {
        var tamanhosProdutos = new List<TamanhoProduto>();

        if (TamanhosIds == null || TamanhosIds.Count == 0)
            return tamanhosProdutos;

        foreach (var tamanhoId in TamanhosIds)
        {
            tamanhosProdutos.Add(new TamanhoProduto(Guid.NewGuid(), Id, tamanhoId));
        }

        return tamanhosProdutos;
    }

    public IList<PesoProduto> ToPesosProdutos()
    {
        var pesosProdutos = new List<PesoProduto>();

        if (PesosIds?.Count == 0 || PesosIds == null)
            return pesosProdutos;


        foreach (var pesoId in PesosIds)
        {
            pesosProdutos.Add(new PesoProduto(Guid.NewGuid(), Id, pesoId));
        }

        return pesosProdutos;
    }

    public IList<ItemTabelaDePreco>? ObterItensTabelaDePrecoEdit()
    {
        return !TabelaDePrecoId.HasValue ? null : ItensTabelaDePreco
        .Select(x => new ItemTabelaDePreco(Guid.NewGuid(), DateTime.Now, DateTime.Now, 0, Id, x.ValorUnitarioAtacado, x.ValorUnitarioVarejo, TabelaDePrecoId.Value, x.TamanhoId, x.PesoId)).ToList();
    }
}

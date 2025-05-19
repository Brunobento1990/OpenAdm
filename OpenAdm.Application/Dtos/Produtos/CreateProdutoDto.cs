using System.Text.Json.Serialization;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.Produtos;

public class CreateProdutoDto
{
    public string Descricao { get; set; } = string.Empty;
    public string NovaFoto { get; set; } = string.Empty;
    public string? EspecificacaoTecnica { get; set; }
    public string? Referencia { get; set; }
    public Guid CategoriaId { get; set; }
    public Guid? TabelaDePrecoId { get; set; }
    public IList<Guid>? TamanhosIds { get; set; }
    public IList<Guid>? PesosIds { get; set; }

    public IList<CreateItemProdutoTabelaDePrecoDto> ItensTabelaDePreco { get; set; } = [];
    public IList<ItemTabelaDePreco>? ObterItensTabelaDePreco()
    {
        return!TabelaDePrecoId.HasValue ? null : ItensTabelaDePreco
        .Select(x => new ItemTabelaDePreco(Guid.NewGuid(), DateTime.Now, DateTime.Now, 0, Guid.Empty, x.ValorUnitarioAtacado, x.ValorUnitarioVarejo, TabelaDePrecoId.Value, x.TamanhoId, x.PesoId)).ToList();
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
        {
            throw new ExceptionApi("Informe a descrição");
        }
    }
    public Produto ToEntity(string nomeFoto)
    {
        var date = DateTime.Now;
        var produto = new Produto(
            Guid.NewGuid(),
            date,
            date,
            0,
            Descricao, EspecificacaoTecnica,
            CategoriaId,
            Referencia,
            NovaFoto,
            nomeFoto,
            false);

        produto.ItensTabelaDePreco = ObterItensTabelaDePreco() ?? [];

        return produto;
    }

    public IList<TamanhoProduto> ToTamanhosProdutos(Guid produtoId)
    {
        var tamanhosProdutos = new List<TamanhoProduto>();

        if (TamanhosIds == null || TamanhosIds.Count == 0)
            return tamanhosProdutos;

        foreach (var tamanhoId in TamanhosIds)
        {
            tamanhosProdutos.Add(new TamanhoProduto(Guid.NewGuid(), produtoId, tamanhoId));
        }

        return tamanhosProdutos;
    }

    public IList<PesoProduto> ToPesosProdutos(Guid produtoId)
    {
        var pesosProdutos = new List<PesoProduto>();

        if (PesosIds?.Count == 0 || PesosIds == null)
            return pesosProdutos;


        foreach (var pesoId in PesosIds)
        {
            pesosProdutos.Add(new PesoProduto(Guid.NewGuid(), produtoId, pesoId));
        }

        return pesosProdutos;
    }
}

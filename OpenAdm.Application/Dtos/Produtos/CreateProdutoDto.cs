using Domain.Pkg.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Produtos;

public class CreateProdutoDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
    [Required]
    public string Foto { get; set; } = string.Empty;
    public string? EspecificacaoTecnica { get; set; }
    public string? Referencia { get; set; }
    public decimal? Peso { get; set; }
    public Guid CategoriaId { get; set; }
    public IList<Guid>? TamanhosIds { get; set; }
    public IList<Guid>? PesosIds { get; set; }

    public Produto ToEntity(string nomeFoto)
    {
        var date = DateTime.Now;

        return new Produto(
            Guid.NewGuid(),
            date,
            date,
            0,
            Descricao, EspecificacaoTecnica,
            CategoriaId,
            Referencia,
            Foto,
            nomeFoto,
            Peso);
    }

    public IList<TamanhosProdutos> ToTamanhosProdutos(Guid produtoId)
    {
        var tamanhosProdutos = new List<TamanhosProdutos>();

        if (TamanhosIds == null || TamanhosIds.Count == 0)
            return tamanhosProdutos;

        foreach (var tamanhoId in TamanhosIds)
        {
            tamanhosProdutos.Add(new TamanhosProdutos(Guid.NewGuid(), produtoId, tamanhoId));
        }

        return tamanhosProdutos;
    }

    public IList<PesosProdutos> ToPesosProdutos(Guid produtoId)
    {
        var pesosProdutos = new List<PesosProdutos>();

        if (PesosIds?.Count == 0 || PesosIds == null)
            return pesosProdutos;


        foreach (var pesoId in PesosIds)
        {
            pesosProdutos.Add(new PesosProdutos(Guid.NewGuid(), produtoId, pesoId));
        }

        return pesosProdutos;
    }
}

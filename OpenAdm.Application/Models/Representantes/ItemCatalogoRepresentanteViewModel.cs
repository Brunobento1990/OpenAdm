using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Representantes;

public sealed class ItemCatalogoRepresentanteViewModel
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid? PesoId { get; set; }
    public string? FotoProduto { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }

    public static ItemCatalogoRepresentanteViewModel FromEntity(ItemTabelaDePreco item)
    {
        var descricaoTamanho = string.IsNullOrWhiteSpace(item.Tamanho?.Descricao)
            ? ""
            : $"- {item.Tamanho?.Descricao}";

        var descricaoPeso = string.IsNullOrWhiteSpace(item.Peso?.Descricao)
            ? ""
            : $"- {item.Peso?.Descricao}";

        return new()
        {
            Id = item.Id,
            ProdutoId = item.ProdutoId,
            TamanhoId = item.TamanhoId,
            PesoId = item.PesoId,
            FotoProduto = item.Produto.UrlFoto,
            Descricao = $"{item.Produto.Descricao} {descricaoTamanho} {descricaoPeso}".Trim(),
            ValorUnitarioAtacado = item.ValorUnitarioAtacado,
            ValorUnitarioVarejo = item.ValorUnitarioVarejo
        };
    }
}
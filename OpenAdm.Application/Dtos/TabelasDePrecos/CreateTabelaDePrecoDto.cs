using OpenAdm.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateTabelaDePrecoDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
    public bool AtivaEcommerce { get; set; }
    public IList<CreateItensTabelaDePrecoDto> ItensTabelaDePreco { get; set; }
        = new List<CreateItensTabelaDePrecoDto>();

    public TabelaDePreco ToEntity()
    {
        var date = DateTime.Now;

        var tabelaDePreco = new TabelaDePreco(
            Guid.NewGuid(),
            date,
            date,
            0,
            Descricao,
            false);

        tabelaDePreco.ItensTabelaDePreco = ItensTabelaDePreco
            .Select(x =>
                new ItemTabelaDePreco(
                    Guid.NewGuid(),
                    date,
                    date,
                    0,
                    x.ProdutoId,
                    x.ValorUnitarioAtacado,
                    x.ValorUnitarioVarejo,
                    tabelaDePreco.Id,
                    x.TamanhoId,
                    x.PesoId))
            .ToList();

        return tabelaDePreco;
    }
}

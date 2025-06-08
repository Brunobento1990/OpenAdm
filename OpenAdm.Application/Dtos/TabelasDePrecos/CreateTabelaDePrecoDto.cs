using OpenAdm.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateTabelaDePrecoDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
    public bool AtivaEcommerce { get; set; }

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

        return tabelaDePreco;
    }
}

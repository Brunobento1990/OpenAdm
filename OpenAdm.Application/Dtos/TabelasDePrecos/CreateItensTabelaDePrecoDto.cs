
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateItensTabelaDePrecoDto
{
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid TabelaDePrecoId { get; set; }
    public Guid? PesoId { get; set; }

    public ItemTabelaDePreco ToEntity()
    {
        var date = DateTime.Now;

        return new ItemTabelaDePreco(
                Guid.NewGuid(),
                date,
                date,
                0,
                ProdutoId,
                ValorUnitarioAtacado,
                ValorUnitarioVarejo,
                TabelaDePrecoId,
                TamanhoId,
                PesoId);
    }
}


using Domain.Pkg.Entities;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateItensTabelaDePrecoDto
{
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid TabelaDePrecoId { get; set; }
    public Guid? PesoId { get; set; }

    public ItensTabelaDePreco ToEntity()
    {
        var date = DateTime.Now;

        return new ItensTabelaDePreco(
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

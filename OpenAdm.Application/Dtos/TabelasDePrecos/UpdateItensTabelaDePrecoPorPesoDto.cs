using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class UpdateItensTabelaDePrecoPorPesoDto
{
    public Guid PesoId { get; set; }
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }

    public void Validar()
    {
        if (ValorUnitarioAtacado <= 0 && ValorUnitarioVarejo <= 0)
        {
            throw new ExceptionApi("Informe o valor de atacado ou de varejo");
        }

        if (PesoId == Guid.Empty)
        {
            throw new ExceptionApi("Informe o peso a ser atualizado");
        }
    }
}

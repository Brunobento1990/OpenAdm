using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class UpdateItemTabelaDePrecoDto
{
    public Guid Id { get; set; }
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }

    public void Validar()
    {
        if (Id == Guid.Empty)
            throw new ExceptionApi("Informe o item da tabela de preço");

        if (ValorUnitarioAtacado <= 0 && ValorUnitarioVarejo <= 0)
            throw new ExceptionApi("Informe o valor de atacado ou de varejo");
    }
}

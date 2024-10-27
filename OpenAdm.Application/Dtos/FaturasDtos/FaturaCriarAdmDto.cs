using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class FaturaCriarAdmDto
{
    public Guid UsuarioId { get; set; }
    public Guid? PedidoId { get; set; }
    public TipoFaturaEnum Tipo { get; set; }
    public IList<ParcelaCriarAdmDto> Parcelas { get; set; } = [];

    public void Validar()
    {
        if (Parcelas.Count == 0)
        {
            throw new ExceptionApi("Informe as parcelas!");
        }

        var temParcelaZerada = Parcelas.Any(x => x.Valor == 0);
        if (temParcelaZerada)
        {
            throw new ExceptionApi("Não é possível informar parcelas com o valor zero!");
        }
    }
}

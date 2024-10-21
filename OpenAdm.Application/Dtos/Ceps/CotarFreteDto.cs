using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Ceps;

public class CotarFreteDto
{
    public string CepDestino { get; set; } = string.Empty;
    public Guid PedidoId { get; set; }

    public void Validar()
    {
        if (!CepHelper.ValidarCep(CepDestino))
        {
            throw new ExceptionApi("CEP inválido!");
        }
    }
}

using OpenAdm.Application.Dtos.Fretes;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Interfaces;

public interface IFreteService
{
    Task<FreteViewModel> CalcularAsync(CalcularFretePedidoDto calcularFretePedidoDto);
}
